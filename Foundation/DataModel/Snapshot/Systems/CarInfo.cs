namespace SecondMonitor.DataModel.Snapshot.Systems
{
    using System;
    using System.Xml.Serialization;
    using BasicProperties;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public class CarInfo
    {
        public CarInfo()
        {
            WheelsInfo = new Wheels();
            OilSystemInfo = new OilInfo();
            FuelSystemInfo = new FuelInfo();
            WaterSystemInfo = new WaterInfo();
            Acceleration = new Acceleration();
            FrontHeight = Distance.ZeroDistance;
            RearHeight = Distance.ZeroDistance;
            TurboPressure = Pressure.Zero;
            CarDamageInformation = new CarDamageInformation();
            DrsSystem = new DrsSystem();
            BoostSystem = new BoostSystem();
            OverallDownForce = new Force();
            FrontDownForce = new Force();
            RearDownForce = new Force();
            FrontRollAngle = new Angle();
            RearRollAngle = new Angle();
            WorldOrientation = new Orientation();
        }

        [ProtoMember(1)]

        public Wheels WheelsInfo { get; set; }

        [ProtoMember(2)]
        public OilInfo OilSystemInfo { get; set; }

        [ProtoMember(3)]
        public FuelInfo FuelSystemInfo { get; set; }

        [ProtoMember(4)]
        public WaterInfo WaterSystemInfo { get; set; }

        [ProtoMember(5)]
        public Acceleration Acceleration { get; set; }

        [ProtoMember(6)]
        public Distance FrontHeight { get; set; }

        [ProtoMember(7)]
        public Distance RearHeight { get; set; }

        [ProtoMember(8)]
        [XmlAttribute]
        public string CurrentGear { get; set; } = string.Empty;

        [ProtoMember(9, IsRequired = true)]
        [XmlAttribute]
        public int EngineRpm { get; set; } = 0;

        [ProtoMember(10)]
        public Pressure TurboPressure { get; set; }

        [ProtoMember(11, IsRequired = true)]
        public bool SpeedLimiterEngaged { get; set; }

        [ProtoMember(12)]
        public CarDamageInformation CarDamageInformation { get; set; }

        public DrsSystem DrsSystem { get; set; }

        public BoostSystem BoostSystem { get; set; }

        [ProtoMember(13)]
        public Force OverallDownForce { get; set; }

        [ProtoMember(14)]
        public Force FrontDownForce { get; set; }

        [ProtoMember(15)]
        public Force RearDownForce { get; set; }

        [ProtoMember(16)]
        public Angle FrontRollAngle { get; set; }

        [ProtoMember(17)]
        public Angle RearRollAngle { get; set; }

        [ProtoMember(18)]
        public Orientation WorldOrientation { get; set; }

    }
}
