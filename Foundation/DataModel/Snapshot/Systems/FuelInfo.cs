namespace SecondMonitor.DataModel.Snapshot.Systems
{
    using System;

    using BasicProperties;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public sealed class FuelInfo
    {
        public FuelInfo()
        {
            FuelCapacity = new Volume();
            FuelRemaining = new Volume();
            FuelPressure = new Pressure();
        }

        [ProtoMember(1)]
        public Volume FuelCapacity { get; set; }

        [ProtoMember(2)]
        public Volume FuelRemaining { get; set; }

        [ProtoMember(3)]
        public Pressure FuelPressure { get; set; }
    }
}
