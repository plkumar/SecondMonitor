namespace SecondMonitor.DataModel.BasicProperties
{
    using System;
    using System.Xml.Serialization;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public sealed class InputInfo
    {
        public InputInfo()
        {
            BrakePedalPosition = -1;
            ThrottlePedalPosition = -1;
            ClutchPedalPosition = -1;
        }

        [XmlAttribute]
        [ProtoMember(1, IsRequired = true)]
        public double BrakePedalPosition { get; set; }

        [XmlAttribute]
        [ProtoMember(2, IsRequired = true)]
        public double ThrottlePedalPosition { get; set; }

        [XmlAttribute]
        [ProtoMember(3, IsRequired = true)]
        public double ClutchPedalPosition { get; set; }

        [XmlAttribute]
        [ProtoMember(4, IsRequired = true)]
        public double SteeringInput { get; set; }

        [XmlAttribute]
        [ProtoMember(5, IsRequired = true)]
        public double WheelAngle { get; set; }

        [XmlAttribute]
        [ProtoMember(6, IsRequired = true)]
        public bool WheelAngleFilled { get; set; }
    }
}
