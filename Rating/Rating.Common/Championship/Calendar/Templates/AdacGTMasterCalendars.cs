namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public class AdacGTMasterCalendars
    {
        public static CalendarTemplateGroup AllCalendars => new CalendarTemplateGroup("ADAC GT Masters", new CalendarTemplate[]{AdacGTMasters2013, AdacGTMasters2014, AdacGTMasters2015, AdacGTMasters2016, AdacGTMasters2017, AdacGTMasters2018, AdacGTMasters2019});

        public static CalendarTemplate AdacGTMasters2013 => new CalendarTemplate("2013 - ADAC GT Masters", new[]
        {
            new EventTemplate(TracksTemplates.Oschersleben),
            new EventTemplate(TracksTemplates.SpaPresent),
            new EventTemplate(TracksTemplates.SachsenringPresent),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.LausitzringGpPresent),
            new EventTemplate(TracksTemplates.SlovakiaRingTrack4),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });

        public static CalendarTemplate AdacGTMasters2014 => new CalendarTemplate("2014 - ADAC GT Masters", new[]
        {
            new EventTemplate(TracksTemplates.Oschersleben),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.LausitzringGpPresent),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.SlovakiaRingTrack4),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.SachsenringPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });

        public static CalendarTemplate AdacGTMasters2015 => new CalendarTemplate("2015 - ADAC GT Masters", new[]
        {
            new EventTemplate(TracksTemplates.Oschersleben),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.SpaPresent),
            new EventTemplate(TracksTemplates.LausitzringGpPresent),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.SachsenringPresent),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });

        public static CalendarTemplate AdacGTMasters2016 => new CalendarTemplate("2016 - ADAC GT Masters", new[]
        {
            new EventTemplate(TracksTemplates.Oschersleben),
            new EventTemplate(TracksTemplates.SachsenringPresent),
            new EventTemplate(TracksTemplates.LausitzringGpPresent),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });

        public static CalendarTemplate AdacGTMasters2017 => new CalendarTemplate("2017 - ADAC GT Masters", new[]
        {
            new EventTemplate(TracksTemplates.Oschersleben),
            new EventTemplate(TracksTemplates.LausitzringGpPresent),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.SachsenringPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });

        public static CalendarTemplate AdacGTMasters2018 => new CalendarTemplate("2018 - ADAC GT Masters", new[]
        {
            new EventTemplate(TracksTemplates.Oschersleben),
            new EventTemplate(TracksTemplates.AutodromMost),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.SachsenringPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });

        public static CalendarTemplate AdacGTMasters2019 => new CalendarTemplate("2019 - ADAC GT Masters", new[]
        {
            new EventTemplate(TracksTemplates.Oschersleben),
            new EventTemplate(TracksTemplates.AutodromMost),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
            new EventTemplate(TracksTemplates.SachsenringPresent),
        });
    }
}