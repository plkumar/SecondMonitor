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

        public List<TimedTelemetrySnapshot> GetReducedSet(TimeSpan pointGap)
        {
            List<TimedTelemetrySnapshot> reducedSet = new List<TimedTelemetrySnapshot>(DataPoints.Count / 2);
            TimedTelemetrySnapshot lastSnapshot = DataPoints[0];
            reducedSet.Add(lastSnapshot);
            for (int i = 1; i < DataPoints.Count; i++)
            {
                var currentSnapshot = DataPoints[i];
                if (currentSnapshot.LapTime - lastSnapshot.LapTime < pointGap)
                {
                    continue;
                }
                reducedSet.Add(currentSnapshot);
                lastSnapshot = currentSnapshot;
            }
            return reducedSet;
        }
    }
}