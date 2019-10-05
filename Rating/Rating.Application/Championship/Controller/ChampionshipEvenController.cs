namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Threading.Tasks;
    using Common.DataModel.Championship;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using Filters;
    using Operations;
    using SecondMonitor.ViewModels.Controllers;
    using SecondMonitor.ViewModels.SessionEvents;

    public class ChampionshipEvenController : AbstractChildController<IChampionshipController>, IChampionshipEvenController
    {
        private readonly IChampionshipManipulator _championshipManipulator;
        private readonly ISessionEventProvider _sessionEventProvider;
        private readonly IChampionshipEligibilityEvaluator _championshipEligibilityEvaluator;
        private ChampionshipDto _runningChampionship;

        public ChampionshipEvenController(IChampionshipManipulator championshipManipulator, ISessionEventProvider sessionEventProvider, IChampionshipEligibilityEvaluator championshipEligibilityEvaluator)
        {
            _championshipManipulator = championshipManipulator;
            _sessionEventProvider = sessionEventProvider;
            _championshipEligibilityEvaluator = championshipEligibilityEvaluator;
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
            if (!IsChampionshipActive)
            {
                return;
            }

            if (_sessionEventProvider.BeforeLastDataSet == null)
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
            }

        }

        private void FinishCurrentEvent(SimulatorDataSet simulatorDataSet)
        {
            IsChampionshipActive = false;
            _runningChampionship = null;
            ParentController.EventFinished(_runningChampionship);
        }
    }
}