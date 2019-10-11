namespace SecondMonitor.Contracts.TrackRecords
{
    using DataModel.BasicProperties;
    using DataModel.TrackRecords;

    public interface ITrackRecordsProvider
    {
        bool TryGetOverallBestRecord(string simulatorName, string trackFullName, SessionType sessionType, out RecordEntryDto recordEntry);
        bool TryGetCarBestRecord(string simulatorName, string trackFullName, string carName, SessionType sessionType, out RecordEntryDto recordEntry);
        bool TryGetClassBestRecord(string simulatorName, string trackFullName, string className, SessionType sessionType, out RecordEntryDto recordEntry);
    }
}