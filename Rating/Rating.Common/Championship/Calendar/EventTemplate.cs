namespace SecondMonitor.Rating.Common.Championship.Calendar
{
    using DataModel.Championship;

    public class EventTemplate
    {

        public EventTemplate(TrackTemplate trackTemplate) : this(trackTemplate, string.Empty)
        {

        }

        public EventTemplate(TrackTemplate trackTemplate, string eventName)
        {
            TrackTemplate = trackTemplate;
            EventName = eventName;
        }

        public TrackTemplate TrackTemplate { get; }

        public string EventName { get; }

        public bool HasEventName => !string.IsNullOrWhiteSpace(EventName);
    }
}