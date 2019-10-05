namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship;
    using DataModel.Snapshot;

    public class OpponentsRequirements : IChampionshipCondition
    {
        public string GetDescription(ChampionshipDto championshipDto)
        {
            if (championshipDto.ChampionshipState == ChampionshipState.NotStarted)
            {
                return championshipDto.AiNamesCanChange ? "Use the opponents count, you would like to run the whole championship with. Their names do not matter." : "Use opponents, you would like to run the championship with. Their names need to match for all races";
            }

            return championshipDto.AiNamesCanChange ? $"Requires {championshipDto.TotalDrivers - 1} opponents. Opponents names can change from previous event." : $"Requires {championshipDto.TotalDrivers - 1} opponents. Opponents names has to be the same, as in previous events. Check championship details if you need the list of all drivers.";
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