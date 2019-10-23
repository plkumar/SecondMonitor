namespace SecondMonitor.Rating.Application.Championship
{
    using System.Linq;
    using System.Windows;
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.Events;
    using Contracts.Commands;
    using Contracts.TrackRecords;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.TrackMap;
    using DataModel.TrackRecords;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.Settings;
    using SimdataManagement;
    using ViewModels.Events;

    public class ChampionshipDialogProvider : IChampionshipDialogProvider
    {
        private readonly IWindowService _windowService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ITrackRecordsProvider _trackRecordsProvider;
        private readonly ISettingsProvider _settingsProvider;
        private readonly MapsLoader _mapsLoader;

        public ChampionshipDialogProvider(IWindowService windowService, IViewModelFactory viewModelFactory, ITrackRecordsProvider trackRecordsProvider, IMapsLoaderFactory mapsLoaderFactory, ISettingsProvider settingsProvider)
        {
            _windowService = windowService;
            _viewModelFactory = viewModelFactory;
            _trackRecordsProvider = trackRecordsProvider;
            _settingsProvider = settingsProvider;
            _mapsLoader = mapsLoaderFactory.Create();
        }

        public void ShowWelcomeScreen(SimulatorDataSet dataSet, ChampionshipDto championship)
        {
            EventDto currentEvent = championship.GetCurrentEvent();
            var eventStartingViewModel = _viewModelFactory.Create<EventStartingViewModel>();
            eventStartingViewModel.ChampionshipName = championship.ChampionshipName;
            eventStartingViewModel.EventName = currentEvent.EventName;
            eventStartingViewModel.EventIndex = $"({championship.CurrentEventIndex + 1} / {championship.Events.Count})";

            SessionDto currentSession = currentEvent.Sessions[championship.CurrentSessionIndex];
            eventStartingViewModel.SessionName = currentSession.Name;
            eventStartingViewModel.SessionIndex = $"({championship.CurrentSessionIndex + 1} / {currentEvent.Sessions.Count})";

            eventStartingViewModel.Screens.Add(CreateTrackOverviewViewModel(dataSet, championship));
            eventStartingViewModel.Screens.Add(CreateStandingOverviewViewModel(championship));

            Window window = _windowService.OpenWindow(eventStartingViewModel, "Event Starting", WindowState.Maximized, SizeToContent.Manual, WindowStartupLocation.CenterOwner);
            eventStartingViewModel.CloseCommand = new RelayCommand(() => CloseWindow(window));
        }

        public void ShowLastEvenResultWindow(ChampionshipDto championship)
        {
            var sessionCompletedViewmodel = _viewModelFactory.Create<SessionCompletedViewModel>();
            (EventDto eventDto, SessionDto sessionDto) = championship.GetLastSessionWithResults();
            var lastResult = sessionDto.SessionResult;
            if (lastResult == null)
            {
                return;
            }

            sessionCompletedViewmodel.Title = "Session Completed";

            var podiumViewModel = _viewModelFactory.Create<PodiumViewModel>();
            podiumViewModel.FromModel(lastResult);

            var driversFinishViewModel = _viewModelFactory.Create<DriversFinishViewModel>();
            driversFinishViewModel.Header = "Session Results";
            driversFinishViewModel.FromModel(lastResult);

            var driversNewStandingsViewModel = _viewModelFactory.Create<DriversNewStandingsViewModel>();

            driversNewStandingsViewModel.ChampionshipName = championship.ChampionshipName;
            driversNewStandingsViewModel.EventName = eventDto.EventName;
            driversNewStandingsViewModel.EventIndex = $"({championship.Events.IndexOf(eventDto) + 1} / {championship.Events.Count})";
            driversNewStandingsViewModel.SessionName = sessionDto.Name;
            driversNewStandingsViewModel.SessionIndex = $"({eventDto.Sessions.IndexOf(sessionDto) + 1} / {eventDto.Sessions.Count})";

            driversNewStandingsViewModel.FromModel(lastResult);

            sessionCompletedViewmodel.Screens.Add(podiumViewModel);
            sessionCompletedViewmodel.Screens.Add(driversFinishViewModel);
            sessionCompletedViewmodel.Screens.Add(driversNewStandingsViewModel);

            if (championship.ChampionshipState == ChampionshipState.Finished)
            {
                var championshipFinalStandings = _viewModelFactory.Create<StandingOverviewViewModel>();
                championshipFinalStandings.Header = "Final Standing: ";
                championshipFinalStandings.FromModel(championship.Drivers.OrderBy(x => x.Position));
                sessionCompletedViewmodel.Screens.Add(championshipFinalStandings);
            }

            Window window = _windowService.OpenWindow(sessionCompletedViewmodel, "Session Completed", WindowState.Maximized, SizeToContent.Manual, WindowStartupLocation.CenterOwner);
            sessionCompletedViewmodel.CloseCommand = new RelayCommand(() => CloseWindow(window));
        }

        private TrackOverviewViewModel CreateTrackOverviewViewModel(SimulatorDataSet dataSet, ChampionshipDto championship)
        {
            TrackInfo trackInfo = dataSet.SessionInfo.TrackInfo;
            bool hasMap = _mapsLoader.TryLoadMap(championship.SimulatorName, trackInfo.TrackFullName, out TrackMapDto trackMapDto);
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

        private StandingOverviewViewModel CreateStandingOverviewViewModel(ChampionshipDto championship)
        {
            var standingOverviewViewModel = _viewModelFactory.Create<StandingOverviewViewModel>();
            standingOverviewViewModel.FromModel(championship.Drivers);
            return standingOverviewViewModel;
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}