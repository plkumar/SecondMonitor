namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Threading.Tasks;
    using Common.DataModel.Championship;
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

        public bool TryResumePreviousChampionship()
        {
            return _runningChampionship != null;
        }
    }
}