﻿using SecondMonitor.SimdataManagement.SimSettings;

namespace SecondMonitor.Timing.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using WindowsControls.Extension;
    using Contracts.Commands;
    using DataModel.Snapshot;
    using PluginManager.Core;
    using PluginManager.GameConnector;
    using LapTimings.ViewModel;
    using Presentation.View;
    using Presentation.ViewModel;
    using SecondMonitor.Telemetry.TelemetryApplication.Controllers;
    using SecondMonitor.Telemetry.TelemetryManagement.Repository;
    using SimdataManagement;
    using SimdataManagement.DriverPresentation;
    using Telemetry;
    using TelemetryPresentation.MainWindow;
    using ViewModels.Settings.ViewModel;
    using System.Windows;
    using Contracts.NInject;
    using Ninject.Syntax;
    using Rating.Application.Championship;
    using Rating.Application.Championship.Controller;
    using Rating.Application.Rating.Controller;
    using Rating.Application.Rating.RatingProvider.FieldRatingProvider.ReferenceRatingProviders;
    using ReportCreation.ViewModel;
    using SessionTiming.Drivers.Presentation.ViewModel;
    using TrackRecords.Controller;
    using ViewModels.SessionEvents;
    using ViewModels.Settings;
    using ViewModels.Settings.Model;
    using ViewModels.SimulatorContent;
    using ViewModels.SplashScreen;

    public class TimingApplicationController : ISecondMonitorPlugin
    {
        private const string MapFolder = "TrackMaps";
        private const string SettingsFolder = "Settings";
        private const string TelemetryFolder = "Telemetry";
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SecondMonitor\\settings.json");

        private readonly KernelWrapper _kernelWrapper;
        private TimingDataViewModel _timingDataViewModel;
        private SimSettingController _simSettingController;
        private DisplaySettingsWindow _settingsWindow;
        private PluginsManager _pluginsManager;
        private TimingGui _timingGui;
        private DisplaySettingsViewModel _displaySettingsViewModel;
        private MapManagementController _mapManagementController;
        private DriverPresentationsManager _driverPresentationsManager;
        private ISessionTelemetryControllerFactory _sessionTelemetryControllerFactory;
        private readonly DisplaySettingsLoader _displaySettingsLoader;
        private ReportsController _reportsController;
        private readonly IRatingApplicationController _ratingApplicationController;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ISimulatorContentController _simulatorContentController;
        private readonly ITrackRecordsController _trackRecordsController;
        private readonly IChampionshipController _championshipController;
        private readonly ISessionEventsController _sessionEventsController;
        private readonly ISessionEventProvider _sessionEventProvider;
        private readonly IChampionshipCurrentEventPointsProvider _championshipCurrentEventPointsProvider;
        private Window _splashScreen;

        public TimingApplicationController()
        {
            _kernelWrapper = new KernelWrapper();
            _displaySettingsLoader = new DisplaySettingsLoader();
            _ratingApplicationController = _kernelWrapper.Get<IRatingApplicationController>();
            _settingsProvider = _kernelWrapper.Get<ISettingsProvider>();
            _simulatorContentController = _kernelWrapper.Get<ISimulatorContentController>();
            _trackRecordsController = _kernelWrapper.Get<ITrackRecordsController>();
            _championshipController = _kernelWrapper.Get<IChampionshipController>();
            _sessionEventsController = _kernelWrapper.Get<ISessionEventsController>();
            _sessionEventProvider = _kernelWrapper.Get<ISessionEventProvider>();
            _championshipCurrentEventPointsProvider = _kernelWrapper.Get<IChampionshipCurrentEventPointsProvider>();
        }

        public PluginsManager PluginManager
        {
            get => _pluginsManager;
            set
            {
                _pluginsManager = value;
                _pluginsManager.DataLoaded += OnDataLoaded;
                _pluginsManager.SessionStarted += OnSessionStarted;
                _pluginsManager.DisplayMessage += DisplayMessage;
            }
        }

        public bool IsDaemon => false;

        public string PluginName => "Timing UI";

        public bool IsEnabledByDefault => true;


        public async Task RunPlugin()
        {
            CreateDisplaySettingsViewModel();
            ShowSplashScreen();

            CreateReportsController();
            CreateSimSettingsController();
            CreateMapManagementController();
            CreateDriverPresentationManager();
            CreateSessionTelemetryControllerFactory();
            await StartControllers();
            DriverLapsWindowManager driverLapsWindowManager = new DriverLapsWindowManager(() => _timingGui, () => _timingDataViewModel.SelectedDriverTiming, _driverPresentationsManager);
            _timingDataViewModel = new TimingDataViewModel(driverLapsWindowManager, _settingsProvider, _driverPresentationsManager, _sessionTelemetryControllerFactory, _ratingApplicationController.RatingProvider,
                _trackRecordsController, _championshipCurrentEventPointsProvider, _sessionEventProvider) {MapManagementController = _mapManagementController};
            _timingDataViewModel.SessionCompleted+=TimingDataViewModelOnSessionCompleted;
            _timingDataViewModel.RatingApplicationViewModel = _ratingApplicationController.RatingApplicationViewModel;
            _timingDataViewModel.ChampionshipIconStateViewModel = _championshipController.ChampionshipIconStateViewModel;
            BindCommands();
            CreateGui();
            _timingDataViewModel.GuiDispatcher = _timingGui.Dispatcher;
            _timingDataViewModel?.Reset();
            await _simulatorContentController.StartControllerAsync();
            await _trackRecordsController.StartControllerAsync();

            HideSplashScreen();
        }

        private void HideSplashScreen()
        {
            _splashScreen.Close();
        }

        private void ShowSplashScreen()
        {
            var splashScreenViewModel = _kernelWrapper.Get<SplashScreenViewModel>();
            splashScreenViewModel.PrimaryInformation = "Loading...";
            _splashScreen = new Window
            {
                Content = splashScreenViewModel,
                Title = "Starting",
                SizeToContent = SizeToContent.Manual,
                WindowStartupLocation = WindowStartupLocation.Manual,
                ShowInTaskbar = false,
            };

            _splashScreen.Show();

            if (_displaySettingsViewModel?.WindowLocationSetting == null)
            {
                return;
            }

            _splashScreen.WindowStartupLocation = WindowStartupLocation.Manual;
            _splashScreen.Left = _displaySettingsViewModel.WindowLocationSetting.Left;
            _splashScreen.Top = _displaySettingsViewModel.WindowLocationSetting.Top;
            _splashScreen.WindowState = WindowState.Normal;
            _splashScreen.WindowState = (WindowState)_displaySettingsViewModel.WindowLocationSetting.WindowState;
            if (_splashScreen.WindowState == WindowState.Maximized)
            {
                _splashScreen.WindowStyle = WindowStyle.None;
            }
        }

        private async Task StartControllers()
        {
            await _ratingApplicationController.StartControllerAsync();
            await _championshipController.StartControllerAsync();
            await _sessionEventsController.StartControllerAsync();
        }

        private void CreateReportsController()
        {
            _reportsController = new ReportsController(_displaySettingsViewModel);
        }

        private void CreateSessionTelemetryControllerFactory()
        {
            _sessionTelemetryControllerFactory = new SessionTelemetryControllerFactory(new TelemetryRepository(Path.Combine(_displaySettingsViewModel.ReportingSettingsView.ExportDirectoryReplacedSpecialDirs, TelemetryFolder), _displaySettingsViewModel.TelemetrySettingsViewModel.MaxSessionsKept));
        }


        private void DisplayMessage(object sender, MessageArgs e)
        {
            _timingDataViewModel?.DisplayMessage(e);
        }

        private void OnSessionStarted(object sender, DataEventArgs e)
        {
            _sessionEventsController.Reset();
            _sessionEventsController.Visit(e.Data);
            _ratingApplicationController.NotifyDataLoaded(e.Data);
            _trackRecordsController.OnSessionStarted(e.Data);
            _timingDataViewModel?.StartNewSession(e.Data);
        }

        private async void OnDataLoaded(object sender, DataEventArgs e)
        {
            SimulatorDataSet dataSet = e.Data;
            try
            {
                _sessionEventsController.Visit(dataSet);
                await _ratingApplicationController.NotifyDataLoaded(dataSet);
                _simSettingController?.ApplySimSettings(dataSet);
                _simulatorContentController.Visit(dataSet);

            }
            catch (SimSettingsException ex)
            {
                _timingDataViewModel.DisplayMessage(new MessageArgs(ex.Message));
                _simSettingController?.ApplySimSettings(dataSet);
            }

            _timingDataViewModel.ApplyDateSet(dataSet);
        }

        private void CreateSimSettingsController()
        {
            _simSettingController = new SimSettingController(_displaySettingsViewModel, _kernelWrapper.Get<ICarSpecificationProvider>(), _kernelWrapper.Get<IResolutionRoot>());
        }

        private void CreateGui()
        {
            _timingGui = new TimingGui(false);
            _timingGui.Show();
            _timingGui.Closed += OnGuiClosed;
            _timingGui.MouseLeave += GuiOnMouseLeave;

            if (_displaySettingsViewModel?.WindowLocationSetting != null)
            {
                _timingGui.WindowStartupLocation = WindowStartupLocation.Manual;
                _timingGui.Left = _displaySettingsViewModel.WindowLocationSetting.Left;
                _timingGui.Top = _displaySettingsViewModel.WindowLocationSetting.Top;
                _timingGui.WindowState = WindowState.Normal;
                _timingGui.WindowState = (WindowState)_displaySettingsViewModel.WindowLocationSetting.WindowState;
                if (_timingGui.WindowState == WindowState.Maximized)
                {
                    _timingGui.WindowStyle = WindowStyle.None;
                }
            }

            _timingGui.DataContext = _timingDataViewModel;
            Application.Current.MainWindow = _timingGui;
        }

        private async void OnGuiClosed(object sender, EventArgs e)
        {
            await _ratingApplicationController.StopControllerAsync();
            await _championshipController.StopControllerAsync();
            await _sessionEventsController.StopControllerAsync();
            _displaySettingsViewModel.WindowLocationSetting = new WindowLocationSetting()
            {
                Left = _timingGui.Left,
                Top = _timingGui.Top,
                WindowState = (int) _timingGui.WindowState
            };
            _timingGui = null;
            List<Exception> exceptions = new List<Exception>();
            _driverPresentationsManager.SavePresentations();
            _timingDataViewModel.SessionCompleted -= TimingDataViewModelOnSessionCompleted;
            _timingDataViewModel?.TerminatePeriodicTask(exceptions);
            _displaySettingsLoader.TrySaveDisplaySettings(_displaySettingsViewModel.SaveToNewModel(), SettingsPath);
            await _trackRecordsController.StopControllerAsync();
            await _simulatorContentController.StopControllerAsync();
            await _pluginsManager.DeletePlugin(this, exceptions);
        }

        private void GuiOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_timingGui != null)
            {
                _timingGui.DtTimig.SelectedItem = null;
            }
        }

        private void BindCommands()
        {
            _timingDataViewModel.RightClickCommand = new RelayCommand(UnSelectItem);
            _timingDataViewModel.OpenSettingsCommand = new RelayCommand(OpenSettingsWindow);
            _timingDataViewModel.ScrollToPlayerCommand = new RelayCommand(() => ScrollToPlayer(_timingDataViewModel.TimingDataGridViewModel.PlayerViewModel));
            _timingDataViewModel.OpenCarSettingsCommand = new RelayCommand(OpenCarSettingsWindow);
            _timingDataViewModel.OpenCurrentTelemetrySession = new AsyncCommand(OpenCurrentTelemetrySession);
            _timingDataViewModel.OpenLastReportCommand = new RelayCommand(_reportsController.OpenLastReport);
            _timingDataViewModel.OpenReportFolderCommand = new RelayCommand(_reportsController.OpenReportsFolder);
            _timingDataViewModel.OpenChampionshipWindowCommand = new RelayCommand(_championshipController.OpenChampionshipWindow);
        }

        private void UnSelectItem()
        {
            _timingDataViewModel.SelectedDriverTimingViewModel = null;
        }

        private void CreateDisplaySettingsViewModel()
        {
            _displaySettingsViewModel = _settingsProvider.DisplaySettingsViewModel;
            FillDisplaySettingsOptions();
        }

        private void FillDisplaySettingsOptions()
        {
            _displaySettingsViewModel.RatingSettingsViewModel.AvailableReferenceRatingProviders = _kernelWrapper.Get<IReferenceRatingProviderFactory>().GetAvailableReferenceRatingsProviders();
        }

        private void CreateMapManagementController()
        {
            _mapManagementController = new MapManagementController(new TrackMapFromTelemetryFactory(TimeSpan.FromMilliseconds(_displaySettingsViewModel.MapDisplaySettingsViewModel.MapPointsInterval),100),
                new MapsLoader(Path.Combine(_displaySettingsViewModel.ReportingSettingsView.ExportDirectoryReplacedSpecialDirs, MapFolder)),
                new TrackDtoManipulator());
        }

        private void CreateDriverPresentationManager()
        {
            _driverPresentationsManager = new DriverPresentationsManager(new DriverPresentationsLoader(Path.Combine(_displaySettingsViewModel.ReportingSettingsView.ExportDirectoryReplacedSpecialDirs, SettingsFolder)));
        }

        private void OpenSettingsWindow()
        {
            if (_settingsWindow != null && _settingsWindow.IsVisible)
            {
                _settingsWindow.Focus();
                return;
            }

            _settingsWindow = new DisplaySettingsWindow
                                  {
                                      DataContext = _displaySettingsViewModel,
                                      Owner = _timingGui
                                  };
            _settingsWindow.Show();
        }

        private async Task OpenCurrentTelemetrySession()
        {
            MainWindow mainWindow = new MainWindow();
            TelemetryApplicationController controller = new TelemetryApplicationController(mainWindow);
            await controller.StartControllerAsync();
            mainWindow.Closed += async (sender, args) =>
            {
                await controller.StopControllerAsync();
            };
            await controller.OpenLastSessionFromRepository();
        }

        private void OpenCarSettingsWindow()
        {
            _simSettingController.OpenCarSettingsControl(_timingGui);
        }

        private void ScrollToPlayer(DriverTimingViewModel driverTimingViewModel)
        {
            if (_displaySettingsViewModel.ScrollToPlayer && _timingGui != null && driverTimingViewModel != null)
            {
                _timingGui.DtTimig.ScrollToCenterOfView(driverTimingViewModel);
            }
        }

        private async void TimingDataViewModelOnSessionCompleted(object sender, SessionSummaryEventArgs e)
        {
            await _ratingApplicationController.NotifySessionCompletion(e.Summary);
            _reportsController?.CreateReport(e.Summary);
        }
    }
}