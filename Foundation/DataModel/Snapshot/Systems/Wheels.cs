namespace SecondMonitor.DataModel.Snapshot.Systems
{
    using System;
    using System.Xml.Serialization;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public sealed class Wheels
    {
        public Wheels()
        {
            FrontRight = new WheelInfo();
            FrontLeft = new WheelInfo();
            RearRight = new WheelInfo();
            RearLeft = new WheelInfo();
        }

        [XmlIgnore]
        public WheelInfo[] AllWheels => new WheelInfo[] { FrontLeft, FrontRight, RearLeft, RearRight };

        [ProtoMember(1)]
        public WheelInfo FrontLeft { get; set; }

        [ProtoMember(2)]
        public WheelInfo FrontRight { get; set; }

        [ProtoMember(3)]
        public WheelInfo RearLeft { get; set; }

        [ProtoMember(4)]
        public WheelInfo RearRight { get; set; }
    }
}