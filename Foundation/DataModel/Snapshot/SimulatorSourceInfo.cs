namespace SecondMonitor.DataModel.Snapshot
{
    using System;
    using System.Xml.Serialization;
    using Drivers;
    using ProtoBuf;

    [ProtoContract]
    [Serializable]
    public sealed class SimulatorSourceInfo
    {
        public SimulatorSourceInfo()
        {
            TelemetryInfo = new TelemetryInfo();
        }

        [XmlAttribute]
        [ProtoMember(2, IsRequired = true)]
        public bool HasLapTimeInformation { get; set; }

        [XmlAttribute]
        [ProtoMember(3, IsRequired = true)]
        public DataInputSupport SectorTimingSupport { get; set; } = DataInputSupport.None;

        [XmlAttribute]
        [ProtoMember(4, IsRequired = true)]
        // Some sims, like rFactor do not show a clear change in laps/lap status when the driver crosses the finish line in his out lap and moves to the hot lap. app needs to use alternative methods to detect this state
        public bool SimNotReportingEndOfOutLapCorrectly { get; set; }

        [XmlAttribute]
        [ProtoMember(5, IsRequired = true)]
        // Some sims, like r3e automatically complete the final lap after the playre crosses the line
        public bool AIInstantFinish { get; set; }

        [XmlAttribute]
        [ProtoMember(6, IsRequired = true)]
        // Some sims, like AC do not report out lap as invalid
        public bool OutLapIsValid { get; set; }

        [XmlAttribute]
        [ProtoMember(7, IsRequired = true)]
        // For some sims (i.e. AMS) it is difficult tu check if lap is valid. One of the methods to look if the sectors were updated. This flag allows this behavior.
        public bool InvalidateLapBySector { get; set; }

        [XmlAttribute]
        [ProtoMember(8, IsRequired = true)]
        // Some sims do not update all relevant information when lap changes. This flag forces a pending state when lap changes, allowing all relevant information to be updated properly.
        public bool ForceLapOverTime { get; set; }

        [XmlAttribute]
        [ProtoMember(9, IsRequired = true)]
        // Indicates if the sim has a global pool of tyre compounds (i.e. assetto corsa), or if each car has its own. The latter means that tyre compound for two cars can have different properties, even if the name is the same
        public bool GlobalTyreCompounds { get; set; }

        [XmlAttribute]
        [ProtoMember(10, IsRequired = true)]
        //Indicates that the world positions (x,y,z) provided by the sim are not valid.
        public bool WorldPositionInvalid { get; set; }

        [XmlAttribute]
        [ProtoMember(11, IsRequired = true)]
        //Indicates if the time is interpolated - i.e RF2 only refresh time every 200ms, between those 200ms the connector will interpolate the time
        public bool TimeInterpolated { get; set; }

        [XmlAttribute]
        [ProtoMember(12, IsRequired = true)]
        //Indicates that there is possibility of prolonged N/A Session Type between two valid session. I.e. in AMS when the player is in the pre-race screen, the game doesn't return a valid session type (i.e. race).
        //This affects i.e. rating,
        public bool NAStateBetweenSessions { get; set; }

        [XmlAttribute]
        [ProtoMember(13, IsRequired = true)]
        public GapInformationKind GapInformationProvided { get; set; }

        [ProtoMember(14, IsRequired = true)]
        [XmlAttribute]
        public bool HasRewindFunctionality { get; set; }

        //In some sims it might be required to the best lap times to be continuously checked and updated if needed. I.e. in F1 2019 this is needed because accelerated time is not exposed to the shared data.
        [ProtoMember(15, IsRequired = true)]
        [XmlAttribute]
        public bool OverrideBestLap { get; set; }

        [ProtoMember(1)]
        public TelemetryInfo TelemetryInfo { get; set; }

    }
}