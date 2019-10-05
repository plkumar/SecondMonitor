namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using Common.DataModel.Championship;
    using DataModel.Snapshot;

    public interface IChampionshipEligibilityEvaluator
    {
        RequirementResultKind EvaluateChampionship(ChampionshipDto championship, SimulatorDataSet simulatorData);
    }
}