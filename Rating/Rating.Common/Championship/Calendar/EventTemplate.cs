namespace SecondMonitor.Rating.Common.Championship.Calendar
{
    using DataModel.Championship;

    public class EventTemplate
    {
        public EventTemplate(TrackTemplate trackTemplate)
        {
            TrackTemplate = trackTemplate;
        }

        public TrackTemplate TrackTemplate { get; }
    }
}