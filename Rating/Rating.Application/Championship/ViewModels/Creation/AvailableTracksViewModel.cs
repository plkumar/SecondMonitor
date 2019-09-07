namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation
{
    using System.Collections.Generic;
    using System.Linq;
    using SecondMonitor.ViewModels;

    public class AvailableTracksViewModel : AbstractViewModel
    {
        private ICollection<AbstractTrackTemplateViewModel> _trackTemplateViewModels;

        public AvailableTracksViewModel()
        {
            _trackTemplateViewModels = Enumerable.Empty<AbstractTrackTemplateViewModel>().ToList();
        }

        public ICollection<AbstractTrackTemplateViewModel> TrackTemplateViewModels
        {
            get => _trackTemplateViewModels;
            set => SetProperty(ref _trackTemplateViewModels, value);
        }
    }
}