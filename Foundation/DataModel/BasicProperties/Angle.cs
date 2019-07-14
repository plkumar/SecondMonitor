using System;
using Newtonsoft.Json;

namespace SecondMonitor.DataModel.BasicProperties
{
    using System.Xml.Serialization;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public class Angle : IQuantity
    {
        public Angle()
        {
            InDegrees = 0;
            IsZero = true;
        }

        private Angle(double inDegrees)
        {
            InDegrees = inDegrees;
            IsZero = false;
        }

        [ProtoMember(1, IsRequired = true)]
        [XmlAttribute]
        public double InDegrees { get; set; }

        [JsonIgnore]
        public double InRadians => 0.0174533 * InDegrees;

        [JsonIgnore]
        public double InMilliRadians => InRadians * 1000;

        public IQuantity ZeroQuantity => new Angle();

        [XmlIgnore]
        public bool IsZero { get; private  set; }
        public double RawValue => InDegrees;

        public double GetValueInUnits(AngleUnits angleUnits)
        {
            switch (angleUnits)
            {
                case AngleUnits.Degrees:
                    return InDegrees;
                case AngleUnits.Radians:
                    return InRadians;
                case AngleUnits.MilliRadians:
                    return InMilliRadians;
                default:
                    throw new ArgumentOutOfRangeException(nameof(angleUnits), angleUnits, null);
            }
        }

        public static string GetUnitsSymbol(AngleUnits angleUnits)
        {
            switch (angleUnits)
            {
                case AngleUnits.Degrees:
                    return "°";
                case AngleUnits.Radians:
                    return "rad";
                case AngleUnits.MilliRadians:
                    return "mil";
                default:
                    throw new ArgumentOutOfRangeException(nameof(angleUnits), angleUnits, null);
            }
        }

        public static Angle GetFromDegrees(double angle)
        {
            return new Angle(angle);
        }

        public static Angle GetFromRadians(double angle)
        {
            return new Angle(angle / 0.0174533);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(Angle other)
        {
            return InDegrees.Equals(other.InDegrees) && IsZero == other.IsZero;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (InDegrees.GetHashCode() * 397) ^ IsZero.GetHashCode();
            }
        }

        public static Angle GetFromValue(double value, AngleUnits units)
        {
            switch (units)
            {
                case AngleUnits.Degrees:
                    return GetFromDegrees(value);
                case AngleUnits.Radians:
                    return GetFromRadians(value);
                case AngleUnits.MilliRadians:
                    return GetFromRadians(value / 1000);
                default:
                    throw new ArgumentOutOfRangeException(nameof(units), units, null);
            }
        }
    }
}