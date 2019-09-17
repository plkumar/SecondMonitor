namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System;
    using Common.DataModel.Championship.TrackMapping;
    using Repository;

    public class TrackTemplateToSimTrackMapper : ITrackTemplateToSimTrackMapper
    {
        private readonly ISimulatorsTrackMappingRepository _simulatorsTrackMappingRepository;
        private readonly Lazy<SimulatorsTrackMapping> _simulatorsTracksMappingLazy;

        public TrackTemplateToSimTrackMapper(ISimulatorsTrackMappingRepository simulatorsTrackMappingRepository)
        {
            _simulatorsTrackMappingRepository = simulatorsTrackMappingRepository;
            _simulatorsTracksMappingLazy = new Lazy<SimulatorsTrackMapping>(() => _simulatorsTrackMappingRepository.LoadRatingsOrCreateNew());
        }

        protected SimulatorsTrackMapping SimulatorsTrackMapping => _simulatorsTracksMappingLazy.Value;

        public bool TryGetSimulatorTrackName(string simulatorName, string templateTrackName, out string simulatorTrackName)
        {
            return SimulatorsTrackMapping.TryGetTrackName(simulatorName, templateTrackName, out simulatorTrackName);
        }

        public void RegisterSimulatorTrackName(string simulatorName, string templateTrackName, string simulatorTrackName)
        {
            SimulatorsTrackMapping.SetTrackMapping(simulatorName, templateTrackName, simulatorTrackName);
        }

        public void SaveTrackMappings()
        {
            if (_simulatorsTracksMappingLazy.IsValueCreated)
            {
                _simulatorsTrackMappingRepository.Save(SimulatorsTrackMapping);
            }
        }
    }
}