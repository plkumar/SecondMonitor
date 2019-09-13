namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar
{
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.Track;

    public class ExistingTrackCalendarEntryViewModel : AbstractCalendarEntryViewModel
    {
        public ExistingTrackCalendarEntryViewModel(IViewModelFactory viewModelFactory)
        {
            TrackGeometryViewModel = viewModelFactory.Create<TrackGeometryViewModel>();
        }

        public double LayoutLengthMeters { get; set; }

        public TrackGeometryViewModel TrackGeometryViewModel { get; }
    }
}