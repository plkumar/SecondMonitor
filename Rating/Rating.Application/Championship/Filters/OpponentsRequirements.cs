namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using Common.DataModel.Championship;

    public class OpponentsRequirements : IChampionshipRaceRequirement
    {
        public string GetDescription(ChampionshipDto championshipDto)
        {
            if (championshipDto.ChampionshipState == ChampionshipState.NotStarted)
            {
                return championshipDto.AiNamesCanChange ? "Use the opponents count, you would like to run the whole championship with. Their names do not matter." : "Use opponents, you would like to run the championship with. Their names need to match for all races";
            }

            return string.Empty;
        }
    }
}