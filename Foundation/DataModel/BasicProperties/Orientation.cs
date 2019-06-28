namespace SecondMonitor.DataModel.BasicProperties
{
    using System;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public class Orientation
    {
        [ProtoMember(1)]
        public Angle Roll { get; set; } = new Angle();

        [ProtoMember(2)]
        public Angle Pitch { get; set; } = new Angle();

        [ProtoMember(3)]
        public Angle Yaw { get; set; } = new Angle();
    }
}