namespace SecondMonitor.Rating.Common.DataModel.Championship.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    [Serializable]
    public class EventDto
    {
        public EventDto()
        {
            Sessions = new List<SessionDto>();
        }

        [XmlAttribute]
        public string EventName { get; set; }

        [XmlAttribute]
        public string TrackName { get; set; }

        [XmlAttribute]
        public bool IsTrackNameExact { get; set; }

        [XmlIgnore]
        public EventStatus EventStatus
        {
            get
            {
                int sessionsWithResults = Sessions.Count(x => x.SessionResult != null);
                if (sessionsWithResults == 0)
                {
                    return EventStatus.NotStarted;
                }

                if (sessionsWithResults == Sessions.Count)
                {
                    return EventStatus.Finished;
                }

                return EventStatus.InProgress;
            }
        }

        public List<SessionDto> Sessions { get; set; }
    }
}