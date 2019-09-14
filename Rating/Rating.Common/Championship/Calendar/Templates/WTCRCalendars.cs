namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class WTCRCalendars
    {
        public static CalendarTemplate WTCR2019 = new CalendarTemplate("2019 - WTCR", new []
        {
            new EventTemplate(TracksTemplates.MarrakechPresent),
            new EventTemplate(TracksTemplates.HungaroringPresent),
            new EventTemplate(TracksTemplates.SlovakiaRingTrack4),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.NordschleifeWithGPNoArena),
            new EventTemplate(TracksTemplates.VilaRealPresent),
            new EventTemplate(TracksTemplates.NingboPresent),
            new EventTemplate(TracksTemplates.SuzukaGPPresent),
            new EventTemplate(TracksTemplates.MacauPresent),
            new EventTemplate(TracksTemplates.SepangGPPresent),
        });
    }
}