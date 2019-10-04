namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.TrackMapping;
    using DataModel.Snapshot;

    public class TrackRequirement : IChampionshipConditionEvaluator
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

        public RequirementResultKind Evaluate(ChampionshipDto championshipDto, SimulatorDataSet dataSet)
        {
            var currentEvent = championshipDto.Events[championshipDto.CurrentEventIndex];
            if (currentEvent.IsTrackNameExact)
            {
                return currentEvent.TrackName == dataSet.SessionInfo.TrackInfo.TrackFullName ? RequirementResultKind.PerfectMatch : RequirementResultKind.DoesNotMatch;
            }

            return RequirementResultKind.CanMatch;
        }
    }
}