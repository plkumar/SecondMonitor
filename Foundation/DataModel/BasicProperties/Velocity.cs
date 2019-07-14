namespace SecondMonitor.DataModel.BasicProperties
{
    using System;
    using System.Xml.Serialization;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public class Velocity : IQuantity
    {
        public static Velocity Zero => FromMs(0);

        public Velocity()
        {
            InMs = 0;
        }

        private Velocity(double ms)
        {
            InMs = ms;
        }


        [XmlIgnore]
        public double InKph
        {
            get => InMs * 3.6;
            set => InMs = value / 3.6;
        }


        [ProtoMember(1, IsRequired = true)]
        [XmlAttribute]
        public double InMs { get; set; }


        public double InMph => InMs * 2.23694;


        public IQuantity ZeroQuantity => Zero;


        public bool IsZero => this == Zero;


        public double RawValue => InMs;


        public double InFps => InMs * 3.28084;


        public double InInPerSecond => InMs * 39.3701;


        public double InCentimeterPerSecond => InMs * 100;


        public double InMillimeterPerSecond => InMs * 1000;

        public static Velocity FromMs(double inMs)
        {
            return new Velocity(inMs);
        }

        public static Velocity FromKph(double inKph)
        {
            return new Velocity(inKph / 3.6);
        }

        public static Velocity FromMph(double inMph)
        {
            return new Velocity(inMph / 2.237);
        }

        public static string GetUnitSymbol(VelocityUnits units)
        {
            switch (units)
            {
                case VelocityUnits.Kph:
                    return "Kph";
                case VelocityUnits.Mph:
                    return "Mph";
                case VelocityUnits.Ms:
                    return "Ms";
                case VelocityUnits.Fps:
                    return "fps";
                case VelocityUnits.CmPerSecond:
                    return "cm/s";
                case VelocityUnits.InPerSecond:
                    return "In/s";
                case VelocityUnits.MMPerSecond:
                    return "mm/s";
                default:
                    throw new ArgumentOutOfRangeException(nameof(units), units, null);
            }
            throw new ArgumentException("Unable to return symbol for" + units.ToString());
        }

        public static bool operator <(Velocity v1, Velocity v2)
        {
            return v1.InMs < v2.InMs;
        }

        public static bool operator >(Velocity v1, Velocity v2)
        {
            return v1.InMs > v2.InMs;
        }

        public static bool operator <=(Velocity v1, Velocity v2)
        {
            return v1.InMs <= v2.InMs;
        }

        public static bool operator >=(Velocity v1, Velocity v2)
        {
            return v1.InMs >= v2.InMs;
        }

        public static Velocity operator -(Velocity v1, Velocity v2)
        {
            return FromMs(v1.InMs - v2.InMs);
        }

        protected bool Equals(Velocity other)
        {
            return InMs.Equals(other.InMs);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Velocity) obj);
        }

        public override int GetHashCode()
        {
            return InMs.GetHashCode();
        }

        public double GetValueInUnits(VelocityUnits units)
        {
            switch (units)
            {
                case VelocityUnits.Kph:
                    return InKph;
                case VelocityUnits.Mph:
                    return InMph;
                case VelocityUnits.Ms:
                    return InMs;
                case VelocityUnits.Fps:
                    return InFps;
                case VelocityUnits.CmPerSecond:
                    return InCentimeterPerSecond;
                case VelocityUnits.InPerSecond:
                    return InInPerSecond;
                case VelocityUnits.MMPerSecond:
                    return InMillimeterPerSecond;
                default:
                    throw new ArgumentException("Unable to return value in" + units);
            }
        }

        public string GetValueInUnits(VelocityUnits units, int decimalPlaces)
        {
            return GetValueInUnits(units).ToString($"F{decimalPlaces}");
        }

        public static Velocity FromUnits(double value, VelocityUnits units)
        {
            switch (units)
            {
                case VelocityUnits.Kph:
                    return FromKph(value);
                case VelocityUnits.Mph:
                    return FromMph(value);
                case VelocityUnits.Ms:
                    return FromMs(value);
                case VelocityUnits.Fps:
                    return FromMs(value / 3.28084);
                case VelocityUnits.CmPerSecond:
                    return FromMs(value / 100);
                case VelocityUnits.InPerSecond:
                    return FromMs(value / 39.3701);
                case VelocityUnits.MMPerSecond:
                    return FromMs(value / 1000);
                default:
                    throw new ArgumentException("Unable to return value in" + units);
            }
        }
    }
}
