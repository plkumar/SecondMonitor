namespace SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO
{

    using System.Collections.Generic;

    using System.Xml.Serialization;

    [XmlRoot(ElementName = "TelemetrySettings")]
    public class TelemetrySettingsDto
    {
        [XmlAttribute]
        public XAxisKind XAxisKind { get; set; }

        [XmlElement(ElementName = "GraphSettings")]
        public List<GraphSettingsDto> GraphSettings { get; set; }

        [XmlElement(ElementName = "AggregatedChartSettings")]
        public AggregatedChartSettingsDto AggregatedChartSettings { get; set; }

        public CarsProperties CarsProperties { get; set;}
    }
}