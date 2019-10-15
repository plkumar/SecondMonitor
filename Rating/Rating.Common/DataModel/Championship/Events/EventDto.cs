namespace SecondMonitor.Rating.Common.DataModel.Championship.Events
{
    using System;
    using System.Collections.Generic;
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

        public List<SessionDto> Sessions { get; set; }
    }
}