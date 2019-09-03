namespace SecondMonitor.Rating.Application.Controller.Championship
{
    using SecondMonitor.ViewModels.Controllers;
    using ViewModels.Championship.IconState;

    public interface IChampionshipController : IController
    {
        ChampionshipIconStateViewModel ChampionshipIconStateViewModel { get; }

        void OpenChampionshipWindow();
    }
}