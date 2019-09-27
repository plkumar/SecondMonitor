namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.TrackMapping;

    public class TrackRequirement : IChampionshipRaceRequirement
    {
        public string GetDescription(ChampionshipDto championshipDto)
        {
            if (championshipDto.ChampionshipState == ChampionshipState.Finished)
            {
                return string.Empty;
            }

            EventDto eventDto = championshipDto.Events[championshipDto.CurrentEventIndex];
            return eventDto.IsTrackNameExact ? $"Track has to be {eventDto.TrackName}" : $"Track should be {eventDto.TrackName}, but different track can be used.";
        }
    }
}