namespace SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO
{
    using System.Xml.Serialization;

    public class AggregatedChartSettingsDto
    {

        [XmlAttribute]
        public StintRenderingKind StintRenderingKind { get ; set; }
    }
}