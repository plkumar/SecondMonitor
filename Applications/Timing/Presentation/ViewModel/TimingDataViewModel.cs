﻿namespace SecondMonitor.Timing.Presentation.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using WindowsControls.SituationOverview;
    using Commands;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;

    using PluginManager.GameConnector;

    using DataModel.Extensions;
    using Controllers;
    using NLog;
    using Rating.Application.Championship;
    using Rating.Application.Championship.ViewModels.IconState;
    using Rating.Application.Rating.RatingProvider;
    using Rating.Application.Rating.ViewModels;
    using ReportCreation;
    using SecondMonitor.Timing.LapTimings.ViewModel;
    using SecondMonitor.Timing.SessionTiming.Drivers.Presentation.ViewModel;
    using SecondMonitor.Timing.SessionTiming.ViewModel;
    using SessionTiming;
    using ViewModels;
    using ViewModels.CarStatus;
    using ViewModels.TrackInfo;

    using SessionTiming.Drivers;
    using SimdataManagement.DriverPresentation;
    using Telemetry;
    using TrackRecords.Controller;
    using ViewModels.Colors;
    using ViewModels.Controllers;
    using ViewModels.SessionEvents;
    using ViewModels.Settings;
    using ViewModels.Settings.Model;
    using ViewModels.Settings.ViewModel;
    using ViewModels.TrackRecords;

    public class TimingDataViewModel : AbstractViewModel, ISimulatorDataSetViewModel,  IPaceProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly DriverLapsWindowManager _driverLapsWindowManager;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ISessionTelemetryControllerFactory _sessionTelemetryControllerFactory;
        private readonly IRatingProvider _ratingProvider;
        private readonly ITrackRecordsController _trackRecordsController;
        private readonly IChampionshipCurrentEventPointsProvider _championshipCurrentEventPointsProvider;
        private readonly ISessionEventProvider _sessionEventProvider;

        private ICommand _resetCommand;

        private TimingDataViewModelResetModeEnum _shouldReset = TimingDataViewModelResetModeEnum.NoReset;

        private SessionTiming _sessionTiming;
        private SessionType _sessionType = SessionType.Na;
        private SimulatorDataSet _lastDataSet;
        private bool _isOpenCarSettingsCommandEnable;

        private bool _isNamesNotUnique;
        private string _notUniqueNamesMessage;
        private Stopwatch _notUniqueCheckWatch;

        private Task _refreshGuiTask;
        private Task _refreshBasicInfoTask;
        private Task _refreshTimingCircleTask;
        private Task _refreshTimingGridTask;

        private string _connectedSource;
        private MapManagementController _mapManagementController;
        private DisplaySettingsViewModel _displaySettingsViewModel;


        public TimingDataViewModel(DriverLapsWindowManager driverLapsWindowManager, ISettingsProvider settingsProvider, DriverPresentationsManager driverPresentationsManager,
            ISessionTelemetryControllerFactory sessionTelemetryControllerFactory, IRatingProvider ratingProvider, ITrackRecordsController trackRecordsController, IChampionshipCurrentEventPointsProvider championshipCurrentEventPointsProvider,
            ISessionEventProvider sessionEventProvider)
        {
            TimingDataGridViewModel = new TimingDataGridViewModel(driverPresentationsManager, settingsProvider.DisplaySettingsViewModel, new ClassColorProvider(new BasicColorPaletteProvider()));
            SessionInfoViewModel = new SessionInfoViewModel();
            TrackInfoViewModel = new TrackInfoViewModel();
            _driverLapsWindowManager = driverLapsWindowManager;
            _settingsProvider = settingsProvider;
            _sessionTelemetryControllerFactory = sessionTelemetryControllerFactory;
            _ratingProvider = ratingProvider;
            _trackRecordsController = trackRecordsController;
            _championshipCurrentEventPointsProvider = championshipCurrentEventPointsProvider;
            _sessionEventProvider = sessionEventProvider;
            DoubleLeftClickCommand = _driverLapsWindowManager.OpenWindowCommand;
            DisplaySettingsViewModel = settingsProvider.DisplaySettingsViewModel;
            TrackRecordsViewModel = _trackRecordsController.TrackRecordsViewModel;
            SituationOverviewProvider = new SituationOverviewProvider(TimingDataGridViewModel, settingsProvider.DisplaySettingsViewModel);
        }

        public event EventHandler<SessionSummaryEventArgs> SessionCompleted;

        public TimeSpan? PlayersPace => SessionTiming?.Player?.Pace;
        public TimeSpan? LeadersPace => SessionTiming?.Leader?.Pace;

        public DisplaySettingsViewModel DisplaySettingsViewModel
        {
            get => _displaySettingsViewModel;
            private set
            {
                _displaySettingsViewModel = value;
                NotifyPropertyChanged();
                DisplaySettingsChanged();
            }
        }

        private SessionOptionsViewModel _currentSessionOptionsView;
        public SessionOptionsViewModel CurrentSessionOptionsView
        {
            get => _currentSessionOptionsView;
            set
            {
                SetProperty(ref _currentSessionOptionsView, value);
                ChangeOrderingMode();
                ChangeTimeDisplayMode();
            }
        }

        public MapManagementController MapManagementController
        {
            set
            {
                _mapManagementController = value;
                SituationOverviewProvider.MapManagementController = value;
                value.SessionTiming = SessionTiming;
            }
        }

        public bool IsNamesNotUnique
        {
            get => _isNamesNotUnique;
            private set => SetProperty(ref _isNamesNotUnique, value);
        }

        public string NotUniqueNamesMessage
        {
            get => _notUniqueNamesMessage;
            private set => SetProperty(ref _notUniqueNamesMessage, value);
        }

        public ITrackRecordsViewModel TrackRecordsViewModel { get; }

        public TimingDataGridViewModel TimingDataGridViewModel { get; }

        public int SessionCompletedPercentage => _sessionTiming?.SessionCompletedPerMiles ?? 50;

        public ICommand ResetCommand => _resetCommand ?? (_resetCommand = new NoArgumentCommand(ScheduleReset));

        public ICommand OpenSettingsCommand { get; set; }

        public ICommand RightClickCommand { get; set; }

        public ICommand ScrollToPlayerCommand { get; set; }

        public ICommand OpenCurrentTelemetrySession { get; set; }

        public ICommand OpenCarSettingsCommand { get; set; }

        public ICommand OpenChampionshipWindowCommand { get; set; }

        public ChampionshipIconStateViewModel ChampionshipIconStateViewModel { get; set; }

        public bool IsOpenCarSettingsCommandEnable
        {
            get => _isOpenCarSettingsCommandEnable;
            set
            {
                _isOpenCarSettingsCommandEnable = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand DoubleLeftClickCommand
        {
            get;
            set;
        }

        public string SessionTime => _sessionTiming?.SessionTime.FormatToDefault() ?? string.Empty;

        public string ConnectedSource
        {
            get => _connectedSource;
            set => SetProperty(ref _connectedSource, value);
        }

        public string SystemTime => DateTime.Now.ToString("HH:mm");

        public string Title
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string version = AssemblyName.GetAssemblyName(assembly.Location).Version.ToString();
                return "Second Monitor (Timing)(v" + version + " )";
            }
        }

        public SessionInfoViewModel SessionInfoViewModel { get; }

        public TrackInfoViewModel TrackInfoViewModel { get; }

        public SituationOverviewProvider SituationOverviewProvider { get; }

        public Dispatcher GuiDispatcher { get; set; }

        private DriverTimingViewModel _selectedDriverTimingViewModel;
        public DriverTimingViewModel SelectedDriverTimingViewModel
        {
            get => _selectedDriverTimingViewModel;
            set => SetProperty(ref _selectedDriverTimingViewModel, value);
        }

        public DriverTimingViewModel SelectedDriverTiming => SelectedDriverTimingViewModel;

        public SessionTiming SessionTiming
        {
            get => _sessionTiming;
            private set => SetProperty(ref _sessionTiming, value);
        }

        public CarStatusViewModel CarStatusViewModel
        {
            get;
            private set;
        }

        private IRatingApplicationViewModel _ratingApplicationViewModel;

        public IRatingApplicationViewModel RatingApplicationViewModel
        {
            get => _ratingApplicationViewModel;
            set => SetProperty(ref _ratingApplicationViewModel, value);
        }

        private bool TerminatePeriodicTasks { get; set; }

        public ICommand OpenLastReportCommand { get; set; }
        public ICommand OpenReportFolderCommand { get; set; }

        public void TerminatePeriodicTask(List<Exception> exceptions)
        {
            TerminatePeriodicTasks = true;
            if (_refreshBasicInfoTask.IsFaulted && _refreshBasicInfoTask.Exception != null)
            {
                exceptions.AddRange(_refreshBasicInfoTask.Exception.InnerExceptions);
            }

            if (_refreshGuiTask.IsFaulted && _refreshGuiTask.Exception != null)
            {
                exceptions.AddRange(_refreshGuiTask.Exception.InnerExceptions);
            }

            if (_refreshTimingCircleTask.IsFaulted && _refreshTimingCircleTask.Exception != null)
            {
                exceptions.AddRange(_refreshTimingCircleTask.Exception.InnerExceptions);
            }

            if (_refreshTimingGridTask.IsFaulted && _refreshTimingGridTask.Exception != null)
            {
                exceptions.AddRange(_refreshTimingGridTask.Exception.InnerExceptions);
            }
        }

        public void ApplyDateSet(SimulatorDataSet data)
        {
            if (data == null)
            {
                return;
            }

            _lastDataSet = data;
            IsOpenCarSettingsCommandEnable = !string.IsNullOrWhiteSpace(data?.PlayerInfo?.CarName);
            ConnectedSource = _lastDataSet?.Source;
            if (_sessionTiming == null || data.SessionInfo.SessionType == SessionType.Na)
            {
                return;
            }

            if (_sessionType != data.SessionInfo.SessionType)
            {
                _shouldReset = TimingDataViewModelResetModeEnum.Automatic;
                _sessionType = _sessionTiming.SessionType;
            }

            // Reset state was detected (either reset button was pressed or timing detected a session change)
            if (_shouldReset != TimingDataViewModelResetModeEnum.NoReset)
            {
                CheckAndNotifySessionCompleted();
                CreateTiming(data);
                _shouldReset = TimingDataViewModelResetModeEnum.NoReset;
            }

            try
            {
                CheckNamesUniques(data);
                _sessionTiming?.UpdateTiming(data);
                CarStatusViewModel?.PedalsAndGearViewModel?.ApplyDateSet(data);
            }
            catch (SessionTiming.DriverNotFoundException)
            {
                _shouldReset = TimingDataViewModelResetModeEnum.Automatic;
            }
        }

        public void DisplayMessage(MessageArgs e)
        {
            if (e.IsDecision)
            {
                if (MessageBox.Show(
                        e.Message,
                        "Message from connector.",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    e.Action();
                }
            }
            else
            {
                MessageBox.Show(e.Message, "Message from connector.", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        public void Reset()
        {
            CarStatusViewModel = new CarStatusViewModel(this, _settingsProvider);
            ConnectedSource = "Not Connected";

            if (GuiDispatcher != null && GuiDispatcher.CheckAccess())
            {
                GuiDispatcher.Invoke(ScheduleRefreshActions);
            }
            else
            {
                ScheduleRefreshActions();
            }

            OnDisplaySettingsChange(this, null);
            _shouldReset = TimingDataViewModelResetModeEnum.NoReset;
            new AutoUpdateController().CheckForUpdate();
        }

        private void ScheduleRefreshActions()
        {
            _refreshGuiTask = SchedulePeriodicAction(() => RefreshGui(_lastDataSet), () => 2000, this, true);
            _refreshBasicInfoTask = SchedulePeriodicAction(() => RefreshBasicInfo(_lastDataSet), () => 100, this, true);
            _refreshTimingCircleTask = SchedulePeriodicAction(() => RefreshTimingCircle(_lastDataSet), () => 100, this, true);
            _refreshTimingGridTask = SchedulePeriodicAction(() => RefreshTimingGrid(_lastDataSet), () => DisplaySettingsViewModel.RefreshRate, this, false);
        }

        private void RefreshTimingGrid(SimulatorDataSet lastDataSet)
        {
            TimingDataGridViewModel.UpdateProperties(lastDataSet);
        }

        private void PaceLapsChanged()
        {
            if (_sessionTiming != null)
            {
                _sessionTiming.PaceLaps = DisplaySettingsViewModel.PaceLaps;
            }

        }

        private void CheckNamesUniques(SimulatorDataSet dataSet)
        {
            if (_notUniqueCheckWatch == null || _notUniqueCheckWatch.ElapsedMilliseconds < 10000)
            {
                return;
            }

            List<IGrouping<string, string>> namesGrouping = dataSet.DriversInfo.Select(x => x.DriverName).GroupBy(x => x).ToList();

            List<string> uniqueNames = namesGrouping.Where(x => x.Count() == 1).SelectMany(x => x).ToList();
            List<string> notUniqueNames = namesGrouping.Where(x => x.Count() > 1).Select(x => x.Key).ToList();


            if (notUniqueNames.Count == 0)
            {
                IsNamesNotUnique = false;
                return;
            }

            IsNamesNotUnique = true;
            NotUniqueNamesMessage = $"Not All Driver Names are unique: Number of unique drivers - {uniqueNames.Count}, Not unique names - {string.Join(", ", notUniqueNames)} ";
            _notUniqueCheckWatch.Restart();
        }

        private void ScheduleReset()
        {
            _shouldReset = TimingDataViewModelResetModeEnum.Manual;
        }

        private void ChangeOrderingMode()
        {
            if (_sessionTiming == null)
            {
                return;
            }

            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(ChangeOrderingMode);
                return;
            }

            var mode = GetOrderTypeFromSettings();
            TimingDataGridViewModel.DriversOrdering = mode;
            _sessionTiming.DisplayGapToPlayerRelative = mode != DisplayModeEnum.Absolute;

        }

        private void ChangeTimeDisplayMode()
        {
            if (_sessionTiming == null || GuiDispatcher == null)
            {
                return;
            }

            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(ChangeTimeDisplayMode);
                return;
            }

            var mode = GetTimeDisplayTypeFromSettings();
            _sessionTiming.DisplayBindTimeRelative = mode == DisplayModeEnum.Relative;
            _sessionTiming.DisplayGapToPlayerRelative = mode == DisplayModeEnum.Relative;
        }

        private DisplayModeEnum GetOrderTypeFromSettings()
        {
            return CurrentSessionOptionsView.OrderingMode;
        }

        private DisplayModeEnum GetTimeDisplayTypeFromSettings()
        {
            return CurrentSessionOptionsView.TimesDisplayMode;
        }

        private SessionOptionsViewModel GetSessionOptionOfCurrentSession(SimulatorDataSet dataSet)
        {
            if (dataSet == null)
            {
                return new SessionOptionsViewModel();
            }

            switch (dataSet.SessionInfo.SessionType)
            {
                case SessionType.Practice:
                case SessionType.WarmUp:
                    return DisplaySettingsViewModel.PracticeSessionDisplayOptionsView;
                case SessionType.Qualification:
                    return DisplaySettingsViewModel.QualificationSessionDisplayOptionsView;
                case SessionType.Race:
                    return DisplaySettingsViewModel.RaceSessionDisplayOptionsView;
                default:
                    return new SessionOptionsViewModel();
            }
        }

        private void RefreshTimingCircle(SimulatorDataSet data)
        {

            if (data == null || GuiDispatcher == null)
            {
                return;
            }

            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => RefreshTimingCircle(data));
                return;
            }

            SituationOverviewProvider.ApplyDateSet(data);
        }

        private void RefreshBasicInfo(SimulatorDataSet data)
        {
            if (data == null)
            {
                return;
            }

            NotifyPropertyChanged(nameof(SessionTime));
            NotifyPropertyChanged(nameof(SystemTime));
            NotifyPropertyChanged(nameof(SessionCompletedPercentage));
            CarStatusViewModel.ApplyDateSet(data);
            TrackInfoViewModel.ApplyDateSet(data);
            SessionInfoViewModel.ApplyDateSet(data);
        }

        private void SessionTimingDriverRemoved(object sender, DriverListModificationEventArgs e)
        {
            if (TerminatePeriodicTasks)
            {
                return;
            }
            TimingDataGridViewModel.RemoveDriver(e.Data);

            GuiDispatcher?.Invoke(() =>
            {
                SituationOverviewProvider.RemoveDriver(e.Data.DriverInfo);
            });

        }

        private void SessionTimingDriverAdded(object sender, DriverListModificationEventArgs e)
        {

            TimingDataGridViewModel.AddDriver(e.Data);

            GuiDispatcher?.Invoke(() =>
            {
                SituationOverviewProvider.AddDriver(e.Data.DriverInfo);
            });

            _driverLapsWindowManager.Rebind(TimingDataGridViewModel.DriversViewModels.FirstOrDefault(x => x.Name == e.Data.Name));
        }

        private void RefreshGui(SimulatorDataSet data)
        {
            if (data == null)
            {
                return;
            }

            ScrollToPlayerCommand?.Execute(null);
        }

        private void CreateTiming(SimulatorDataSet data)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => CreateTiming(data));
                return;
            }

            if (SessionTiming != null)
            {
                SessionTiming.DriverAdded -= SessionTimingDriverAdded;
                SessionTiming.DriverRemoved -= SessionTimingDriverRemoved;
                SessionTiming.LapCompleted -= SessionTimingOnLapCompleted;
            }

            bool invalidateLap = _shouldReset == TimingDataViewModelResetModeEnum.Manual ||
                                data.SessionInfo.SessionType != SessionType.Race;
            _lastDataSet = data;
            SessionTiming = SessionTiming.FromSimulatorData(data, invalidateLap, this, _sessionTelemetryControllerFactory, _ratingProvider, _trackRecordsController, _championshipCurrentEventPointsProvider, _sessionEventProvider);
            SessionInfoViewModel.SessionTiming = _sessionTiming;
            SessionTiming.DriverAdded += SessionTimingDriverAdded;
            SessionTiming.DriverRemoved += SessionTimingDriverRemoved;
            SessionTiming.LapCompleted += SessionTimingOnLapCompleted;
            SessionTiming.PaceLaps = DisplaySettingsViewModel.PaceLaps;

            CarStatusViewModel.Reset();
            TrackInfoViewModel.Reset();
            SituationOverviewProvider.Reset();
            if (_mapManagementController != null)
            {
                _mapManagementController.SessionTiming = _sessionTiming;
            }

            InitializeGui(data);
            ChangeTimeDisplayMode();
            ChangeOrderingMode();
            ConnectedSource = data.Source;
            _notUniqueCheckWatch = Stopwatch.StartNew();
            NotifyPropertyChanged(nameof(ConnectedSource));
        }

        private async void SessionTimingOnLapCompleted(object sender, LapEventArgs e)
        {
            try
            {
                if (e.Lap.Driver.IsPlayer && e.Lap.Driver.Session.SessionType == SessionType.Race)
                {
                    await TimingDataGridViewModel.UpdateAndShowPitBoard();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void StartNewSession(SimulatorDataSet dataSet)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => StartNewSession(dataSet));
                return;
            }
            CheckAndNotifySessionCompleted();
            if (dataSet.SessionInfo.SessionType == SessionType.Na)
            {
                return;
            }
            SessionInfoViewModel.Reset();
            UpdateCurrentSessionOption(dataSet);
            CreateTiming(dataSet);
        }

        private void UpdateCurrentSessionOption(SimulatorDataSet data)
        {
            CurrentSessionOptionsView = GetSessionOptionOfCurrentSession(data);
        }

        private void InitializeGui(SimulatorDataSet data)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => InitializeGui(data));
                return;
            }

            if (data.SessionInfo.SessionType != SessionType.Na)
            {
                TimingDataGridViewModel.MatchDriversList(_sessionTiming.Drivers.Values.ToList());
                _driverLapsWindowManager.RebindAll(TimingDataGridViewModel.DriversViewModels);
            }

            SituationOverviewProvider.ApplyDateSet(data);
        }

        private void OnDisplaySettingsChange(object sender, PropertyChangedEventArgs args)
        {
            ApplyDisplaySettings(DisplaySettingsViewModel);
            switch (args?.PropertyName)
            {
                case nameof(DisplaySettingsViewModel.PaceLaps):
                    PaceLapsChanged();
                    break;
                case nameof(SessionOptionsViewModel.OrderingMode):
                    ChangeOrderingMode();
                    break;
                case nameof(SessionOptionsViewModel.TimesDisplayMode):
                    ChangeTimeDisplayMode();
                    break;
            }
        }

        private void ApplyDisplaySettings(DisplaySettingsViewModel settingsView)
        {
            TrackInfoViewModel.TemperatureUnits = settingsView.TemperatureUnits;
            TrackInfoViewModel.DistanceUnits = settingsView.DistanceUnits;
            SituationOverviewProvider.DisplaySettingsViewModel  = settingsView;
        }

        private void DisplaySettingsChanged()
        {
            DisplaySettingsViewModel newDisplaySettingsViewModel = _displaySettingsViewModel;
            newDisplaySettingsViewModel.PropertyChanged += OnDisplaySettingsChange;
            newDisplaySettingsViewModel.PracticeSessionDisplayOptionsView.PropertyChanged += OnDisplaySettingsChange;
            newDisplaySettingsViewModel.RaceSessionDisplayOptionsView.PropertyChanged += OnDisplaySettingsChange;
            newDisplaySettingsViewModel.QualificationSessionDisplayOptionsView.PropertyChanged += OnDisplaySettingsChange;
        }

        private static async Task SchedulePeriodicAction(Action action, Func<int> delayFunc, TimingDataViewModel sender, bool captureContext)
        {
            while (!sender.TerminatePeriodicTasks)
            {
                await Task.Delay(delayFunc(), CancellationToken.None).ConfigureAwait(captureContext);

                if (!sender.TerminatePeriodicTasks)
                {
                    action();
                }
            }


        }

        private void CheckAndNotifySessionCompleted()
        {
            if (_sessionTiming?.WasGreen != true)
            {
                return;
            }
            SessionCompleted?.Invoke(this, new SessionSummaryEventArgs(_sessionTiming.ToSessionSummary()));
        }
    }
}
