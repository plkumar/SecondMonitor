namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using Common.DataModel.Championship;
    using ViewModels.Creation;

    public interface IChampionshipFactory
    {
        ChampionshipDto Create(ChampionshipCreationViewModel championshipCreationViewModel);
    }
}