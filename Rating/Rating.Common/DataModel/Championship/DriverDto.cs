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
        }

        [XmlAttribute]
        public string LastUsedName { get; set; }

        public List<string> OtherNames { get; set; }
    }
}