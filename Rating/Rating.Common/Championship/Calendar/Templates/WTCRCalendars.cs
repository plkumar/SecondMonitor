namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class WTCRCalendars
    {
        public static CalendarTemplate WTCR2018 => new CalendarTemplate("2018 - WTCR", new[]
        {
            new EventTemplate(TracksTemplates.MarrakechPresent, "AFRIQUIA Race of Morocco"),
            new EventTemplate(TracksTemplates.HungaroringPresent, "Race of Hungary"),
            new EventTemplate(TracksTemplates.NordschleifeWithGPNoArena, "Race of Germany"),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent, "Race of the Netherlands"),
            new EventTemplate(TracksTemplates.VilaRealPresent, "Race of Portugal"),
            new EventTemplate(TracksTemplates.SlovakiaRingTrack4, "Race of Slovakia"),
            new EventTemplate(TracksTemplates.NingboPresent, "Race of China – Ningbo"),
            new EventTemplate(TracksTemplates.WuhanStreetCircuitPresent, "Race of China – Wuhan"),
            new EventTemplate(TracksTemplates.SuzukaGPPresent, "JVCKENWOOD Race of Japan"),
            new EventTemplate(TracksTemplates.MacauPresent, "Guia Race of Macau"),

        });

        public static CalendarTemplate WTCR2019 => new CalendarTemplate("2019 - WTCR", new []
        {
            new EventTemplate(TracksTemplates.MarrakechPresent,"Race of Morocco"),
            new EventTemplate(TracksTemplates.HungaroringPresent, "Race of Hungary"),
            new EventTemplate(TracksTemplates.SlovakiaRingTrack4, "Race of Slovakia"),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent, "Race of the Netherlands"),
            new EventTemplate(TracksTemplates.NordschleifeWithGPNoArena, "Race of Germany"),
            new EventTemplate(TracksTemplates.VilaRealPresent, "Race of Portugal"),
            new EventTemplate(TracksTemplates.NingboPresent, "Race of China"),
            new EventTemplate(TracksTemplates.SuzukaGPPresent, "Race of Japan"),
            new EventTemplate(TracksTemplates.MacauPresent, "Guia Race of Macau"),
            new EventTemplate(TracksTemplates.SepangGPPresent, "Race of Malaysia"),
        });
    }
}