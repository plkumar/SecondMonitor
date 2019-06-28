using System;

namespace SecondMonitor.DataModel.BasicProperties
{
    using System.Xml.Serialization;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public sealed class TyreWear
    {

        [XmlAttribute]
        [ProtoMember(1, IsRequired = true)]
        public double ActualWear { get; set; }

        [XmlAttribute]
        [ProtoMember(2, IsRequired = true)]
        public double NoWearWearLimit { get; set; }

        [XmlAttribute]
        [ProtoMember(3, IsRequired = true)]
        public double LightWearLimit { get; set; }

        [XmlAttribute]
        [ProtoMember(4, IsRequired = true)]
        public double HeavyWearLimit { get; set; }
    }
}