namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship;
    using DataModel.Snapshot;

    public class ChampionshipEligibilityEvaluator : IChampionshipEligibilityEvaluator
    {
        private readonly List<IChampionshipCondition> _conditions;

        public ChampionshipEligibilityEvaluator(IEnumerable<IChampionshipCondition> conditions)
        {
            _conditions = conditions.ToList();
        }

        public RequirementResultKind EvaluateChampionship(ChampionshipDto championship, SimulatorDataSet simulatorData)
        {
            List<RequirementResultKind> evaluationResult = _conditions.Select(x => x.Evaluate(championship, simulatorData)).ToList();
            if (evaluationResult.Any(x => x == RequirementResultKind.DoesNotMatch))
            {
                return RequirementResultKind.DoesNotMatch;
            }

            if (evaluationResult.All(x => x == RequirementResultKind.PerfectMatch))
            {
                return RequirementResultKind.PerfectMatch;
            }

            return RequirementResultKind.CanMatch;
        }
    }
}