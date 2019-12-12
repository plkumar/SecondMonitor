namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using Common.DataModel.Championship;
    using DataModel.Snapshot;

    public class CarClassRequirement : IChampionshipCondition
    {
        public string GetDescription(ChampionshipDto championshipDto)
        {
            return championshipDto.ChampionshipState == ChampionshipState.NotStarted ? "Use class you would like to run the whole championship with." : $"Use one of the cars from class: {championshipDto.ClassName} ";
        }

        public RequirementResultKind Evaluate(ChampionshipDto championshipDto, SimulatorDataSet dataSet)
        {
            if (championshipDto.ChampionshipState == ChampionshipState.NotStarted)
            {
                return RequirementResultKind.CanMatch;
            }

            return dataSet.PlayerInfo.CarClassName == championshipDto.ClassName ? RequirementResultKind.PerfectMatch : RequirementResultKind.DoesNotMatch;
        }
    }
}