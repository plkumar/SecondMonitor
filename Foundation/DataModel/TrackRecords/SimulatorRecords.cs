namespace SecondMonitor.DataModel.TrackRecords
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    public class SimulatorRecords
    {
        public SimulatorRecords()
        {
            TrackRecords = new List<TrackRecord>();
        }

        [XmlAttribute]
        public string SimulatorName { get; set; }

        public List<TrackRecord> TrackRecords { get; set; }

        public TrackRecord GetOrCreateTrackRecord(string trackName)
        {
            var trackRecord = TrackRecords.FirstOrDefault(x => x.TrackName == trackName);
            if (trackRecord == null)
            {
                trackRecord = new TrackRecord()
                {
                    TrackName = trackName
                };
                TrackRecords.Add(trackRecord);
            }

            return trackRecord;
        }
    }
}