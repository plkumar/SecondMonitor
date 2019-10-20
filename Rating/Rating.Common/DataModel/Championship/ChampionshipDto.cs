namespace SecondMonitor.Rating.Common.DataModel.Championship
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;
    using Events;

    [Serializable]
    public class ChampionshipDto
    {
        public ChampionshipDto()
        {
            ChampionshipGlobalId = Guid.NewGuid();
            Events = new List<EventDto>();
            Scoring = new List<ScoringDto>();
            Drivers = new List<DriverDto>();
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

        [XmlAttribute]
        public bool AiNamesCanChange { get; set; }

        public List<ScoringDto> Scoring { get; set; }

        public List<DriverDto> Drivers { get; set; }

        public Dictionary<Guid, DriverDto> GetGuidToDriverDictionary() => Drivers.ToDictionary(x => x.GlobalKey, x => x);

        [XmlIgnore]
        public int CompletedRaces => CurrentEventIndex * Events[0].Sessions.Count + CurrentSessionIndex + 1;

        public EventDto GetCurrentEvent()
        {
            return CurrentEventIndex < Events.Count ? Events[CurrentEventIndex] : Events.Last();
        }

        public IEnumerable<SessionDto> GetAllSessions()
        {
            return Events.SelectMany(x => x.Sessions);
        }

        public IEnumerable<SessionResultDto> GetAllResults()
        {
            return Events.SelectMany(x => x.Sessions).Select(x => x.SessionResult).Where(x => x != null);
        }

        public (EventDto eventDto, SessionDto sessionDto) GetLastSessionWithResults()
        {
            var lastEvent = Events.Last(x => x.Sessions.Any(y => y.SessionResult != null));
            return (lastEvent, lastEvent.Sessions.Last(x => x.SessionResult != null));
        }
    }
}