namespace SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO.DefaultCarProperties
{
    using System.Xml.Serialization;

    public class DefaultR3ECarProperties
    {
        [XmlAttribute]
        public string CarName { get; set; }

        [XmlAttribute]
        public double BumpTransitionFront { get; set; }

        [XmlAttribute]
        public double BumpTransitionRear { get; set; }

        [XmlAttribute]
        public double ReboundTransitionFront { get; set; }

        [XmlAttribute]
        public double ReboundTransitionRear { get; set; }
    }
}