namespace SecondMonitor.Rating.Common.DataModel.Championship.TrackMapping
{
    using System.Xml.Serialization;

    public class SessionDto
    {
        [XmlAttribute]
        public string DistanceDescription { get; set; }

        [XmlAttribute]
        public string Name { get; set; }
    }
}