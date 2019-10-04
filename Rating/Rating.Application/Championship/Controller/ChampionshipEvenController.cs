namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Threading.Tasks;
    using Common.DataModel.Championship;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using SecondMonitor.ViewModels.Controllers;

    public class ChampionshipEvenController : AbstractChildController<IChampionshipController>, IChampionshipEvenController
    {
        private ChampionshipDto _runningChampionship;

        public override Task StartControllerAsync()
        {
            return Task.CompletedTask;
        }

        public override Task StopControllerAsync()
        {
            return Task.CompletedTask;
        }

        public bool IsChampionshipActive => _runningChampionship != null;

        public void StartNextEvent(ChampionshipDto championship)
        {
            _runningChampionship = championship;
        }

        public bool TryResumePreviousChampionship(SimulatorDataSet dataSet)
        {
            if (dataSet.PlayerInfo.FinishStatus == DriverFinishStatus.Finished)
            {
                return false;
            }

            return _runningChampionship != null;
        }
    }
}