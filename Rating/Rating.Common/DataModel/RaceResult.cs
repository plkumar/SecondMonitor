namespace SecondMonitor.Rating.Common.DataModel
{
    using System;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class RaceResult
    {
        [XmlAttribute]
        public string ClassName { get; set; }

        [XmlAttribute]
        public string CarName { get; set; }

        [XmlAttribute]
        public string TrackName { get; set; }

        [XmlAttribute]
        public int FinishingPosition { get; set; }

        public RatingChange ClassRatingChange { get; set; }

        public RatingChange SimulatorRatingChange { get; set; }

        [XmlAttribute]
        public string CreationTimeFormatted
        {
            get => CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
            set => CreationTime = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime CreationTime { get; set; }

        [XmlAttribute]
        public int Difficulty { get; set; }
    }
}