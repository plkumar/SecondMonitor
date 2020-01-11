namespace SecondMonitor.DataModel.Summary
{
    using System;
    using System.Xml.Serialization;
    using BasicProperties;
    using Telemetry;

    [Serializable]
    public sealed class Lap
    {
        public Lap()
        {

        }

        public Lap(Driver driver, bool isValid)
        {
            Driver = driver;
            IsValid = isValid;
        }

        [XmlIgnore]
        public Driver Driver { get; set; }

        public int LapNumber { get; set; }

        public bool IsValid { get; set; }

        public SessionType SessionType { get; set; }

        public TimeSpan LapTime { get; set; } = TimeSpan.Zero;

        public TimeSpan Sector1 { get; set; } = TimeSpan.Zero;

        public TimeSpan Sector2 { get; set; } = TimeSpan.Zero;

        public TimeSpan Sector3 { get; set; } = TimeSpan.Zero;

        public TelemetrySnapshot LapEndSnapshot { get; set; }

        public TelemetrySnapshot LapStartSnapshot { get; set; }

    }
}