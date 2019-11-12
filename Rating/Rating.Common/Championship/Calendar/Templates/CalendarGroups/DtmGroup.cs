namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates.CalendarGroups
{
    using System.Linq;

    public class DtmGroup
    {
        public static CalendarTemplateGroup AllDtmGroups => new CalendarTemplateGroup("Deutsche Tourenwagen Masters (DTM)", new CalendarTemplateGroup[] { DtmCalendars.Dtm1990Group, DtmCalendars.Dtm2000Group,  DtmCalendars.Dtm2010Group});
    }
}