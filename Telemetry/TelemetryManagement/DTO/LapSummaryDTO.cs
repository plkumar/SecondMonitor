namespace SecondMonitor.Telemetry.TelemetryManagement.DTO
{
    using System;
    using System.Xml.Serialization;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public class LapSummaryDto
    {

        [XmlIgnore]
        public string Id => $"{SessionIdentifier}-L{LapNumber}";

        [XmlAttribute]
        [ProtoMember(1)]
        public int LapNumber { get; set; }

        [XmlAttribute]
        [ProtoMember(2)]
        public string Simulator { get; set; }

        [XmlAttribute]
        [ProtoMember(3)]
        public string TrackName { get; set; }

        [XmlAttribute]
        [ProtoMember(4)]
        public string LayoutName { get; set; }

        [XmlIgnore]
        public TimeSpan LapTime { get; set; }

        [XmlIgnore]
        public TimeSpan Sector1Time { get; set; }

        [XmlIgnore]
        public TimeSpan Sector2Time { get; set; }

        [XmlIgnore]
        public TimeSpan Sector3Time { get; set; }

        [XmlAttribute]
        [ProtoMember(5)]
        public string SessionIdentifier { get; set; }

        [XmlAttribute]
        [ProtoMember(6, IsRequired = true)]
        public double LapTimeSeconds
        {
            get => LapTime.TotalSeconds;
            set => LapTime = TimeSpan.FromSeconds(value);
        }

        [XmlAttribute]
        [ProtoMember(7, IsRequired = true)]
        public double Sector1TimeSeconds
        {
            get => Sector1Time.TotalSeconds;
            set => Sector1Time = TimeSpan.FromSeconds(value);
        }

        [XmlAttribute]
        [ProtoMember(8, IsRequired = true)]
        public double Sector2TimeSeconds
        {
            get => Sector2Time.TotalSeconds;
            set => Sector2Time = TimeSpan.FromSeconds(value);
        }

        [XmlAttribute]
        [ProtoMember(9, IsRequired = true)]
        public double Sector3TimeSeconds
        {
            get => Sector3Time.TotalSeconds;
            set => Sector3Time = TimeSpan.FromSeconds(value);
        }

        [XmlAttribute]
        [ProtoMember(10, IsRequired = true)]
        public int Stint { get; set; }

        [XmlIgnore]
        public string CustomDisplayName
        {
            get;
            set;
        }

        public static bool operator ==(LapSummaryDto lap1, LapSummaryDto lap2)
        {
            if (lap1 is null && lap2 is null)
            {
                return true;
            }

            if (lap1 is null || lap2 is null)
            {
                return false;
            }

            return lap1.LapNumber == lap2.LapNumber && lap1.SessionIdentifier == lap2.SessionIdentifier;

        }

        public static bool operator !=(LapSummaryDto lap1, LapSummaryDto lap2)
        {
            return !(lap1 == lap2);
        }

        protected bool Equals(LapSummaryDto other)
        {
            return LapNumber == other.LapNumber && LapTime.Equals(other.LapTime) && string.Equals(SessionIdentifier, other.SessionIdentifier);
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

            return Equals((LapSummaryDto)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = LapNumber;
                hashCode = (hashCode * 397) ^ LapTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (SessionIdentifier != null ? SessionIdentifier.GetHashCode() : 0);
                return hashCode;
            }
        }

    }
}