namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using Common.DataModel.Championship;

    public interface IChampionshipRaceRequirement
    {
        string GetDescription(ChampionshipDto championshipDto);
    }
}