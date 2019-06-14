namespace SecondMonitor.Rating.Common.DataModel
{
    using System.Xml.Serialization;

    public class RatingChange
    {
        [XmlAttribute]
        public int RatingBeforeChange { get; set; }

        [XmlAttribute]
        public int RatingAfterChange { get; set; }

        [XmlIgnore]
        public int Change => RatingAfterChange - RatingBeforeChange;
    }
}