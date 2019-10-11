namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.Track;
    using SecondMonitor.ViewModels.TrackRecords;

    public class TrackOverviewViewModel : AbstractViewModel
    {
        public TrackOverviewViewModel(IViewModelFactory viewModelFactory)
        {
            TrackGeometryViewModel = viewModelFactory.Create<TrackGeometryViewModel>();
            OverallRecord = viewModelFactory.Create<RecordEntryViewModel>();
            CarRecord = viewModelFactory.Create<RecordEntryViewModel>();
            ClassRecord = viewModelFactory.Create<RecordEntryViewModel>();
        }

        public string TrackName { get; set; }
        public string LayoutLength { get; set; }

        public TrackGeometryViewModel TrackGeometryViewModel { get; }

        public RecordEntryViewModel OverallRecord { get; }

        public RecordEntryViewModel CarRecord { get; }

        public RecordEntryViewModel ClassRecord { get; }
    }
}