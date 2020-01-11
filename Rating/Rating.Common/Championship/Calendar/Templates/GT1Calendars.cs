namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public class GT1Calendars
    {
        public static CalendarTemplateGroup AllGt1Calendars => new CalendarTemplateGroup("GT 1", new CalendarTemplateGroup[] { FiaGt1WorldChampionshipCalendars });

        public static CalendarTemplateGroup FiaGt1WorldChampionshipCalendars => new CalendarTemplateGroup("FIA GT1 World Championship", new CalendarTemplate[] { FiaGt1World2011, FiaGt1World2012 });

        public static CalendarTemplate FiaGt1World2011 => new CalendarTemplate("2011", new[]
        {
            new EventTemplate(TracksTemplates.YasMarinaGrandPrix),
            new EventTemplate(TracksTemplates.ZolderGpPresent),
            new EventTemplate(TracksTemplates.AlgarveCircuit1Present),
            new EventTemplate(TracksTemplates.SachsenringPresent),
            new EventTemplate(TracksTemplates.SilverstoneGpPresent),
            new EventTemplate(TracksTemplates.NavarraSpeedCircuitLong),
            new EventTemplate(TracksTemplates.PaulRicard1CV2),
            new EventTemplate(TracksTemplates.OrdosGrandPrix),
            new EventTemplate(TracksTemplates.GoldenportMororPark),
            new EventTemplate(TracksTemplates.PotrerodelosFunesPresent),
        });

        public static CalendarTemplate FiaGt1World2012 => new CalendarTemplate("2012", new[]
        {
            new EventTemplate(TracksTemplates.NogaroGrandPrixPresent),
            new EventTemplate(TracksTemplates.ZolderGpPresent),
            new EventTemplate(TracksTemplates.NavarraSpeedCircuitLong),
            new EventTemplate(TracksTemplates.SlovakiaRingTrack4),
            new EventTemplate(TracksTemplates.AlgarveCircuit1Present),
            new EventTemplate(TracksTemplates.SlovakiaRingTrack4),
            new EventTemplate(TracksTemplates.MoscowRacewayFim),
            new EventTemplate(TracksTemplates.NurburgringGpPresent),
            new EventTemplate(TracksTemplates.DoningtonPark),
        });
    }
}