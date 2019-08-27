namespace SecondMonitor.Rating.Application.Controller
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Common.DataModel;
    using Common.Repository;
    using Contracts.Commands;
    using DataModel.Snapshot;
    using DataModel.Summary;
    using NLog;
    using RaceObserver;
    using RatingProvider;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.Settings;
    using SecondMonitor.ViewModels.Settings.ViewModel;
    using ViewModels;
    using ViewModels.RatingHistory;
    using ViewModels.RatingOverview;

    public class RatingApplicationController : IRatingApplicationController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Stopwatch _refreshStopwatch;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IRaceObserverController _raceObserverController;
        private readonly IWindowService _windowService;
        private readonly IRatingRepository _ratingRepository;
        private bool _inMp;
        private readonly DisplaySettingsViewModel _displaySettingsViewModel;
        private Ratings _currentRatings;

        public RatingApplicationController(IViewModelFactory viewModelFactory, IRaceObserverController raceObserverController, ISettingsProvider settingsProvider, IWindowService windowService, IRatingRepository ratingRepository)
        {
            _refreshStopwatch = Stopwatch.StartNew();
            _viewModelFactory = viewModelFactory;
            _raceObserverController = raceObserverController;
            _windowService = windowService;
            _ratingRepository = ratingRepository;
            _displaySettingsViewModel = settingsProvider.DisplaySettingsViewModel;
        }

        public IRatingApplicationViewModel RatingApplicationViewModel { get; set; }
        public IRatingProvider RatingProvider => _raceObserverController;

        public async Task StartControllerAsync()
        {
            RatingApplicationViewModel = _viewModelFactory.Create<IRatingApplicationViewModel>();
            RatingApplicationViewModel.IsVisible = _displaySettingsViewModel.RatingSettingsViewModel.IsEnabled;
            _raceObserverController.RatingApplicationViewModel = RatingApplicationViewModel;
            _displaySettingsViewModel.RatingSettingsViewModel.PropertyChanged+= RatingSettingsViewModelOnPropertyChanged;
            BindCommands();
            await _raceObserverController.StartControllerAsync();
        }

        private void RefreshRatings()
        {
            _currentRatings = _ratingRepository.LoadRatingsOrCreateNew();
        }

        private void BindCommands()
        {
            RatingApplicationViewModel.PropertyChanged += RatingApplicationViewModelOnPropertyChanged;
            RatingApplicationViewModel.ShowAllHistoryCommand = new RelayCommand(ShowAllHistory);
            RatingApplicationViewModel.ShowAllRatings = new RelayCommand(ShowAllRatings);
            RatingApplicationViewModel.ShowClassHistoryCommand = new RelayCommand(ShowCurrentClassHistory);
        }

        private void ShowCurrentClassHistory()
        {
            string selectedClass = RatingApplicationViewModel.SelectedClass;
            string currentSimulator = _raceObserverController.CurrentSimulator;
            if (string.IsNullOrEmpty(selectedClass) || string.IsNullOrEmpty(currentSimulator))
            {
                return;
            }

            RefreshRatings();
            Common.DataModel.SimulatorRating simulatorRating = _currentRatings.SimulatorsRatings.FirstOrDefault(x => x.SimulatorName == currentSimulator);
            if (simulatorRating == null)
            {
                return;
            }

            ShowRaceResults( selectedClass, simulatorRating.Results.Where(x => x.ClassName == selectedClass));
        }

        private void ShowAllRatings()
        {
            RefreshRatings();
            IRatingOverviewWindowViewModel ratingOverviewWindowViewModel = _viewModelFactory.Create<IRatingOverviewWindowViewModel>();
            ratingOverviewWindowViewModel.FromModel(_currentRatings);
            ratingOverviewWindowViewModel.OpenClassHistoryCommand = new RelayCommandWithParameter<(ISimulatorRatingsViewModel simulatorRating, IClassRatingViewModel classRating)>(x => ShowClassRatings(x.simulatorRating, x.classRating));
            _windowService.OpenWindow(ratingOverviewWindowViewModel, "Ratings", WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner);
        }

        private void ShowAllHistory()
        {
            RefreshRatings();
            IHistoryWindowViewModel historyWindowViewModel = _viewModelFactory.Create<IHistoryWindowViewModel>();
            historyWindowViewModel.FromModel(_currentRatings);
            _windowService.OpenWindow(historyWindowViewModel, "Races History", WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner);
        }

        private void ShowClassRatings(ISimulatorRatingsViewModel simulatorRatingsViewModel, IClassRatingViewModel classRatingViewModel)
        {
            IEnumerable <RaceResult> results = _currentRatings.SimulatorsRatings.First(x => x.SimulatorName == simulatorRatingsViewModel.SimulatorName).Results.Where(x => x.ClassName == classRatingViewModel.ClassName);
            ShowRaceResults(classRatingViewModel.ClassName, results);
        }

        private void ShowRaceResults(string className, IEnumerable<RaceResult> results)
        {
            IRaceHistoriesViewModel viewModel = _viewModelFactory.Create<IRaceHistoriesViewModel>();
            viewModel.FromModel(results);
            viewModel.Title = className;
            _windowService.OpenWindow(viewModel, $"{viewModel.Title} History", WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner);
        }

        private void RatingApplicationViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RatingApplicationViewModel.IsRateRaceCheckboxChecked))
            {
                _raceObserverController.Reset();
            }
        }

        private void RatingSettingsViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RatingSettingsViewModel.IsEnabled))
            {
                RatingApplicationViewModel.IsVisible = _displaySettingsViewModel.RatingSettingsViewModel.IsEnabled;
                _raceObserverController.Reset();
            }
        }

        public async Task StopControllerAsync()
        {
            RatingApplicationViewModel.PropertyChanged -= RatingApplicationViewModelOnPropertyChanged;
            _displaySettingsViewModel.RatingSettingsViewModel.PropertyChanged -= RatingSettingsViewModelOnPropertyChanged;
            await _raceObserverController.StopControllerAsync();
        }

        public async Task NotifySessionCompletion(SessionSummary sessionSummary)
        {
            if (_inMp || !_displaySettingsViewModel.RatingSettingsViewModel.IsEnabled || !RatingApplicationViewModel.IsRateRaceCheckboxChecked)
            {
                return;
            }
            try
            {
                await _raceObserverController.NotifySessionCompletion(sessionSummary);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public async Task NotifyDataLoaded(SimulatorDataSet simulatorDataSet)
        {
            if (_refreshStopwatch.ElapsedMilliseconds < 1000)
            {
                return;
            }
            try
            {
                _refreshStopwatch.Restart();
                CheckMp(simulatorDataSet.SessionInfo);
                if (_inMp || !RatingApplicationViewModel.IsEnabled || !_displaySettingsViewModel.RatingSettingsViewModel.IsEnabled || !RatingApplicationViewModel.IsRateRaceCheckboxChecked)
                {
                    return;
                }
                await _raceObserverController.NotifyDataLoaded(simulatorDataSet);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void CheckMp(SessionInfo sessionInfo)
        {
            if (_inMp == sessionInfo.IsMultiplayer)
            {
                return;
            }

            if (sessionInfo.IsMultiplayer)
            {
                _inMp = true;
                RatingApplicationViewModel.CollapsedMessage = "MP Detected, Rating Disabled";
                RatingApplicationViewModel.IsEnabled = false;
            }
            else
            {
                _inMp = false;
                RatingApplicationViewModel.CollapsedMessage = string.Empty;
                RatingApplicationViewModel.IsEnabled = true;
            }

        }
    }
}