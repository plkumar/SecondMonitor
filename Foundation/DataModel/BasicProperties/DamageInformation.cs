namespace SecondMonitor.DataModel.BasicProperties
{
    using System;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public class DamageInformation
    {
        public DamageInformation()
        {
            Damage = 0;
            MediumDamageThreshold = 0.05;
            HeavyDamageThreshold = 0.25;
        }

        [ProtoMember(1, IsRequired = true)]
        public double Damage { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public double MediumDamageThreshold { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public double HeavyDamageThreshold { get; set; }
    }
}