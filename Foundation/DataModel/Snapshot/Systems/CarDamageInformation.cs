namespace SecondMonitor.DataModel.Snapshot.Systems
{
    using System;
    using BasicProperties;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public class CarDamageInformation
    {
        public CarDamageInformation()
        {
            Engine = new DamageInformation();
            Transmission = new DamageInformation();
            Suspension = new DamageInformation();
            Bodywork = new DamageInformation();
        }

        [ProtoMember(1)]
        public DamageInformation Engine { get; set; }

        [ProtoMember(2)]
        public DamageInformation Transmission { get; set; }

        [ProtoMember(3)]
        public DamageInformation Suspension { get; set; }

        [ProtoMember(4)]
        public DamageInformation Bodywork { get; set; }
    }
}