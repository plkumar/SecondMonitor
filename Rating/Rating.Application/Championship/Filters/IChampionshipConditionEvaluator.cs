namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using Common.DataModel.Championship;
    using DataModel.Snapshot;

    public interface IChampionshipConditionEvaluator
    {
        string GetDescription(ChampionshipDto championshipDto);

        RequirementResultKind Evaluate(ChampionshipDto championshipDto, SimulatorDataSet dataSet);
    }
}