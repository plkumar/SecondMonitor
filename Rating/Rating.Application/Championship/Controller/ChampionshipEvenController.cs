namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Threading.Tasks;
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels.Controllers;

    public class ChampionshipEvenController : AbstractChildController<IChampionshipController>, IChampionshipEvenController
    {
        public override Task StartControllerAsync()
        {
            return Task.CompletedTask;
        }

        public override Task StopControllerAsync()
        {
            return Task.CompletedTask;
        }

        public void StartNextEvent(ChampionshipDto championship)
        {
        }

        public void StopCurrentEvent()
        {
        }
    }
}