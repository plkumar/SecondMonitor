namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class Formula3Calendars
    {
        public static CalendarTemplateGroup Formula3AllGroup => new CalendarTemplateGroup("Formula 3", new CalendarTemplateGroup[] {Formula3WorldChampionship, Formula3Asian, Formula3Americas });

        public static CalendarTemplateGroup Formula3Asian = new CalendarTemplateGroup("F3 Asian Championship", new CalendarTemplate[] { Formula3Asian2019 });
        public static CalendarTemplateGroup Formula3Americas = new CalendarTemplateGroup("F3 Americas Championship", new CalendarTemplate[] { Formula3Americas2019 });
        public static CalendarTemplateGroup Formula3WorldChampionship = new CalendarTemplateGroup("FIA Formula 3 Championship", new CalendarTemplate[] {Formula32019});

        public static CalendarTemplate Formula32019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.CircuitDeCatalunyaGpPresent),
            new EventTemplate(TracksTemplates.PaulRicard1CV2),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.SilverstoneGpPresent),
            new EventTemplate(TracksTemplates.HungaroringPresent),
            new EventTemplate(TracksTemplates.SpaPresent),
            new EventTemplate(TracksTemplates.MonzaGpPresent),
            new EventTemplate(TracksTemplates.SochipGp),
        });

        public static CalendarTemplate Formula3Americas2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.BarberMotorsportsPark),
            new EventTemplate(TracksTemplates.RoadAtlanta),
            new EventTemplate(TracksTemplates.PittsburghRaceComplexFull),
            new EventTemplate(TracksTemplates.VirginiaIntRacewayFull),
            new EventTemplate(TracksTemplates.RoadAmerica),
            new EventTemplate(TracksTemplates.SebringGpPresent),
        });

        public static CalendarTemplate Formula3Asian2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.SepangGPPresent),
            new EventTemplate(TracksTemplates.ChangGp),
            new EventTemplate(TracksTemplates.SuzukaGPPresent),
            new EventTemplate(TracksTemplates.ShanghaiGp),
            new EventTemplate(TracksTemplates.ShanghaiGp),
        });
    }
}