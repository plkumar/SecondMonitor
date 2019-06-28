namespace SecondMonitor.DataModel.Snapshot.Systems
{
    using System;

    using BasicProperties;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public sealed class WaterInfo
    {
        public WaterInfo()
        {
            //WaterTemperature = Temperature.Zero;
            OptimalWaterTemperature = new OptimalQuantity<Temperature>()
            {
                ActualQuantity = Temperature.Zero,
                IdealQuantity = Temperature.FromCelsius(90),
                IdealQuantityWindow = Temperature.FromCelsius(10),
            };

            WaterPressure = Pressure.Zero;
        }


        //public Temperature WaterTemperature { get; set; }

        [ProtoMember(1)]
        public OptimalQuantity<Temperature> OptimalWaterTemperature { get; set; }

        [ProtoMember(2)]
        public Pressure WaterPressure { get; set; }
    }
}
