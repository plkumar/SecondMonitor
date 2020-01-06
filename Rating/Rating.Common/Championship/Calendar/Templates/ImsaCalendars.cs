namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class ImsaCalendars
    {
        public static CalendarTemplateGroup AllImsaCalendars => new CalendarTemplateGroup("IMSA", new CalendarTemplateGroup[] { SportsCarsChampionshipsCalendars, PilotChallengeCalendars, PorscheGT3CanadaCalendars, PorscheGT3USACalendars, IMSAGtChampionshipCalendars });

        public static CalendarTemplateGroup SportsCarsChampionshipsCalendars => new CalendarTemplateGroup("SportsCar Championship", new CalendarTemplate[] { SportsCarChallenge2019 });

        public static CalendarTemplateGroup PilotChallengeCalendars => new CalendarTemplateGroup("Pilot Challenge", new CalendarTemplate[] {PilotChallenge2019 });

        public static CalendarTemplateGroup PorscheGT3CanadaCalendars => new CalendarTemplateGroup("GT3 Cup Challenge Canada", new CalendarTemplate[] { PorscheGT3Canada2019 });

        public static CalendarTemplateGroup PorscheGT3USACalendars => new CalendarTemplateGroup("GT3 Cup Challenge USA", new CalendarTemplate[] { PorscheGT3USA2019 });

        public static CalendarTemplateGroup IMSAGtChampionshipCalendars => new CalendarTemplateGroup("IMSA GT Championship", new CalendarTemplate[] { ImsaGTChampionship1979, ImsaGTChampionship1987, ImsaGTChampionship1994 });

        public static CalendarTemplate SportsCarChallenge2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.DaytonaRoadCourse, "ROLEX 24 AT DAYTONA"),
            new EventTemplate(TracksTemplates.SebringGpPresent, "MOBIL 1 TWELVE HOURS OF SEBRING PRESENTED BY ADVANCE AUTO PARTS"),
            new EventTemplate(TracksTemplates.LongBeach, "BUBBA BURGER SPORTS CAR GRAND PRIX AT LONG BEACH"),
            new EventTemplate(TracksTemplates.MidOhioMainWithoutChicane, "ACURA SPORTS CAR CHALLENGE AT MID-OHIO"),
            new EventTemplate(TracksTemplates.DetroitBelleIsle, " CHEVROLET DETROIT GRAND PRIX PRESENTED BY LEAR"),
            new EventTemplate(TracksTemplates.WatkinsGlenGpWithInnerLoop, " SAHLEN'S SIX HOURS OF THE GLEN"),
            new EventTemplate(TracksTemplates.CanadianTireMotosportPark, "MOBIL 1 SPORTSCAR GRAND PRIX PRESENTED BY ACURA"),
            new EventTemplate(TracksTemplates.LimeRockRoadCourse, "NORTHEAST GRAND PRIX"),
            new EventTemplate(TracksTemplates.RoadAmerica, "ROAD AMERICA"),
            new EventTemplate(TracksTemplates.VirginiaIntRacewayFull, "MICHELIN GT CHALLENGE AT VIR"),
            new EventTemplate(TracksTemplates.LagunaSecaPresent, "WEATHERTECH RACEWAY LAGUNA SECA"),
            new EventTemplate(TracksTemplates.RoadAtlanta, "MOTUL PETIT LE MANS"),
        });

        public static CalendarTemplate PilotChallenge2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.DaytonaRoadCourse, "BMW Endurance Challenge at Daytona"),
            new EventTemplate(TracksTemplates.SebringGpPresent, "Alan Jay Automotive Network 120"),
            new EventTemplate(TracksTemplates.MidOhioMainWithoutChicane, "Mid-Ohio 120"),
            new EventTemplate(TracksTemplates.WatkinsGlenGpWithInnerLoop, "Tioga Downs Casino Resort 240 at The Glen"),
            new EventTemplate(TracksTemplates.CanadianTireMotosportPark, "Canadian Tire Motorsport Park 120"),
            new EventTemplate(TracksTemplates.LimeRockRoadCourse, "	Lime Rock Park 120"),
            new EventTemplate(TracksTemplates.RoadAmerica, "Road America 120"),
            new EventTemplate(TracksTemplates.VirginiaIntRacewayFull, "Virginia Is For Racing Lovers Grand Prix"),
            new EventTemplate(TracksTemplates.LagunaSecaPresent, "	WeatherTech Raceway Laguna Seca 12"),
            new EventTemplate(TracksTemplates.RoadAtlanta, "Fox Factory 120"),
        });

        public static CalendarTemplate PorscheGT3Canada2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.CanadianTireMotosportPark, "VICTORIA DAY SPEEDFEST WEEKEND"),
            new EventTemplate(TracksTemplates.MontrealGpPresent, "CIRCUIT GILLES VILLENEUVE"),
            new EventTemplate(TracksTemplates.CanadianTireMotosportPark, "MOBIL 1 SPORTSCAR GRAND PRIX PRESENTED BY ACURA"),
            new EventTemplate(TracksTemplates.TorontoExhibitionPlace, "STREETS OF TORONTO"),
            new EventTemplate(TracksTemplates.RoadAmerica, "ROAD AMERICA"),
            new EventTemplate(TracksTemplates.CircuitMontTremblantFull, "CIRCUIT MONT-TREMBLANT"),

        });

        public static CalendarTemplate PorscheGT3USA2019 => new CalendarTemplate("2019", new[]
        {
            new EventTemplate(TracksTemplates.BarberMotorsportsPark, "BARBER MOTORSPORTS PARK"),
            new EventTemplate(TracksTemplates.MidOhioMainWithoutChicane, "ACURA SPORTS CAR CHALLENGE AT MID-OHIO"),
            new EventTemplate(TracksTemplates.MontrealGpPresent, "CIRCUIT GILLES VILLENEUVE"),
            new EventTemplate(TracksTemplates.WatkinsGlenGpWithInnerLoop, "SAHLEN'S SIX HOURS OF THE GLEN"),
            new EventTemplate(TracksTemplates.RoadAmerica, "ROAD AMERICA"),
            new EventTemplate(TracksTemplates.VirginiaIntRacewayFull, "MICHELIN GT CHALLENGE AT VIR"),
            new EventTemplate(TracksTemplates.LagunaSecaPresent, "WEATHERTECH RACEWAY LAGUNA SECA"),
            new EventTemplate(TracksTemplates.RoadAtlanta, "MOTUL PETIT LE MANS"),

        });

        public static CalendarTemplate ImsaGTChampionship1979 => new CalendarTemplate("1979", new[]
        {
            new EventTemplate(TracksTemplates.DaytonaRoadCourse7583, "24 Hour Pepsi Challenge"),
            new EventTemplate(TracksTemplates.SebringGp6782, "Coca-Cola 12 Hours of Sebring	"),
            new EventTemplate(TracksTemplates.RoadAtlanta7097, "Road Atlanta Grand Prix"),
            new EventTemplate(TracksTemplates.RiversideInternationalRaceway, "Los Angeles Times Grand Prix"),
            new EventTemplate(TracksTemplates.LagunaSeca6885, "Datsun Monterey Triple Crown"),
            new EventTemplate(TracksTemplates.Hallett, "Mother's Day Grand Prix"),
            new EventTemplate(TracksTemplates.LimeRockRoadCourse, "Coca-Cola 350"),
            new EventTemplate(TracksTemplates.BrainerdInternationalRaceway6888, "Pepsi Grand Prix"),
            new EventTemplate(TracksTemplates.DaytonaRoadCourse7583, "Paul Revere 250"),
            new EventTemplate(TracksTemplates.MidOhio6389, "Mid-Ohio GT 250"),
            new EventTemplate(TracksTemplates.SonomaRaceway6892, "Sprite Grand Prix"),
            new EventTemplate(TracksTemplates.PortlandInternationalRaceway7183, "G.I. Joe's Grand Prix"),
            new EventTemplate(TracksTemplates.RoadAmerica, "Pabst 500"),
            new EventTemplate(TracksTemplates.RoadAtlanta7097, "Grand Prix of Road Atlanta"),
            new EventTemplate(TracksTemplates.DaytonaRoadCourse7583, "Winston GT 250"),

        });

        public static CalendarTemplate ImsaGTChampionship1987 => new CalendarTemplate("1987", new[]
        {
            new EventTemplate(TracksTemplates.DaytonaRoadCourse8502, "24 Hour Pepsi Challenge"),
            new EventTemplate(TracksTemplates.StretsOfMiami8704, "Grand Prix of Miami"),
            new EventTemplate(TracksTemplates.SebringGp8790, "12 Hours of Sebring"),
            new EventTemplate(TracksTemplates.RoadAtlanta7097, "Atlanta Journal-Constitution Grand Prix	"),
            new EventTemplate(TracksTemplates.RiversideInternationalRaceway, "Los Angeles Times Grand Prix"),
            new EventTemplate(TracksTemplates.LagunaSeca8687, "Nissan Monterey Triple Crown"),
            new EventTemplate(TracksTemplates.WildHorsePassRoadCourse, "Arizona 300"),
            new EventTemplate(TracksTemplates.LimeRockRoadCourse, "Lime Rock Grand Prix"),
            new EventTemplate(TracksTemplates.MidOhio6389, "Champion Spark Plug Grand Prix"),
            new EventTemplate(TracksTemplates.PalmBeach6400, "Grand Prix of Palm Beach"),
            new EventTemplate(TracksTemplates.RoadAtlanta7097, "Kuppenheimer GT Challenge"),
            new EventTemplate(TracksTemplates.WatkinsGlenGpWithoutInnerLoop, "Camel Continental"),
            new EventTemplate(TracksTemplates.SummitPointMainCourse, "Mid-Atlantic Toyota Grand Prix"),
            new EventTemplate(TracksTemplates.PortlandInternationalRaceway8491, "G.I. Joe's Grand Prix"),
            new EventTemplate(TracksTemplates.SonomaRaceway6892, "Ford California Grand Prix"),
            new EventTemplate(TracksTemplates.RoadAmerica, "Löwenbräu Classic"),
            new EventTemplate(TracksTemplates.SanAntonioStreetCourse, "Nissan Grand Prix of San Antonio"),
            new EventTemplate(TracksTemplates.LimeRockRoadCourse, "150 Lap Lime Rock"),
            new EventTemplate(TracksTemplates.WatkinsGlenGpWithoutInnerLoop, "	Kodak Copier 500"),
            new EventTemplate(TracksTemplates.ColumbusStreetCourse, "Columbus Ford Dealers 500"),
            new EventTemplate(TracksTemplates.DelMarFairgrounds, "Camel Grand Prix of Southern California"),
        });

        public static CalendarTemplate ImsaGTChampionship1994 => new CalendarTemplate("1994", new[]
       {
            new EventTemplate(TracksTemplates.DaytonaRoadCourse8502, "Rolex 24 at Daytona"),
            new EventTemplate(TracksTemplates.SebringGp9195, "Contac 12 Hours of Sebring"),
            new EventTemplate(TracksTemplates.RoadAtlanta7097, "Grand Prix of Atlanta"),
            new EventTemplate(TracksTemplates.LimeRockRoadCourse, "The New England Dodge Dealers Grand Prix"),
            new EventTemplate(TracksTemplates.WatkinsGlenGpWithInnerLoop, "The Glen Continental"),
            new EventTemplate(TracksTemplates.IndianapolisRacewayPark, "Indy Grand Prix"),
            new EventTemplate(TracksTemplates.LagunaSeca9095, "Monterey Sports Car Grand Prix"),
            new EventTemplate(TracksTemplates.PortlandInternationalRaceway9204, "Grand Prix of Portland"),
            new EventTemplate(TracksTemplates.PhoenixInternationalRacewayRoad9111, "Exxon World Sports Car Championships")
       });
    }
}
