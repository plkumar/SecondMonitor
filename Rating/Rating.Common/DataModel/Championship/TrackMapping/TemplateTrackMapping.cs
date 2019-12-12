namespace SecondMonitor.Rating.Common.DataModel.Championship.TrackMapping
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class TemplateTrackMapping
    {
        [XmlAttribute]
        public string TemplateTrackName { get; set; }

        [XmlAttribute]
        public string SimulatorTrackName { get; set; }
    }
}