namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class Formula4Calendars
    {
        public static CalendarTemplateGroup Formula4AllGroup => new CalendarTemplateGroup("Formula 4", new CalendarTemplateGroup[] {Formula4Japanese, Formula4Adac, Formula4British, Formula4SouthEastAsia });

        public static CalendarTemplateGroup Formula4Adac => new CalendarTemplateGroup("ADAC Formula 4", new CalendarTemplate[] { Formula4Adac2019 });
        public static CalendarTemplateGroup Formula4British => new CalendarTemplateGroup("F4 British Championship", new CalendarTemplate[] { Formula4British2019 });
        public static CalendarTemplateGroup Formula4Japanese => new CalendarTemplateGroup("F4 Japanese Championship", new CalendarTemplate[] {Formula4Japanese2019});
        public static CalendarTemplateGroup Formula4SouthEastAsia => new CalendarTemplateGroup("Formula 4 South East Asia Championship", new CalendarTemplate[] { Formula4SEAsia2019 });

        public static CalendarTemplate Formula4Japanese2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.OkayamaGpPresent),
            new EventTemplate(TracksTemplates.FujiGpPresent),
            new EventTemplate(TracksTemplates.SuzukaGPPresent),
            new EventTemplate(TracksTemplates.FujiGpPresent),
            new EventTemplate(TracksTemplates.AutopolisHitaInternational),
            new EventTemplate(TracksTemplates.SugoInternational),
            new EventTemplate(TracksTemplates.SuzukaGPPresent),
            new EventTemplate(TracksTemplates.TwinRingMotegiRoadCourse),
        });

        public static CalendarTemplate Formula4British2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.BrandsHatchIndyPresent),
            new EventTemplate(TracksTemplates.DoningtonParkNational),
            new EventTemplate(TracksTemplates.Thruxton),
            new EventTemplate(TracksTemplates.CroftMainCircuit),
            new EventTemplate(TracksTemplates.OultonParkIslandCircuit),
            new EventTemplate(TracksTemplates.Snetterton300),
            new EventTemplate(TracksTemplates.Thruxton),
            new EventTemplate(TracksTemplates.KnockhillNational),
            new EventTemplate(TracksTemplates.SilverstoneNationalPresent),
            new EventTemplate(TracksTemplates.BrandsHatchGpPresent),
        });

        public static CalendarTemplate Formula4Adac2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.Oschersleben),
            new EventTemplate(TracksTemplates.RedBullRing),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.NurburgringSprintPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
            new EventTemplate(TracksTemplates.SachsenringPresent),
        });

        public static CalendarTemplate Formula4SEAsia2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.SepangGPPresent),
            new EventTemplate(TracksTemplates.SepangGPPresent),
            new EventTemplate(TracksTemplates.ChangGp),
            new EventTemplate(TracksTemplates.ChangGp),
            new EventTemplate(TracksTemplates.MadrasIrungattukottaiFull),
            new EventTemplate(TracksTemplates.MadrasIrungattukottaiFull),
            new EventTemplate(TracksTemplates.SepangGPPresent),
            new EventTemplate(TracksTemplates.SepangGPPresent),
            new EventTemplate(TracksTemplates.SepangGPPresent),
            new EventTemplate(TracksTemplates.SepangGPPresent),
        });
    }
}