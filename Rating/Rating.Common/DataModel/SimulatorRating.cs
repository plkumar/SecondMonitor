﻿namespace SecondMonitor.Rating.Common.DataModel
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Player;

    public class SimulatorRating
    {
        public SimulatorRating()
        {
            ClassRatings = new List<ClassRating>();
        }

        [XmlAttribute]
        public string SimulatorName { get; set; }

        public DriversRating PlayersRating { get; set; }

        public List<ClassRating> ClassRatings { get; set; }

        [XmlAttribute]
        public string LastPlayerClass { get; set; }
    }
}