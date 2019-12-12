namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar
{
    using System.Linq;
    using Common.Championship.Calendar;
    using Controller;
    using DataModel.SimulatorContent;
    using DataModel.TrackMap;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.SimulatorContent;
    using SimdataManagement;

    public class CalendarEntryViewModelFactory : ICalendarEntryViewModelFactory
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ITrackTemplateToSimTrackMapper _trackTemplateToSimTrackMapper;
        private readonly ISimulatorContentController _simulatorContentController;
        private readonly MapsLoader _mapsLoader;

        public CalendarEntryViewModelFactory(IViewModelFactory viewModelFactory, ITrackTemplateToSimTrackMapper trackTemplateToSimTrackMapper, IMapsLoaderFactory mapsLoaderFactory, ISimulatorContentController simulatorContentController)
        {
            _viewModelFactory = viewModelFactory;
            _trackTemplateToSimTrackMapper = trackTemplateToSimTrackMapper;
            _simulatorContentController = simulatorContentController;
            _mapsLoader = mapsLoaderFactory.Create();
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

                newEntry.LayoutLength = existingTrackTemplateViewModel.LayoutLengthMeters;
                return newEntry;
            }

            return new EditableCalendarEntryViewModel()
            {
                TrackName = "ENTER TRACK NAME"
            };
        }

        public AbstractCalendarEntryViewModel Create(EventTemplate eventTemplate, string simulatorName, bool useCalendarEventNames, bool autoReplaceKnownTracks)
        {
            if (!string.IsNullOrEmpty(simulatorName) &&  autoReplaceKnownTracks && _trackTemplateToSimTrackMapper.TryGetSimulatorTrackName(simulatorName, eventTemplate.TrackTemplate.TrackName, out string simulatorTrackName))
            {
                Track trackDefinition = _simulatorContentController.GetAllTracksForSimulator(simulatorName).FirstOrDefault(x => x.Name == simulatorTrackName);
                if (trackDefinition != null)
                {
                    var newEntry = _viewModelFactory.Create<ExistingTrackCalendarEntryViewModel>();
                    newEntry.CustomEventName = useCalendarEventNames ? eventTemplate.EventName : string.Empty;
                    newEntry.TrackName = simulatorTrackName;
                    newEntry.LayoutLength = trackDefinition.LapDistance;
                    bool hasMap = _mapsLoader.TryLoadMap(simulatorName, simulatorTrackName, out TrackMapDto trackMapDto);
                    if (hasMap)
                    {
                        newEntry.TrackGeometryViewModel.FromModel(trackMapDto.TrackGeometry);
                    }

                    return newEntry;
                }
            }
            return new CalendarPlaceholderEntryViewModel()
                {
                    CustomEventName = useCalendarEventNames ? eventTemplate.EventName : string.Empty,
                    LayoutLength = eventTemplate.TrackTemplate.LayoutLength,
                    TrackName = eventTemplate.TrackTemplate.TrackName,
                };

        }
    }
}