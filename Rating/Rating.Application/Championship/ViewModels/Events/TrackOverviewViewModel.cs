namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.Track;

    public class TrackOverviewViewModel : AbstractViewModel
    {
        public TrackOverviewViewModel(IViewModelFactory viewModelFactory)
        {
            TrackGeometryViewModel = viewModelFactory.Create<TrackGeometryViewModel>();
        }

        public string TrackName { get; set; }
        public string LayoutLength { get; set; }

        public TrackGeometryViewModel TrackGeometryViewModel { get; }
    }
}