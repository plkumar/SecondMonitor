namespace SecondMonitor.DataModel.BasicProperties
{
    using System;
    using System.Xml.Serialization;

    using Newtonsoft.Json;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public sealed class Acceleration
    {

        private static readonly double GConst = 9.8;

        public Acceleration()
        {

        }

        [JsonIgnore]
        [XmlIgnore]
        public double XinG
        {
            get => XinMs / GConst;
            set => XinMs = value * GConst;
        }

        [JsonIgnore]
        [XmlIgnore]
        public double YinG
        {
            get => YinMs / GConst;
            set => YinMs = value * GConst;
        }

        [JsonIgnore]
        [XmlIgnore]
        public double ZinG
        {
            get => ZinMs / GConst;
            set => ZinMs = value * GConst;
        }

        [XmlAttribute]
        [ProtoMember(1, IsRequired = true)]
        public double XinMs { get; set; }

        [XmlAttribute]
        [ProtoMember(2, IsRequired = true)]
        public double YinMs { get; set; }

        [XmlAttribute]
        [ProtoMember(3, IsRequired = true)]
        public double ZinMs { get; set; }
    }
}
