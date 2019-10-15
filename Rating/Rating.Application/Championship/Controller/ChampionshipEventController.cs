namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Threading.Tasks;
    using System.Windows;
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.Events;
    using Common.DataModel.Championship.TrackMapping;
    using Contracts.Commands;
    using Contracts.TrackRecords;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using DataModel.TrackMap;
    using DataModel.TrackRecords;
    using Filters;
    using NLog;
    using Operations;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Controllers;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.SessionEvents;
    using SecondMonitor.ViewModels.Settings;
    using SimdataManagement;
    using ViewModels.Events;

    public class ChampionshipEventController : AbstractChildController<IChampionshipController>, IChampionshipEventController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IChampionshipManipulator _championshipManipulator;
        private readonly ISessionEventProvider _sessionEventProvider;
        private readonly IChampionshipEligibilityEvaluator _championshipEligibilityEvaluator;
        private readonly IWindowService _windowService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ITrackRecordsProvider _trackRecordsProvider;
        private readonly MapsLoader _mapsLoader;
        private string _lastTrack;
        private ChampionshipDto _runningChampionship;

        public ChampionshipEventController(IChampionshipManipulator championshipManipulator, ISessionEventProvider sessionEventProvider, IChampionshipEligibilityEvaluator championshipEligibilityEvaluator,
            IWindowService windowService, IMapsLoaderFactory mapsLoaderFactory, IViewModelFactory viewModelFactory, ISettingsProvider settingsProvider, ITrackRecordsProvider trackRecordsProvider)
        {
            _championshipManipulator = championshipManipulator;
            _sessionEventProvider = sessionEventProvider;
            _championshipEligibilityEvaluator = championshipEligibilityEvaluator;
            _windowService = windowService;
            _viewModelFactory = viewModelFactory;
            _settingsProvider = settingsProvider;
            _trackRecordsProvider = trackRecordsProvider;
            _mapsLoader = mapsLoaderFactory.Create();
        }

        public bool IsChampionshipActive { get; private set; }

        public ChampionshipDto CurrentChampionship => _runningChampionship;

        public void StartNextEvent(ChampionshipDto championship)
        {
            Logger.Info($"Starting new Event {championship.ChampionshipName}");
            _runningChampionship = championship;
            if (_runningChampionship.ChampionshipState == ChampionshipState.NotStarted)
            {
                _championshipManipulator.StartChampionship(championship, _sessionEventProvider.LastDataSet);

            }
            else
            {
                _championshipManipulator.UpdateAiDriversNames(championship, _sessionEventProvider.LastDataSet);
            }

            _championshipManipulator.StartNextEvent(championship, _sessionEventProvider.LastDataSet);

            IsChampionshipActive = true;
            ShowWelcomeScreen(_sessionEventProvider.LastDataSet);
        }

        public bool TryResumePreviousChampionship(SimulatorDataSet dataSet)
        {
            IsChampionshipActive = dataSet.PlayerInfo.FinishStatus != DriverFinishStatus.Finished && _runningChampionship != null && _championshipEligibilityEvaluator.EvaluateChampionship(_runningChampionship, dataSet) != RequirementResultKind.DoesNotMatch;
            Logger.Info($"TryResumePreviousChampionship result is {IsChampionshipActive}");
            if (IsChampionshipActive && dataSet.SessionInfo.TrackInfo.TrackFullName != _lastTrack)
            {
                ShowWelcomeScreen(dataSet);
            }
            return IsChampionshipActive;
        }

        public override Task StartControllerAsync()
        {
            _sessionEventProvider.PlayerFinishStateChanged += SessionEventProviderOnPlayerFinishStateChanged;
            _sessionEventProvider.SessionTypeChange += SessionEventProviderOnSessionTypeChange;
            return Task.CompletedTask;
        }

        public override Task StopControllerAsync()
        {
            _sessionEventProvider.PlayerFinishStateChanged -= SessionEventProviderOnPlayerFinishStateChanged;
            _sessionEventProvider.SessionTypeChange -= SessionEventProviderOnSessionTypeChange;
            return Task.CompletedTask;
        }

        private void SessionEventProviderOnSessionTypeChange(object sender, DataSetArgs e)
        {
            if (!IsChampionshipActive || _sessionEventProvider.BeforeLastDataSet == null)
            {
                return;
            }

            if (_sessionEventProvider.BeforeLastDataSet.SessionInfo.SessionType == SessionType.Race)
            {
                _championshipManipulator.AddResultsForCurrentSession(_runningChampionship, _sessionEventProvider.BeforeLastDataSet);
                FinishCurrentEvent(_sessionEventProvider.BeforeLastDataSet);
                return;
            }

            if (e.DataSet.SessionInfo.SessionType != SessionType.Na && _championshipEligibilityEvaluator.EvaluateChampionship(_runningChampionship, e.DataSet) == RequirementResultKind.DoesNotMatch)
            {
                FinishCurrentEvent(_sessionEventProvider.BeforeLastDataSet);
            }
        }

        private void SessionEventProviderOnPlayerFinishStateChanged(object sender, DataSetArgs e)
        {
            if (!IsChampionshipActive)
            {
                return;
            }

            if (e.DataSet.PlayerInfo.FinishStatus == DriverFinishStatus.Finished && e.DataSet.SessionInfo.SessionType == SessionType.Race)
            {
                _championshipManipulator.AddResultsForCurrentSession(_runningChampionship, e.DataSet);
                FinishCurrentEvent(e.DataSet);
                _runningChampionship = null;
            }

        }

        private void FinishCurrentEvent(SimulatorDataSet simulatorDataSet)
        {
            Logger.Info($"Finishing event for {_runningChampionship.ChampionshipName}");
            IsChampionshipActive = false;
            ParentController.EventFinished(_runningChampionship);
        }

        private void ShowWelcomeScreen(SimulatorDataSet dataSet)
        {
            _lastTrack = dataSet.SessionInfo.TrackInfo.TrackFullName;
            EventDto currentEvent = _runningChampionship.GetCurrentEvent();
            var eventStartingViewModel = _viewModelFactory.Create<EventStartingViewModel>();
            eventStartingViewModel.ChampionshipName = _runningChampionship.ChampionshipName;
            eventStartingViewModel.EventName = currentEvent.EventName;
            eventStartingViewModel.EventIndex = $"({_runningChampionship.CurrentEventIndex + 1} / {_runningChampionship.TotalEvents})";

            SessionDto currentSession = currentEvent.Sessions[_runningChampionship.CurrentSessionIndex];
            eventStartingViewModel.SessionName = currentSession.Name;
            eventStartingViewModel.SessionIndex = $"({_runningChampionship.CurrentSessionIndex + 1} / {currentEvent.Sessions.Count})";

            eventStartingViewModel.Screens.Add(CreateTrackOverviewViewModel(dataSet));
            eventStartingViewModel.Screens.Add(CreateStandingOverviewViewModel());

            Window window = _windowService.OpenWindow(eventStartingViewModel, "Event Starting", WindowState.Maximized, SizeToContent.Manual, WindowStartupLocation.CenterOwner);
            eventStartingViewModel.CloseCommand = new RelayCommand(() => CloseEventStartingView(window));
        }

        private TrackOverviewViewModel CreateTrackOverviewViewModel(SimulatorDataSet dataSet)
        {
            TrackInfo trackInfo = dataSet.SessionInfo.TrackInfo;
            bool hasMap = _mapsLoader.TryLoadMap(_runningChampionship.SimulatorName, trackInfo.TrackFullName, out TrackMapDto trackMapDto);
            var trackOverviewViewModel = _viewModelFactory.Create<TrackOverviewViewModel>();
            trackOverviewViewModel.TrackName = trackInfo.TrackFullName;
            trackOverviewViewModel.LayoutLength = $"{trackInfo.LayoutLength.GetByUnit(_settingsProvider.DisplaySettingsViewModel.DistanceUnits):N2} {Distance.GetUnitsSymbol(_settingsProvider.DisplaySettingsViewModel.DistanceUnits)} ";
            if (hasMap)
            {
                trackOverviewViewModel.TrackGeometryViewModel.FromModel(trackMapDto.TrackGeometry);
            }

            if (_trackRecordsProvider.TryGetOverallBestRecord(dataSet.Source, trackInfo.TrackFullName, SessionType.Race, out RecordEntryDto recordEntry))
            {
                trackOverviewViewModel.OverallRecord.FromModel(recordEntry);
            }

            if (_trackRecordsProvider.TryGetCarBestRecord(dataSet.Source, trackInfo.TrackFullName, dataSet.PlayerInfo.CarName, SessionType.Race, out recordEntry))
            {
                trackOverviewViewModel.CarRecord.FromModel(recordEntry);
            }

            if (_trackRecordsProvider.TryGetClassBestRecord(dataSet.Source, trackInfo.TrackFullName, dataSet.PlayerInfo.CarClassName, SessionType.Race, out recordEntry))
            {
                trackOverviewViewModel.ClassRecord.FromModel(recordEntry);
            }

            return trackOverviewViewModel;
        }

        private StandingOverviewViewModel CreateStandingOverviewViewModel()
        {
            var standingOverviewViewModel = _viewModelFactory.Create<StandingOverviewViewModel>();
            standingOverviewViewModel.FromModel(_runningChampionship.Drivers);
            return standingOverviewViewModel;
        }

        private void CloseEventStartingView(Window window)
        {
            window.Close();
        }
    }
}