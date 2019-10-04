namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels.Controllers;
    using ViewModels.IconState;

    public interface IChampionshipController : IController
    {
        ChampionshipIconStateViewModel ChampionshipIconStateViewModel { get; }

        void OpenChampionshipWindow();

        void StartNextEvent(ChampionshipDto championship);
    }
}