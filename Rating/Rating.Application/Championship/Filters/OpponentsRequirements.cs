namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship;
    using DataModel.Snapshot;

    public class OpponentsRequirements : IChampionshipConditionEvaluator
    {
        public string GetDescription(ChampionshipDto championshipDto)
        {
            if (championshipDto.ChampionshipState == ChampionshipState.NotStarted)
            {
                return championshipDto.AiNamesCanChange ? "Use the opponents count, you would like to run the whole championship with. Their names do not matter." : "Use opponents, you would like to run the championship with. Their names need to match for all races";
            }

            return string.Empty;
        }

        public RequirementResultKind Evaluate(ChampionshipDto championshipDto, SimulatorDataSet dataSet)
        {
            if (championshipDto.ChampionshipState == ChampionshipState.NotStarted)
            {
                return RequirementResultKind.CanMatch;
            }

            List<string> filteredDrivers = dataSet.DriversInfo.Where(x => x.CarClassId == dataSet.PlayerInfo.CarClassId).Select(x => x.DriverName).ToList();

            if (championshipDto.AiNamesCanChange)
            {
                return championshipDto.Drivers.Count == filteredDrivers.Count ? RequirementResultKind.PerfectMatch : RequirementResultKind.DoesNotMatch;
            }

            return filteredDrivers.All(x => championshipDto.Drivers.Any(y => y.LastUsedName == x)) ? RequirementResultKind.PerfectMatch : RequirementResultKind.DoesNotMatch;


        }
    }
}