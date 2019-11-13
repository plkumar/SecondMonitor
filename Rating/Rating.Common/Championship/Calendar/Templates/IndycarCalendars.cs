namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class IndycarCalendars
    {
        public static CalendarTemplateGroup AllIndyCar => new CalendarTemplateGroup("IndyCar", new CalendarTemplate[]{IndyCar2016});

        public static CalendarTemplate IndyCar2016 => new CalendarTemplate("2016 - IndyCar", new []
        {
            new EventTemplate(TracksTemplates.StPetersburg0316, "Firestone Grand Prix of St. Petersburg"),
            new EventTemplate(TracksTemplates.PhoenixInternationalRaceway, "Desert Diamond West Valley Phoenix Grand Prix"),
            new EventTemplate(TracksTemplates.LongBeach, "42nd Toyota Grand Prix of Long Beach"),
            new EventTemplate(TracksTemplates.BarberMotorsportsPark, "Honda Indy Grand Prix of Alabama"),
            new EventTemplate(TracksTemplates.IndianapolisMotorSpeedwayRoadPresent, "Angie's List Grand Prix of Indianapolis"),
            new EventTemplate(TracksTemplates.IndianapolisMotorSpeedwayOval, "100th Indianapolis 500 presented by PennGrade"),
            new EventTemplate(TracksTemplates.DetroitBelleIsle, "Chevrolet Dual in Detroit Presented by Quicken Loan"),
            new EventTemplate(TracksTemplates.DetroitBelleIsle, "Chevrolet Dual in Detroit Presented by Quicken Loan"),
            new EventTemplate(TracksTemplates.RoadAmerica, "Kohler Grand Prix"),
            new EventTemplate(TracksTemplates.IowaSpeedwayOval, "Iowa Corn 300"),
            new EventTemplate(TracksTemplates.TorontoExhibitionPlace, "Honda Indy Toronto"),
            new EventTemplate(TracksTemplates.MidOhioMainWithoutChicane, "Honda Indy 200"),
            new EventTemplate(TracksTemplates.PoconoRacewayOval, "ABC Supply 500"),
            new EventTemplate(TracksTemplates.TexasMotorSpeedwayOval, "Firestone 600"),
            new EventTemplate(TracksTemplates.WatkinsGlenGpWithInnerLoop, "Grand Prix at The Glen"),
            new EventTemplate(TracksTemplates.SonomaRacewayIndy, "GoPro Grand Prix of Sonoma"),

        });
    }
}