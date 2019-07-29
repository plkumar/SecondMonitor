namespace SecondMonitor.ViewModels.TrackRecords
{
    using DataModel.TrackRecords;

    public interface IRecordViewModel : IViewModel<RecordEntryDto>
    {
        bool IsHighlighted { get; set; }
    }
}