namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using Common.DataModel.Championship;
    using DataModel.Snapshot;

    public interface IChampionshipCondition
    {
        string GetDescription(ChampionshipDto championshipDto);

        RequirementResultKind Evaluate(ChampionshipDto championshipDto, SimulatorDataSet dataSet);
    }
}