namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.Windows;
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.TrackMapping;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using DataModel.TrackMap;
    using Filters;
    using Operations;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Controllers;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.SessionEvents;
    using SecondMonitor.ViewModels.Settings;
    using SecondMonitor.ViewModels.SplashScreen;
    using SimdataManagement;
    using ViewModels.Events;

    public class ChampionshipEvenController : AbstractChildController<IChampionshipController>, IChampionshipEvenController
    {
        private readonly IChampionshipManipulator _championshipManipulator;
        private readonly ISessionEventProvider _sessionEventProvider;
        private readonly IChampionshipEligibilityEvaluator _championshipEligibilityEvaluator;
        private readonly IWindowService _windowService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ISettingsProvider _settingsProvider;
        private readonly MapsLoader _mapsLoader;
        private ChampionshipDto _runningChampionship;

        public ChampionshipEvenController(IChampionshipManipulator championshipManipulator, ISessionEventProvider sessionEventProvider, IChampionshipEligibilityEvaluator championshipEligibilityEvaluator,
            IWindowService windowService, IMapsLoaderFactory mapsLoaderFactory, IViewModelFactory viewModelFactory, ISettingsProvider settingsProvider)
        {
            _championshipManipulator = championshipManipulator;
            _sessionEventProvider = sessionEventProvider;
            _championshipEligibilityEvaluator = championshipEligibilityEvaluator;
            _windowService = windowService;
            _viewModelFactory = viewModelFactory;
            _settingsProvider = settingsProvider;
            _mapsLoader = mapsLoaderFactory.Create();
        }

        public bool IsChampionshipActive { get; private set; }

        public void StartNextEvent(ChampionshipDto championship)
        {
            _runningChampionship = championship;
            if (_runningChampionship.ChampionshipState == ChampionshipState.NotStarted)
            {
                _championshipManipulator.StartChampionship(championship, _sessionEventProvider.LastDataSet);
            }

            IsChampionshipActive = true;
            ShowWelcomeScreen(_sessionEventProvider.LastDataSet.SessionInfo.TrackInfo);
        }

        public bool TryResumePreviousChampionship(SimulatorDataSet dataSet)
        {
            IsChampionshipActive = dataSet.PlayerInfo.FinishStatus != DriverFinishStatus.Finished && _runningChampionship != null && _championshipEligibilityEvaluator.EvaluateChampionship(_runningChampionship, dataSet) != RequirementResultKind.DoesNotMatch;
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
                FinishCurrentEvent(e.DataSet);
                _runningChampionship = null;
            }

        }

        private void FinishCurrentEvent(SimulatorDataSet simulatorDataSet)
        {
            IsChampionshipActive = false;
            ParentController.EventFinished(_runningChampionship);
        }

        private void ShowWelcomeScreen(TrackInfo trackInfo)
        {
            bool hasMap = _mapsLoader.TryLoadMap(_runningChampionship.SimulatorName, trackInfo.TrackFullName, out TrackMapDto trackMapDto);

            EventDto currentEvent = _runningChampionship.GetCurrentEvent();
            var eventStartingViewModel = _viewModelFactory.Create<EventStartingViewModel>();
            eventStartingViewModel.ChampionshipName = _runningChampionship.ChampionshipName;
            eventStartingViewModel.EventName = currentEvent.EventName;
            eventStartingViewModel.EventIndex = $"({_runningChampionship.CurrentEventIndex + 1} / {_runningChampionship.TotalEvents})";

            SessionDto currentSession = currentEvent.Sessions[_runningChampionship.CurrentSessionIndex];
            eventStartingViewModel.SessionName = currentSession.Name;
            eventStartingViewModel.SessionIndex = $"({_runningChampionship.CurrentSessionIndex + 1} / {currentEvent.Sessions.Count})";

            var trackOverviewViewModel = _viewModelFactory.Create<TrackOverviewViewModel>();
            trackOverviewViewModel.TrackName = trackInfo.TrackFullName;
            trackOverviewViewModel.LayoutLength = $"{trackInfo.LayoutLength.GetByUnit(_settingsProvider.DisplaySettingsViewModel.DistanceUnits):N2} {Distance.GetUnitsSymbol(_settingsProvider.DisplaySettingsViewModel.DistanceUnits)} ";
            if (hasMap)
            {
                trackOverviewViewModel.TrackGeometryViewModel.FromModel(trackMapDto.TrackGeometry);
            }

            eventStartingViewModel.Screens.Add(trackOverviewViewModel);
            eventStartingViewModel.Screens.Add(new SplashScreenViewModel(){PrimaryInformation = "TEXT"});

            _windowService.OpenWindow(eventStartingViewModel, "Event Starting", WindowState.Maximized, SizeToContent.Manual, WindowStartupLocation.CenterOwner);
        }
    }
}