namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class DtmCalendars
    {
        public static CalendarTemplate DTM2019 => new CalendarTemplate("2019 - DTM", new[]
        {
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
            new EventTemplate(TracksTemplates.ZolderGpPresent),
            new EventTemplate(TracksTemplates.MisanoWorldCircuitPresent),
            new EventTemplate(TracksTemplates.NorisringPresent),
            new EventTemplate(TracksTemplates.TTAssenGpPresent),
            new EventTemplate(TracksTemplates.BrandsHatchGpPresent),
            new EventTemplate(TracksTemplates.LausitzringGpPresent),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });
    }
}