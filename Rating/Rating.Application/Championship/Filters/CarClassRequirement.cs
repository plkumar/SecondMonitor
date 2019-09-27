namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using Common.DataModel.Championship;

    public class CarClassRequirement : IChampionshipRaceRequirement
    {
        public string GetDescription(ChampionshipDto championshipDto)
        {
            return championshipDto.ChampionshipState == ChampionshipState.NotStarted ? "Use class you would like to run the whole championship with." : $"Use one of the cars from class: {championshipDto.ClassName} ";
        }
    }
}