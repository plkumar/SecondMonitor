namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class Formula1Calendars
    {
        public static CalendarTemplateGroup Formula1AllGroup => new CalendarTemplateGroup("Formula 1", new CalendarTemplateGroup[] { Formula190s, Formula120092013, Formula1V6HybridEra });

        public static CalendarTemplateGroup Formula190s => new CalendarTemplateGroup("1990 - 1999", new CalendarTemplate[] { Formula11993, Formula11998 });

        public static CalendarTemplateGroup Formula120092013 => new CalendarTemplateGroup("2009 - 2013", new CalendarTemplate[] { Formula12013 });

        public static CalendarTemplateGroup Formula1V6HybridEra => new CalendarTemplateGroup("2014 - Present (V6 Hybrid Era)", new CalendarTemplate[]{Formula12019});

        public static CalendarTemplate Formula11993 => new CalendarTemplate("1993 - Formula 1", new[]
        {
            new EventTemplate(TracksTemplates.KyalamiGP9308, "South African Grand Prix"),
            new EventTemplate(TracksTemplates.InterlagosGpPresent, "Brazilian Grand Prix"),
            new EventTemplate(TracksTemplates.DoningtonParkGP8609, "European Grand Prix"),
            new EventTemplate(TracksTemplates.ImolaGp8594, "San Marino Grand Prix"),
            new EventTemplate(TracksTemplates.CircuitDeCatalunyaGp9194, "Spanish Grand Prix"),
            new EventTemplate(TracksTemplates.Monaco8696, "Monaco Grand Prix"),
            new EventTemplate(TracksTemplates.MontrealGp9193, "Canadian Grand Prix"),
            new EventTemplate(TracksTemplates.MagnyCourseGp92to02, "French Grand Prix"),
            new EventTemplate(TracksTemplates.SilverstoneGp9193, "British Grand Prix"),
            new EventTemplate(TracksTemplates.HockenheimringGp92to01, "German Grand Prix"),
            new EventTemplate(TracksTemplates.Hungaroring8902, "Hungarian Grand Prix"),
            new EventTemplate(TracksTemplates.Spa8393, "Belgian Grand Prix"),
            new EventTemplate(TracksTemplates.MonzaGp7994, "Italian Grand Prix"),
            new EventTemplate(TracksTemplates.EstorilGp72to93, "Portuguese Grand Prix"),
            new EventTemplate(TracksTemplates.SuzukaGP9199, "Japanese Grand Prix"),
            new EventTemplate(TracksTemplates.AdelaideGp8595, "Australian Grand Prix"),
        });

        public static CalendarTemplate Formula11998 => new CalendarTemplate("1998 - Formula 1", new[]
        {
            new EventTemplate(TracksTemplates.AlbertParkPresent, "	Qantas Australian Grand Prix"),
            new EventTemplate(TracksTemplates.InterlagosGpPresent, "Grande Premio Marlboro do Brasil"),
            new EventTemplate(TracksTemplates.BuenosAiresNo6, "Gran Premio Marlboro Argentina"),
            new EventTemplate(TracksTemplates.ImolaGp9506, "Gran Premio di San Marino"),
            new EventTemplate(TracksTemplates.CircuitDeCatalunyaGp9503, "Gran Premio Marlboro de Espana"),
            new EventTemplate(TracksTemplates.Monaco9702, "Grand Prix de Monaco"),
            new EventTemplate(TracksTemplates.MontrealGp9601, "Grand Prix Player's du Canada"),
            new EventTemplate(TracksTemplates.MagnyCourseGp92to02, "Mobil 1 Grand Prix de France"),
            new EventTemplate(TracksTemplates.SilverstoneGp9702, "RAC British Grand Prix"),
            new EventTemplate(TracksTemplates.RedBullA1Ring, "Grosser Preis von Osterreich"),
            new EventTemplate(TracksTemplates.HockenheimringGp92to01, "Grosser Mobil 1 Preis von Deutschland"),
            new EventTemplate(TracksTemplates.Hungaroring8902, "Marlboro Magyar Nagydij"),
            new EventTemplate(TracksTemplates.Spa9501, "Foster's Belgian Grand Prix"),
            new EventTemplate(TracksTemplates.MonzaGp9599, "Gran Premio Campari d'Italia"),
            new EventTemplate(TracksTemplates.NurburgringGp84to02, "Grosser Warsteiner Preis von Luxemburg"),
            new EventTemplate(TracksTemplates.SuzukaGP9199, "Fuji Television Japanese Grand Prix"),
        });

        public static CalendarTemplate Formula12013 => new CalendarTemplate("2013 - Formula 1", new []
        {
            new EventTemplate(TracksTemplates.AlbertParkPresent),
            new EventTemplate(TracksTemplates.SepangGPPresent),
            new EventTemplate(TracksTemplates.ShanghaiGp),
            new EventTemplate(TracksTemplates.BahrainGP),
            new EventTemplate(TracksTemplates.CircuitDeCatalunyaGpPresent),
            new EventTemplate(TracksTemplates.MonacoPresent),
            new EventTemplate(TracksTemplates.MontrealGpPresent),
            new EventTemplate(TracksTemplates.SilverstoneGpPresent),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent),
            new EventTemplate(TracksTemplates.HungaroringPresent),
            new EventTemplate(TracksTemplates.SpaPresent),
            new EventTemplate(TracksTemplates.MonzaGpPresent),
            new EventTemplate(TracksTemplates.Singapore),
            new EventTemplate(TracksTemplates.KoreaGp),
            new EventTemplate(TracksTemplates.SuzukaGPPresent),
            new EventTemplate(TracksTemplates.BudhGp),
            new EventTemplate(TracksTemplates.YasMarinaGrandPrix),
            new EventTemplate(TracksTemplates.CotaGP),
            new EventTemplate(TracksTemplates.InterlagosGpPresent),
        });

        public static CalendarTemplate Formula12019 => new CalendarTemplate("2019 - Formula 1", new[]
        {
            new EventTemplate(TracksTemplates.AlbertParkPresent, "Formula 1 Rolex Australian Grand Prix 2019"),
            new EventTemplate(TracksTemplates.BahrainGP, "Formula 1 Gulf Air Bahrain Grand Prix 2019"),
            new EventTemplate(TracksTemplates.ShanghaiGp, "Formula 1 Heineken Chinese Grand Prix 2019"),
            new EventTemplate(TracksTemplates.BakuGP, "Formula 1 Socar Azerbaijan Grand Prix 2019"),
            new EventTemplate(TracksTemplates.CircuitDeCatalunyaGpPresent, "Formula 1 Emirates Gran Premio De España 2019"),
            new EventTemplate(TracksTemplates.MonacoPresent, "Formula 1 Grand Prix De Monaco 2019"),
            new EventTemplate(TracksTemplates.MontrealGpPresent, "Formula 1 Pirelli Grand Prix Du Canada 2019"),
            new EventTemplate(TracksTemplates.PaulRicard1CV2, "Formula 1 Pirelli Grand Prix De France 2019"),
            new EventTemplate(TracksTemplates.RedBullRing, "Formula 1 Myworld Grosser Preis Von Österreich 2019"),
            new EventTemplate(TracksTemplates.SilverstoneGpPresent, "Formula 1 Rolex British Grand Prix 2019"),
            new EventTemplate(TracksTemplates.HockenheimringGpPresent, "Formula 1 Mercedes-Benz Grosser Preis Von Deutschland 2019"),
            new EventTemplate(TracksTemplates.HungaroringPresent, "Formula 1 Rolex Magyar Nagydíj 2019"),
            new EventTemplate(TracksTemplates.SpaPresent, "Formula 1 Johnnie Walker Belgian Grand Prix 2019"),
            new EventTemplate(TracksTemplates.MonzaGpPresent, "Formula 1 Gran Premio Heineken D'italia 2019"),
            new EventTemplate(TracksTemplates.Singapore, "Formula 1 Singapore Airlines Singapore Grand Prix 2019"),
            new EventTemplate(TracksTemplates.SochipGp, "Formula 1 Vtb Russian Grand Prix 2019"),
            new EventTemplate(TracksTemplates.SuzukaGPPresent, "Formula 1 Japanese Grand Prix 2019"),
            new EventTemplate(TracksTemplates.MexicoGpPresent, "Formula 1 Gran Premio De México 2019"),
            new EventTemplate(TracksTemplates.CotaGP, "Formula 1 Emirates United States Grand Prix 2019"),
            new EventTemplate(TracksTemplates.InterlagosGpPresent, "Formula 1 Heineken Grande Prêmio Do Brasil 2019"),
            new EventTemplate(TracksTemplates.YasMarinaGrandPrix, "Formula 1 Etihad Airways Abu Dhabi Grand Prix 2019")
        });
    }
}