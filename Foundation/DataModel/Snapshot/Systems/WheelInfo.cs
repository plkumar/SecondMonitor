namespace SecondMonitor.DataModel.Snapshot.Systems
{
    using System;
    using System.Xml.Serialization;
    using BasicProperties;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public sealed class WheelInfo
    {
        private static readonly Temperature OptimalTemperature = Temperature.FromCelsius(85);
        private static readonly Temperature OptimalTemperatureWindow = Temperature.FromCelsius(10);
        private string _tyreVisualType;

        public WheelInfo()
        {
            _tyreVisualType = string.Empty;
            BrakeTemperature = new OptimalQuantity<Temperature>()
            {
                IdealQuantity = Temperature.FromCelsius(350),
                IdealQuantityWindow = Temperature.FromCelsius(200),
                ActualQuantity = Temperature.Zero
            };
            TyrePressure = new OptimalQuantity<Pressure>()
            {
                IdealQuantity = Pressure.Zero,
                IdealQuantityWindow = Pressure.Zero,
                ActualQuantity = Pressure.Zero
            };
            LeftTyreTemp = new OptimalQuantity<Temperature>()
            {
                IdealQuantity = Temperature.FromCelsius(OptimalTemperature.InCelsius),
                IdealQuantityWindow = Temperature.FromCelsius(OptimalTemperatureWindow.InCelsius),
            };
            RightTyreTemp = new OptimalQuantity<Temperature>()
            {
                IdealQuantity = Temperature.FromCelsius(OptimalTemperature.InCelsius),
                IdealQuantityWindow = Temperature.FromCelsius(OptimalTemperatureWindow.InCelsius),
            };
            CenterTyreTemp = new OptimalQuantity<Temperature>()
            {
                IdealQuantity = Temperature.FromCelsius(OptimalTemperature.InCelsius),
                IdealQuantityWindow = Temperature.FromCelsius(OptimalTemperatureWindow.InCelsius),
            };

            TyreCoreTemperature = new OptimalQuantity<Temperature>()
            {
                IdealQuantity = Temperature.FromCelsius(OptimalTemperature.InCelsius),
                IdealQuantityWindow = Temperature.FromCelsius(OptimalTemperatureWindow.InCelsius),
            };

            TyreWear = new TyreWear() {ActualWear = 0.0, NoWearWearLimit = 0.03, LightWearLimit = 0.25, HeavyWearLimit = 0.7};
            TyreType = string.Empty;

            RideHeight = Distance.FromMeters(0);
            SuspensionTravel = Distance.FromMeters(0);
            SuspensionVelocity = Velocity.Zero;
            Camber = new Angle();
            TyreLoad = new Force();
        }

        [ProtoMember(1)]
        public double Rps { get; set; } //Currently in Radians / s

        [ProtoMember(2)]
        public Distance SuspensionTravel { get; set; }

        [ProtoMember(3)]
        public Distance RideHeight { get; set; }

        [ProtoMember(4)]
        public OptimalQuantity<Temperature> BrakeTemperature { get; set; }

        [ProtoMember(5)]
        public OptimalQuantity<Pressure> TyrePressure { get; set; }

        [XmlAttribute]
        [ProtoMember(6)]
        public string TyreType { get; set; }

        [ProtoMember(7)]
        public TyreWear TyreWear { get; set; }

        [ProtoMember(8)]
        public bool Detached { get; set; }

        [ProtoMember(9)]
        public double DirtLevel { get; set; } = 0;

        [ProtoMember(10)]
        public OptimalQuantity<Temperature> LeftTyreTemp { get; set; }

        [ProtoMember(11)]
        public OptimalQuantity<Temperature> RightTyreTemp{ get; set; }

        [ProtoMember(12)]
        public OptimalQuantity<Temperature> CenterTyreTemp { get; set; }

        [ProtoMember(13)]
        public OptimalQuantity<Temperature> TyreCoreTemperature { get; set; }

        [ProtoMember(14)]
        public Velocity SuspensionVelocity { get; set; }

        [ProtoMember(15)]
        public Angle Camber { get; set; }

        [ProtoMember(16)]
        public Force TyreLoad { get; set; }

        [ProtoMember(17, IsRequired = true)]
        public double Slip { get; set; }

        [ProtoMember(18, IsRequired = true)]
        public string TyreVisualType
        {
            get => string.IsNullOrEmpty(_tyreVisualType) ? TyreType : _tyreVisualType;
            set => _tyreVisualType = value;
        }

    }

}
