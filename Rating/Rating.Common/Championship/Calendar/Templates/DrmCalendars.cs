namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates
{
    using Tracks;

    public static class DrmCalendars
    {
        public static CalendarTemplateGroup AllCalendars = new CalendarTemplateGroup("Deutsche Rennsport Meisterschaft (DRM)", new CalendarTemplate[] { Drm1978, Drm1979});

        public static CalendarTemplate Drm1978 => new CalendarTemplate("1978 - DRM", new EventTemplate[]
        {
            new EventTemplate(TracksTemplates.ZolderGp7581, "IX. AvD/MVBL Rundstreckenrennen \"Bergischer Löwe\""),
            new EventTemplate(TracksTemplates.Nordschleife7382, "XIII. ADAC Goodyear 300 km Rennen"),
            new EventTemplate(TracksTemplates.Nordschleife7382, "41. Int. ADAC-Eifelrennen"),
            new EventTemplate(TracksTemplates.AVUSBerling, "Int. ADAC-Mampe-Avus-Rennen"),
            new EventTemplate(TracksTemplates.MainzFinthenAirport, "Int. AvD/HMSC Flugplatzrennen Mainz-Finthen"),
            new EventTemplate(TracksTemplates.Zandvoort7578, "ADAC-Trophy Zandvoort"),
            new EventTemplate(TracksTemplates.KasselCaldenAirfield, "Int. ADAC Super Sport Rennen Kassel-Calden"),
            new EventTemplate(TracksTemplates.HockenheimringGp7080, "Avd Rennsport-Trophäe um den Bilstein Pokal - Grosser Preis von Deutschland"),
            new EventTemplate(TracksTemplates.ZolderGp7581, "12. ADAC-Westfalen-Pokal-Rennen"),
            new EventTemplate(TracksTemplates.NorisringPresent, "ADAC Norisring Trophäe “200 Meilen von Nürnberg”"),
            new EventTemplate(TracksTemplates.NordschleifeBetonschleife, "Int. V. ADAC Bilstein Super Sprint"),
        } );


        public static CalendarTemplate Drm1979 => new CalendarTemplate("1979 - DRM", new EventTemplate[]
        {
            new EventTemplate(TracksTemplates.ZolderGp7581, "X. AvD/MVBL-Rundstreckenrennen „Bergischer Löwe“"),
            new EventTemplate(TracksTemplates.HockenheimringGp7080, "Int. AvD Deutschland Trophaë - Jim Clark Rennen"),
            new EventTemplate(TracksTemplates.Nordschleife7382, "42. Int. ADAC-Eifelrennen"),
            new EventTemplate(TracksTemplates.Salzburgring7685, "7. ADAC-Bavaria-Rennen Salzburgring"),
            new EventTemplate(TracksTemplates.MainzFinthenAirport, "AvD/HMSC-Flugplatz-Rennen Mainz-Finthen"),
            new EventTemplate(TracksTemplates.NorisringPresent, "ADAC-Norisring-Trophäe „200 Meilen von Nürnberg“"),
            new EventTemplate(TracksTemplates.Zandvoort79, "ADAC-Trophy Zandvoort"),
            new EventTemplate(TracksTemplates.DiepholzAirfieldCircuit, "12. ADAC-Flugplatzrennen Diepholz"),
            new EventTemplate(TracksTemplates.ZolderGp7581, "13. ADAC-Westfalen-Pokal-Rennen"),
            new EventTemplate(TracksTemplates.HockenheimringGp7080, "ADAC-Hessen-Cup"),
            new EventTemplate(TracksTemplates.NordschleifeBetonschleife, "Int. VI. ADAC Bilstein Super Sprint"),
        });
    }
}