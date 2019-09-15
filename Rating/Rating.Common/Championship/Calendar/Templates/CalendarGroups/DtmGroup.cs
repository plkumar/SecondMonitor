namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates.CalendarGroups
{
    using System.Linq;

    public class DtmGroup
    {
        public static CalendarTemplateGroup AllDtmGroups => new CalendarTemplateGroup("DTM", new CalendarTemplateGroup[0], new CalendarTemplate[]{DtmCalendars.DTM2019});
    }
}