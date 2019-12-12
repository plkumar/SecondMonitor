namespace SecondMonitor.Rating.Common.DataModel.Championship.Events
{
    using System;
    using System.Xml.Serialization;

    public class DriverSessionResultDto
    {
        [XmlAttribute]
        public Guid DriverGuid { get; set; }

        [XmlAttribute]
        public string DriverName { get; set; }

        [XmlAttribute]
        public int FinishPosition { get; set; }

        [XmlAttribute]
        public int TotalPoints { get; set; }

        [XmlAttribute]
        public int PointsGain { get; set; }

        [XmlAttribute]
        public int BeforeEventPosition { get; set; }

        [XmlAttribute]
        public int AfterEventPosition { get; set; }

        [XmlIgnore]
        public int PositionGained =>  BeforeEventPosition - AfterEventPosition;

        [XmlAttribute]
        public bool IsPlayer { get; set; }
    }
}