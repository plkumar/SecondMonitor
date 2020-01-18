namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public class Gt3Calendar
    {
        public static CalendarTemplateGroup AllGt3Calendar => new CalendarTemplateGroup("GT 3", new CalendarTemplateGroup[] { IntercontinentalGtChallenge, GtWorldChallenge });

        public static CalendarTemplateGroup IntercontinentalGtChallenge => new CalendarTemplateGroup("Intercontinental GT Challenge", new CalendarTemplate[] { IntercontinentalGtChallenge2019, IntercontinentalGtChallenge2020 });

        public static CalendarTemplateGroup GtWorldChallenge => new CalendarTemplateGroup("Blancpain GT World Challenge", new CalendarTemplateGroup[] { GtWorldChallengeEuropeSprint, GtWorldChallengeEuropeEndurance, GtWorldChallengeAsia, GtWorldChallengeAmerica });

        public static CalendarTemplateGroup GtWorldChallengeEuropeEndurance => new CalendarTemplateGroup("GT World Challenge Europe Endurance Cup", new CalendarTemplate[] { GtWorldChallengeEnduranceEurope2019 });

        public static CalendarTemplateGroup GtWorldChallengeEuropeSprint => new CalendarTemplateGroup("GT World Challenge Europe Sprint Cup", new CalendarTemplate[] { GtWorldChallengeEurope2019 });

        public static CalendarTemplateGroup GtWorldChallengeAsia => new CalendarTemplateGroup("Blancpain GT World Challenge Asia", new CalendarTemplate[] { GtWorldChallengeAsia2019 });

        public static CalendarTemplateGroup GtWorldChallengeAmerica => new CalendarTemplateGroup("Blancpain GT World Challenge America", new CalendarTemplate[] { GtWorldChallengeAmerica2019 });

        public static CalendarTemplate IntercontinentalGtChallenge2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.BathurstPresent, "Liqui Moly Bathurst 12 Hour"),
            new EventTemplate(TracksTemplates.LagunaSecaPresent, "California 8 Hours"),
            new EventTemplate(TracksTemplates.SpaPresent, "	Total 24 Hours of Spa"),
            new EventTemplate(TracksTemplates.SuzukaGPPresent, "Suzuka 10 Hours"),
            new EventTemplate(TracksTemplates.KyalamiGPPresent, "Kyalami 9 Hours"),
        });

        public static CalendarTemplate IntercontinentalGtChallenge2020 => new CalendarTemplate("2020", new[]
        {
            new EventTemplate(TracksTemplates.BathurstPresent, "Liqui Moly Bathurst 12 Hour"),
            new EventTemplate(TracksTemplates.SpaPresent, "	Total 24 Hours of Spa"),
            new EventTemplate(TracksTemplates.SuzukaGPPresent, "Suzuka 10 Hours"),
            new EventTemplate(TracksTemplates.IndianapolisMotorSpeedwayRoadPresent, "Indianapolis 8 Hours"),
            new EventTemplate(TracksTemplates.KyalamiGPPresent, "Kyalami 9 Hours"),
        });

        public static CalendarTemplate GtWorldChallengeEnduranceEurope2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.MonzaGpPresent, "3 Hours of Monza"),
            new EventTemplate(TracksTemplates.SilverstoneGpPresent, "3 Hours of Silverstone"),
            new EventTemplate(TracksTemplates.PaulRicard1AV2, "	Circuit Paul Ricard 1000kms"),
            new EventTemplate(TracksTemplates.SpaPresent, "Total 24 Hours of Spa"),
            new EventTemplate(TracksTemplates.CircuitDeCatalunyaGpPresent, "3 Hours of Barcelona"),
        });

        public static CalendarTemplate GtWorldChallengeEurope2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.BrandsHatchGpPresent),
            new EventTemplate(TracksTemplates.MisanoWorldCircuitPresent),
            new EventTemplate(TracksTemplates.ZandvoortGPPresent),
            new EventTemplate(TracksTemplates.NurburgringGpPresent),
            new EventTemplate(TracksTemplates.HungaroringPresent),
        });

        public static CalendarTemplate GtWorldChallengeAsia2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.SepangGPPresent),
            new EventTemplate(TracksTemplates.ChangGp),
            new EventTemplate(TracksTemplates.SuzukaGPPresent),
            new EventTemplate(TracksTemplates.FujiGpPresent),
            new EventTemplate(TracksTemplates.KoreaGp),
            new EventTemplate(TracksTemplates.ShanghaiGp),
        });

        public static CalendarTemplate GtWorldChallengeAmerica2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.CotaGP),
            new EventTemplate(TracksTemplates.VirginiaIntRacewayFull),
            new EventTemplate(TracksTemplates.CanadianTireMotosportPark),
            new EventTemplate(TracksTemplates.SonomaRacewayLong),
            new EventTemplate(TracksTemplates.WatkinsGlenGpWithInnerLoop),
            new EventTemplate(TracksTemplates.RoadAmerica),
            new EventTemplate(TracksTemplates.LasVegasCombinedLayout),
        });
    }
}