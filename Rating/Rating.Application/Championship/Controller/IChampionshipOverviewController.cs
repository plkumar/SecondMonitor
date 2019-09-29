namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using SecondMonitor.ViewModels.Controllers;

    public interface IChampionshipOverviewController : IChildController<IChampionshipController>
    {
        void OpenChampionshipOverviewWindow();
    }
}