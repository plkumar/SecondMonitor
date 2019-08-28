namespace SecondMonitor.DataModel.BasicProperties.Units
{
    using System;
    using System.Xml.Serialization;
    using ProtoBuf;

    [ProtoContract]
    [Serializable]
    public class Torque : IQuantity
    {

        public Torque()
        {
            InNm = 0;
        }

        private Torque(double inNm)
        {
            InNm = inNm;
        }

        [XmlIgnore]
        public IQuantity ZeroQuantity => new Torque();

        [XmlIgnore]
        public bool IsZero => InNm == 0;

        [XmlIgnore]
        public double RawValue => InNm;

        [XmlAttribute]
        [ProtoMember(1, IsRequired = true)]
        public double InNm { get; set; }

        [XmlIgnore]
        public double InPoundFeet
        {
            get => InNm * 0.73756;
            set => InNm = value / 0.73756;
        }

        public static string GetUnitSymbol(TorqueUnits torqueUnits)
        {
            switch (torqueUnits)
            {
                case TorqueUnits.Nm:
                    return "Nm";

                case TorqueUnits.lbf:
                    return "lb.-ft.";
                default:
                    throw new ArgumentOutOfRangeException(nameof(torqueUnits), torqueUnits, null);
            }
        }

        public double GetValueInUnit(TorqueUnits torqueUnits)
        {
            switch (torqueUnits)
            {
                case TorqueUnits.Nm:
                    return InNm;
                case TorqueUnits.lbf:
                    return InPoundFeet;
                default:
                    throw new ArgumentOutOfRangeException(nameof(torqueUnits), torqueUnits, null);
            }
        }

        public static Torque FromNm(double nm)
        {
            return new Torque(nm);
        }
    }
}