namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation
{
    using Calendar;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.Track;

    public class ExistingTrackTemplateViewModel : AbstractTrackTemplateViewModel
    {
        public ExistingTrackTemplateViewModel(IViewModelFactory viewModelFactory)
        {
            TrackGeometryViewModel = viewModelFactory.Create<TrackGeometryViewModel>();
        }



        public TrackGeometryViewModel TrackGeometryViewModel { get; }
    }
}