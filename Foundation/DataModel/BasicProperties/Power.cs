namespace SecondMonitor.DataModel.BasicProperties
{
    using System;
    using System.Xml.Serialization;
    using ProtoBuf;
    using Units;

    [ProtoContract]
    [Serializable]
    public class Power : IQuantity
    {
        public Power()
        {
            InKw = 0;
        }

        private Power(double inKw)
        {
            InKw = inKw;
        }

        [XmlAttribute]
        [ProtoMember(1, IsRequired = true)]
        public double InKw { get; set; }

        [XmlIgnore]
        public double InHorsePower
        {
            get => InKw * 1.34102;
            set => InKw = value / 1.34102;
        }

        public IQuantity ZeroQuantity => new Power();
        public bool IsZero => InKw == 0;
        public double RawValue => InKw;

        public static string GetUnitSymbol(PowerUnits powerUnits)
        {
            switch (powerUnits)
            {
                case PowerUnits.KW:
                    return "KW";
                case PowerUnits.HP:
                    return "HP";
                default:
                    throw new ArgumentOutOfRangeException(nameof(powerUnits), powerUnits, null);
            }
        }

        public double GetValueInUnit(PowerUnits powerUnits)
        {
            switch (powerUnits)
            {
                case PowerUnits.KW:
                    return InKw;
                case PowerUnits.HP:
                    return InHorsePower;
                default:
                    throw new ArgumentOutOfRangeException(nameof(powerUnits), powerUnits, null);
            }
        }

        public static Power FromKw(double inKw)
        {
            return new Power(inKw);
        }
    }
}