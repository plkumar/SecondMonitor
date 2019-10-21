namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Threading.Tasks;
    using Common.DataModel.Championship;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using Filters;
    using NLog;
    using Operations;
    using SecondMonitor.ViewModels.Controllers;
    using SecondMonitor.ViewModels.SessionEvents;

    public class ChampionshipEventController : AbstractChildController<IChampionshipController>, IChampionshipEventController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IChampionshipManipulator _championshipManipulator;
        private readonly ISessionEventProvider _sessionEventProvider;
        private readonly IChampionshipEligibilityEvaluator _championshipEligibilityEvaluator;
        private readonly IChampionshipDialogProvider _championshipDialogProvider;
        private string _lastTrack;
        private ChampionshipDto _runningChampionship;

        public ChampionshipEventController(IChampionshipManipulator championshipManipulator, ISessionEventProvider sessionEventProvider, IChampionshipEligibilityEvaluator championshipEligibilityEvaluator, IChampionshipDialogProvider championshipDialogProvider)
        {
            _championshipManipulator = championshipManipulator;
            _sessionEventProvider = sessionEventProvider;
            _championshipEligibilityEvaluator = championshipEligibilityEvaluator;
            _championshipDialogProvider = championshipDialogProvider;

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
                _championshipManipulator.AddResultsForCurrentSession(_runningChampionship, _sessionEventProvider.BeforeLastDataSet, shiftPlayerToLastPlace: true);
                _runningChampionship.ChampionshipState = ChampionshipState.LastSessionCanceled;
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
                _championshipManipulator.CommitLastSessionResults(_runningChampionship);
                _championshipDialogProvider.ShowLastEvenResultWindow(_runningChampionship);
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
            _championshipDialogProvider.ShowWelcomeScreen(dataSet, _runningChampionship);
        }
    }
}