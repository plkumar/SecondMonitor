namespace SecondMonitor.DataModel.TrackRecords
{
    using System.Collections.Generic;
    using System.Linq;

    public class TrackRecord
    {
        public TrackRecord()
        {
            VehicleRecords = new List<NamedRecordSet>();
            ClassRecords = new List<NamedRecordSet>();
            OverallRecord = new RecordSet();
        }

        public string TrackName { get; set; }

        public RecordSet OverallRecord { get; set; }

        public List<NamedRecordSet> VehicleRecords { get; set; }

        public List<NamedRecordSet> ClassRecords { get; set; }

        public NamedRecordSet GetOrCreateClassRecord(string className)
        {
            var classRecord = ClassRecords.FirstOrDefault(x => x.Name == className);
            if (classRecord == null)
            {
                classRecord = new NamedRecordSet()
                {
                    Name = className,
                };
                ClassRecords.Add(classRecord);
            }

            return classRecord;
        }

        public NamedRecordSet GetOrCreateVehicleRecord(string vehicleName)
        {
            var vehicleRecord = VehicleRecords.FirstOrDefault(x => x.Name == vehicleName);
            if (vehicleRecord == null)
            {
                vehicleRecord = new NamedRecordSet()
                {
                    Name = vehicleName,
                };
                VehicleRecords.Add(vehicleRecord);
            }

            return vehicleRecord;
        }
    }
}