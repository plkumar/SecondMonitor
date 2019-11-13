namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates.CalendarGroups
{
    using System.Linq;

    public static class AllGroups
    {
        public static CalendarTemplateGroup MainGroup = new CalendarTemplateGroup("All", new[] {DtmGroup.AllDtmGroups, TCRGroups.AllTCRGroups, AudiTTGroup, AdacGTMasterCalendars.AllCalendars, DrmCalendars.AllCalendars, IndycarCalendars.AllIndyCar}, Enumerable.Empty<CalendarTemplate>());

        public static CalendarTemplateGroup AudiTTGroup => new CalendarTemplateGroup("Audi TT Cup", new []{AudiTTCalendars.AudiTT2016, AudiTTCalendars.AudiTT2017});
    }
}