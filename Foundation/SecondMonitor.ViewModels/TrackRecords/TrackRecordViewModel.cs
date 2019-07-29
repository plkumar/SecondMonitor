namespace SecondMonitor.ViewModels.TrackRecords
{
    using System;

    public class TrackRecordViewModel : AbstractViewModel
    {
        public string TrackName { get ; set; }

        public TimeSpan BestTime { get; set; }

        public string CarName { get; set; }

        public string ClassName { get; set; }

        public DateTime RecordSetDate { get; set; }
        public string SessionType { get; set; }
    }
}