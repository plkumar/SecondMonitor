namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class DtmCalendars
    {
        public static CalendarTemplateGroup Dtm2010Group => new CalendarTemplateGroup("2010\'s DTM", new CalendarTemplate[]
        {
            DTM2013,
            DTM2014,
            DTM2015,
            DTM2016,
            DTM2018,
            DTM2019,
        } );

        public static CalendarTemplate DTM2013 => new CalendarTemplate("2013 - DTM", new[]
        {
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
            new EventTemplate(TracksTemplates.BrandsHatchGpPresent),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.LausitzringDtmShortCoursePresent),
            new EventTemplate(TracksTemplates.NorisringPresent),
            new EventTemplate(TracksTemplates.MoscowRacewayFim),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.Oschersleben),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });

        public static CalendarTemplate DTM2014 => new CalendarTemplate("2014 - DTM", new[]
        {
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
            new EventTemplate(TracksTemplates.Oschersleben),
            new EventTemplate(TracksTemplates.HungaroringPresent),
            new EventTemplate(TracksTemplates.NorisringPresent),
            new EventTemplate(TracksTemplates.MoscowRacewayFim),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.LausitzringDtmShortCoursePresent),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });

        public static CalendarTemplate DTM2015 => new CalendarTemplate("2015 - DTM", new[]
        {
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
            new EventTemplate(TracksTemplates.LausitzringDtmShortCoursePresent),
            new EventTemplate(TracksTemplates.NorisringPresent),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.MoscowRacewayFim),
            new EventTemplate(TracksTemplates.Oschersleben),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });

        public static CalendarTemplate DTM2016 => new CalendarTemplate("2016 - DTM", new[]
        {
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.LausitzringDtmShortCoursePresent),
            new EventTemplate(TracksTemplates.NorisringPresent),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.MoscowRacewayFim),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.HungaroringPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),


        });

        public static CalendarTemplate DTM2018 => new CalendarTemplate("2018 - DTM", new[]
        {
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
            new EventTemplate(TracksTemplates.LausitzringGpPresent),
            new EventTemplate(TracksTemplates.HungaroringPresent),
            new EventTemplate(TracksTemplates.NorisringPresent),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.BrandsHatchGpPresent),
            new EventTemplate(TracksTemplates.MisanoWorldCircuitPresent),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });

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