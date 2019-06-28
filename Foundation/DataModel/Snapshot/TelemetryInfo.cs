namespace SecondMonitor.DataModel.Snapshot
{
    using System;
    using System.Xml.Serialization;
    using ProtoBuf;

    [ProtoContract]
    [Serializable]
    public sealed class TelemetryInfo
    {
        [XmlAttribute]
        [ProtoMember(1)]
        public bool RequiresDistanceInterpolation { get; set; }

        [XmlAttribute]
        [ProtoMember(2)]
        public bool RequiresPositionInterpolation { get; set; }

        [XmlAttribute]
        [ProtoMember(3)]
        public bool ContainsSuspensionVelocity { get; set; }

        [XmlAttribute]
        [ProtoMember(4)]
        public bool ContainsSuspensionTravel { get; set; }

        [XmlAttribute]
        [ProtoMember(5)]
        public bool ContainsOptimalTemperatures { get; set; }
    }
}