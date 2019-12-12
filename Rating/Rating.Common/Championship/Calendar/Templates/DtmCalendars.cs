namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class DtmCalendars
    {
        public static CalendarTemplateGroup Dtm1990Group => new CalendarTemplateGroup("1990\'s DTM", new CalendarTemplate[]
        {
            DTM1993,
            DTM1995
        });

        public static CalendarTemplateGroup Dtm2000Group => new CalendarTemplateGroup("2000\'s DTM", new CalendarTemplate[]
        {
            DTM2003,
            DTM2005
        });

        public static CalendarTemplateGroup Dtm2010Group => new CalendarTemplateGroup("2010\'s DTM", new CalendarTemplate[]
        {
            DTM2013,
            DTM2014,
            DTM2015,
            DTM2016,
            DTM2018,
            DTM2019,
        } );


        public static CalendarTemplate DTM1993 => new CalendarTemplate("1993 - DTM", new[]
        {
            new EventTemplate(TracksTemplates.ZolderGp86to93),
            new EventTemplate(TracksTemplates.HockenheimringGp92to01),
            new EventTemplate(TracksTemplates.NurburgringGp84to02),
            new EventTemplate(TracksTemplates.WunstorfAirfield),
            new EventTemplate(TracksTemplates.NordschleifeWithGp8302),
            new EventTemplate(TracksTemplates.NorisringPresent),
            new EventTemplate(TracksTemplates.DoningtonPark),
            new EventTemplate(TracksTemplates.DiepholzAirfieldCircuit),
            new EventTemplate(TracksTemplates.Alemannenring),
            new EventTemplate(TracksTemplates.AVUSBerling),
            new EventTemplate(TracksTemplates.HockenheimringGp92to01),
        });

        public static CalendarTemplate DTM1995 => new CalendarTemplate("1995 - DTM", new[]
        {
            new EventTemplate(TracksTemplates.HockenheimringGp92to01),
            new EventTemplate(TracksTemplates.AVUSBerling),
            new EventTemplate(TracksTemplates.MugelloGpPresent),
            new EventTemplate(TracksTemplates.HelsinkyThunder),
            new EventTemplate(TracksTemplates.NorisringPresent),
            new EventTemplate(TracksTemplates.DoningtonPark),
            new EventTemplate(TracksTemplates.DiepholzAirfieldCircuit),
            new EventTemplate(TracksTemplates.EstorilGp94to99),
            new EventTemplate(TracksTemplates.NurburgringGp84to02),
            new EventTemplate(TracksTemplates.Alemannenring),
            new EventTemplate(TracksTemplates.MagnyCourseGp92to02),
            new EventTemplate(TracksTemplates.HockenheimringGp92to01),
        });

        public static CalendarTemplate DTM2003 => new CalendarTemplate("2003 - DTM", new[]
        {
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
            new EventTemplate(TracksTemplates.AdriaFull),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.LausitzringDtmShortCoursePresent),
            new EventTemplate(TracksTemplates.NorisringPresent),
            new EventTemplate(TracksTemplates.DoningtonPark),
            new EventTemplate(TracksTemplates.NurburgringGpPresent),
            new EventTemplate(TracksTemplates.RedBullA1Ring),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });


        public static CalendarTemplate DTM2005 => new CalendarTemplate("2005 - DTM", new[]
        {
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
            new EventTemplate(TracksTemplates.LausitzringDtmShortCoursePresent),
            new EventTemplate(TracksTemplates.SpaPresent),
            new EventTemplate(TracksTemplates.BrnoPresent),
            new EventTemplate(TracksTemplates.Oschersleben00to06),
            new EventTemplate(TracksTemplates.NorisringPresent),
            new EventTemplate(TracksTemplates.NurburgringGpPresent),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.LausitzringGpPresent),
            new EventTemplate(TracksTemplates.IstanbulParkGp),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
        });

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