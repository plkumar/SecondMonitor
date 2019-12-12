namespace SecondMonitor.Rating.Common.DataModel.Championship.TrackMapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    [Serializable]
    public class SimulatorTrackMapping
    {
        public SimulatorTrackMapping()
        {
            TemplateTrackMappings = new List<TemplateTrackMapping>();
        }

        [XmlAttribute]
        public string SimulatorName { get; set; }

        public List<TemplateTrackMapping> TemplateTrackMappings
        {
            get;
            set;
        }

        public bool TryGetTrackMapping(string trackTemplateName, out TemplateTrackMapping templateTrackMapping)
        {
            templateTrackMapping = TemplateTrackMappings.FirstOrDefault(x => x.TemplateTrackName == trackTemplateName);
            return templateTrackMapping != null;
        }

        public void SetTrackMapping(string trackTemplateName, string simulatorTrackName)
        {
            TemplateTrackMappings.RemoveAll(x => x.TemplateTrackName == trackTemplateName);
            TemplateTrackMappings.Add(new TemplateTrackMapping() {SimulatorTrackName = simulatorTrackName, TemplateTrackName = trackTemplateName});
        }


    }
}