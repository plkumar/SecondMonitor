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

        [XmlAttribute]
        public EventStatus EventStatus { get; set; }

        public List<SessionDto> Sessions { get; set; }
    }
}