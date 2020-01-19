namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public class Gt4Calendar
    {
        public static CalendarTemplateGroup AllGt4Calendar => new CalendarTemplateGroup("GT 4", new CalendarTemplateGroup[] { Gt4Europe, Gt4British, Gt4America });

        public static CalendarTemplateGroup Gt4Europe => new CalendarTemplateGroup("GT4 European Series", new CalendarTemplate[] { Gt4Europe2019 });

        public static CalendarTemplateGroup Gt4British => new CalendarTemplateGroup("British GT Championship", new CalendarTemplate[] { Gt4British2019 });

        public static CalendarTemplateGroup Gt4America => new CalendarTemplateGroup("GT4 America Series", new CalendarTemplate[] { Gt4America2019SprintX, Gt4America2019Sprint });


        public static CalendarTemplate Gt4Europe2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.MonzaGpPresent),
            new EventTemplate(TracksTemplates.BrandsHatchGpPresent),
            new EventTemplate(TracksTemplates.PaulRicard1CV2),
            new EventTemplate(TracksTemplates.MisanoWorldCircuitPresent),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.NurburgringGpPresent),
        });

        public static CalendarTemplate Gt4British2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.OultonParkInternationalCircuit),
            new EventTemplate(TracksTemplates.Snetterton300),
            new EventTemplate(TracksTemplates.SilverstoneGpPresent),
            new EventTemplate(TracksTemplates.DoningtonPark),
            new EventTemplate(TracksTemplates.SpaPresent),
            new EventTemplate(TracksTemplates.BrandsHatchGpPresent),
            new EventTemplate(TracksTemplates.DoningtonPark),
        });

        public static CalendarTemplate Gt4America2019SprintX => new CalendarTemplate("2019 Sprint X", new[]
        {
            new EventTemplate(TracksTemplates.CotaGP),
            new EventTemplate(TracksTemplates.LagunaSecaPresent),
            new EventTemplate(TracksTemplates.VirginiaIntRacewayFull),
            new EventTemplate(TracksTemplates.CanadianTireMotosportPark),
            new EventTemplate(TracksTemplates.SonomaRacewayLong),
            new EventTemplate(TracksTemplates.PortlandInternationalRacewayPresent),
            new EventTemplate(TracksTemplates.WatkinsGlenGpWithInnerLoop),
            new EventTemplate(TracksTemplates.RoadAmerica),
            new EventTemplate(TracksTemplates.LasVegasCombinedLayout),
        });

        public static CalendarTemplate Gt4America2019Sprint => new CalendarTemplate("2019 Sprint", new[]
        {
            new EventTemplate(TracksTemplates.StPetersburgPresent),
            new EventTemplate(TracksTemplates.LongBeach),
            new EventTemplate(TracksTemplates.VirginiaIntRacewayFull),
            new EventTemplate(TracksTemplates.CanadianTireMotosportPark),
            new EventTemplate(TracksTemplates.SonomaRacewayLong),
            new EventTemplate(TracksTemplates.WatkinsGlenGpWithInnerLoop),
            new EventTemplate(TracksTemplates.RoadAmerica),
            new EventTemplate(TracksTemplates.LasVegasCombinedLayout),
        });

    }
}