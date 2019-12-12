namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class Formula1Calendars
    {
        public static CalendarTemplateGroup Formula1AllGroup => new CalendarTemplateGroup("Formula1", new CalendarTemplateGroup[] { Formula120092013, Formula1V6HybridEra });

        public static CalendarTemplateGroup Formula120092013 => new CalendarTemplateGroup("2009 - 2013", new CalendarTemplate[] { Formula12013 });

        public static CalendarTemplateGroup Formula1V6HybridEra => new CalendarTemplateGroup("2014 - Present (V6 Hybrid Era)", new CalendarTemplate[]{Formula12019});

        public static CalendarTemplate Formula12013 = new CalendarTemplate("2013 - Formula 1", new []
        {
            new EventTemplate(TracksTemplates.AlbertPartPresent),
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
            new EventTemplate(TracksTemplates.AlbertPartPresent, "Formula 1 Rolex Australian Grand Prix 2019"),
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