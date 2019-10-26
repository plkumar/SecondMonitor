namespace SecondMonitor.Rating.Application.Championship.ViewModels.Overview
{
    using System.Collections.Generic;
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.Events;
    using DataModel.TrackMap;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;
    using SimdataManagement;

    public class ChampionshipResultsOverviewViewModel : AbstractViewModel<ChampionshipDto>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly MapsLoader _mapsLoader;

        public ChampionshipResultsOverviewViewModel(IViewModelFactory viewModelFactory, IMapsLoaderFactory mapsLoaderFactory)
        {
            _viewModelFactory = viewModelFactory;
            _mapsLoader = mapsLoaderFactory.Create();
        }

        public string Title { get; private set; }
        public List<EventResultOverviewViewModel> EventsResults { get; private set; }

        protected override void ApplyModel(ChampionshipDto model)
        {
            Title = model.ChampionshipName;
            EventsResults = new List<EventResultOverviewViewModel>();
            foreach (EventDto eventDto in model.Events)
            {
                var eventResultViewModel = _viewModelFactory.Create<EventResultOverviewViewModel>();
                eventResultViewModel.FromModel(eventDto);
                EventsResults.Add(eventResultViewModel);

                if (_mapsLoader.TryLoadMap(model.SimulatorName, eventDto.TrackName, out TrackMapDto trackMapDto))
                {
                    eventResultViewModel.TrackGeometryViewModel.FromModel(trackMapDto.TrackGeometry);
                }

            }
        }

        public override ChampionshipDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}