namespace SecondMonitor.Rating.Common.DataModel.Championship.TrackMapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class SimulatorsTrackMapping
    {
        public SimulatorsTrackMapping()
        {
            SimulatorTrackMappings =  new List<SimulatorTrackMapping>();
        }

        public List<SimulatorTrackMapping> SimulatorTrackMappings
        {
            get;
            set;
        }

        public bool TryGetTrackName(string simulatorName, string trackTemplateName, out string templateTrackMapping)
        {
            var simulatorTrackMapping = GetOrCreateSimulatorTrackMapping(simulatorName);
            if (simulatorTrackMapping.TryGetTrackMapping(trackTemplateName, out TemplateTrackMapping mapping))
            {
                templateTrackMapping = mapping.SimulatorTrackName;
                return true;
            }

            templateTrackMapping = string.Empty;
            return false;
        }

        public void SetTrackMapping(string simulatorName, string trackTemplateName, string simulatorTrackName)
        {
            var simulatorTrackMapping = GetOrCreateSimulatorTrackMapping(simulatorName);
            simulatorTrackMapping.SetTrackMapping(trackTemplateName, simulatorTrackName);
        }

        private SimulatorTrackMapping GetOrCreateSimulatorTrackMapping(string simulatorName)
        {
            SimulatorTrackMapping simulatorTrackMapping = SimulatorTrackMappings.FirstOrDefault(x => x.SimulatorName == simulatorName);

            if (simulatorTrackMapping != null)
            {
                return simulatorTrackMapping;
            }

            simulatorTrackMapping = new SimulatorTrackMapping() {SimulatorName = simulatorName};
            SimulatorTrackMappings.Add(simulatorTrackMapping);
            return simulatorTrackMapping;
        }
    }
}