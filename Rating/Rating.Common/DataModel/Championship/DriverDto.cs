namespace SecondMonitor.Rating.Common.DataModel.Championship
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable]
    public class DriverDto
    {
        public DriverDto()
        {
            OtherNames = new List<string>();
            GlobalKey = Guid.NewGuid();
        }

        [XmlAttribute]
        public Guid GlobalKey { get; set; }

        [XmlAttribute]
        public string LastUsedName { get; set; }

        [XmlAttribute]
        public int TotalPoints { get; set; }

        [XmlAttribute]
        public int Position { get; set; }

        [XmlAttribute]
        public bool IsPlayer { get; set; }

        [XmlAttribute]
        public string LastCarName { get; set; }

        public List<string> OtherNames { get; set; }

        public void SetAnotherName(string driverName)
        {
            if (!OtherNames.Contains(LastUsedName))
            {
                OtherNames.Add(LastUsedName);
            }

            LastUsedName = driverName;
        }
    }
}