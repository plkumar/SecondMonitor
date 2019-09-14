namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates.CalendarGroups
{
    using System.Linq;

    public static class TCRGroups
    {
        public static CalendarTemplateGroup AllTCRGroups = new CalendarTemplateGroup("TCR", new []{WTCRGroup}, Enumerable.Empty<CalendarTemplate>());

        public static CalendarTemplateGroup WTCRGroup = new CalendarTemplateGroup("WTCR", Enumerable.Empty<CalendarTemplateGroup>(), new[] {WTCRCalendars.WTCR2019});
    }
}