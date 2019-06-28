namespace SecondMonitor.DataModel.Snapshot.Systems
{
    using System;

    using BasicProperties;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public sealed class OilInfo
    {
        public OilInfo()
        {
            OilPressure = new Pressure();
            OptimalOilTemperature = new OptimalQuantity<Temperature>()
            {
                ActualQuantity = Temperature.Zero,
                IdealQuantity = Temperature.FromCelsius(100),
                IdealQuantityWindow = Temperature.FromCelsius(15),
            };
        }

        [ProtoMember(1)]
        public OptimalQuantity<Temperature> OptimalOilTemperature { get; set; }

        [ProtoMember(2)]
        public Pressure OilPressure { get; set; }
    }
}
