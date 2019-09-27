namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using Common.DataModel.Championship;

    public class SimulatorRequirement : IChampionshipRaceRequirement
    {
        public string GetDescription(ChampionshipDto championshipDto)
        {
            return $"Simulator has to be {championshipDto.SimulatorName}";
        }
    }
}