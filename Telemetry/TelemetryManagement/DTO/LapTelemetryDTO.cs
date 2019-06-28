namespace SecondMonitor.Telemetry.TelemetryManagement.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;
    using DataModel.Telemetry;
    using ProtoBuf;

    [XmlRoot(ElementName = "LapTelemetry")]
    [Serializable]
    [ProtoContract]
    public class LapTelemetryDto
    {
        public LapTelemetryDto()
        {
            DataPoints = new List<TimedTelemetrySnapshot>();
        }

        [ProtoMember(1)]
        public LapSummaryDto LapSummary { get; set; }

        [ProtoMember(2)]
        public List<TimedTelemetrySnapshot> DataPoints { get; set; }

        protected TimedTelemetrySnapshot[] TimedTelemetrySnapshots { get; set; }

        public void MigrateToProtoBuf()
        {
            DataPoints = TimedTelemetrySnapshots.ToList();
        }
    }
}