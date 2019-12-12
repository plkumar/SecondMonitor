namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels.Controllers;

    public interface IChampionshipOverviewController : IChildController<IChampionshipController>
    {
        void OpenChampionshipOverviewWindow();
        void OpenChampionshipDetailsWindow(ChampionshipDto championship);
    }
}