namespace SecondMonitor.Rating.Common.DataModel.Championship
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class ChampionshipDto
    {
        public ChampionshipDto()
        {
            ChampionshipGlobalId = Guid.NewGuid();
        }

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