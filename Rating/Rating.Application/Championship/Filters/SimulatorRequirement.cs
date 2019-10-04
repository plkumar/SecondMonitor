namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using Common.DataModel.Championship;
    using DataModel.Snapshot;

    public class SimulatorRequirement : IChampionshipConditionEvaluator
    {
        public string GetDescription(ChampionshipDto championshipDto)
        {
            return $"Simulator has to be {championshipDto.SimulatorName}";
        }

        public RequirementResultKind Evaluate(ChampionshipDto championshipDto, SimulatorDataSet dataSet)
        {
            return championshipDto.SimulatorName == dataSet.Source ? RequirementResultKind.PerfectMatch : RequirementResultKind.DoesNotMatch;
        }
    }
}