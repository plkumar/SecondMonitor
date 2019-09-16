namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar
{
    using Common.Championship.Calendar;
    using SecondMonitor.ViewModels.Factory;

    public class CalendarEntryViewModelFactory : ICalendarEntryViewModelFactory
    {
        private readonly IViewModelFactory _viewModelFactory;

        public CalendarEntryViewModelFactory(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public AbstractCalendarEntryViewModel Create(AbstractTrackTemplateViewModel trackTemplate)
        {
            if (trackTemplate is ExistingTrackTemplateViewModel existingTrackTemplateViewModel)
            {
                var newEntry = _viewModelFactory.Create<ExistingTrackCalendarEntryViewModel>();
                newEntry.TrackName = existingTrackTemplateViewModel.TrackName;
                if (existingTrackTemplateViewModel.TrackGeometryViewModel.OriginalModel != null)
                {
                    newEntry.TrackGeometryViewModel.FromModel(existingTrackTemplateViewModel.TrackGeometryViewModel.OriginalModel);
                }

                newEntry.LayoutLengthMeters = existingTrackTemplateViewModel.LayoutLengthMeters;
                return newEntry;
            }

            return new EditableCalendarEntryViewModel()
            {
                TrackName = "ENTER TRACK NAME"
            };
        }

        public AbstractCalendarEntryViewModel Create(EventTemplate eventTemplate, string simulatorName)
        {
            return new CalendarPlaceholderEntryViewModel()
            {
                CustomEventName = eventTemplate.EventName,
                LayoutLength = eventTemplate.TrackTemplate.LayoutLength,
                TrackName = eventTemplate.TrackTemplate.TrackName,
            };
        }
    }
}