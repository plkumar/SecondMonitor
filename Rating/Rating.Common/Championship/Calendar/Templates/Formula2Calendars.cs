namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class Formula2Calendars
    {
        public static CalendarTemplateGroup Formula2AllGroup => new CalendarTemplateGroup("Formula 2", new CalendarTemplate[] { Formula22019 });

        public static CalendarTemplate Formula22019 => new CalendarTemplate("2019 - Formula 2", new[]
        {
            new EventTemplate(TracksTemplates.BahrainGP),
            new EventTemplate(TracksTemplates.BakuGP),
            new EventTemplate(TracksTemplates.CircuitDeCatalunyaGpPresent),
            new EventTemplate(TracksTemplates.MonacoPresent),
            new EventTemplate(TracksTemplates.PaulRicard1CV2),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.SilverstoneGpPresent),
            new EventTemplate(TracksTemplates.HungaroringPresent),
            new EventTemplate(TracksTemplates.SpaPresent),
            new EventTemplate(TracksTemplates.MonzaGpPresent),
            new EventTemplate(TracksTemplates.SochipGp),
            new EventTemplate(TracksTemplates.YasMarinaGrandPrix)
        });
    }
}