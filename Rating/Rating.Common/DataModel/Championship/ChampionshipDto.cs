namespace SecondMonitor.Rating.Common.DataModel.Championship
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using TrackMapping;

    [Serializable]
    public class ChampionshipDto
    {
        public ChampionshipDto()
        {
            ChampionshipGlobalId = Guid.NewGuid();
            Events = new List<EventDto>();
        }

        public List<EventDto> Events { get; set; }

        [XmlAttribute]
        public string SimulatorName { get; set; }

        [XmlAttribute]
        public Guid ChampionshipGlobalId { get; set; }

        [XmlAttribute]
        public string ChampionshipName { get; set; }

        [XmlAttribute]
        public string NextTrack { get; set; }

        [XmlAttribute]
        public ChampionshipState ChampionshipState { get; set; }

        [XmlAttribute]
        public int TotalEvents { get; set; }

        [XmlAttribute]
        public int CurrentEventIndex { get; set; }

        [XmlAttribute]
        public int CurrentSessionIndex { get; set; }

        [XmlAttribute]
        public int Position { get; set; }

        [XmlAttribute]
        public int TotalDrivers { get; set; }

        [XmlAttribute]
        public string ClassName { get; set; }
    }
}