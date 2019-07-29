namespace SecondMonitor.Timing.TrackRecords.Controller
{
    public interface ITrackRecordsRepositoryFactory
    {
        TrackRecordsRepository Create();
    }
}