namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using SecondMonitor.ViewModels.Controllers;
    using ViewModels.IconState;

    public interface IChampionshipController : IController
    {
        ChampionshipIconStateViewModel ChampionshipIconStateViewModel { get; }

        void OpenChampionshipWindow();
    }
}