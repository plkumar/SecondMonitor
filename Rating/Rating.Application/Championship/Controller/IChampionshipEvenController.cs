namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels.Controllers;

    public interface IChampionshipEvenController : IChildController<IChampionshipController>
    {
        void StartNextEvent(ChampionshipDto championship);
        void StopCurrentEvent();
    }
}