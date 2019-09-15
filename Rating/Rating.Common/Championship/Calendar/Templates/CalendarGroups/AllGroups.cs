namespace SecondMonitor.Rating.Common.Championship.Calendar.Templates.CalendarGroups
{
    using System.Linq;

    public static class AllGroups
    {
        public static CalendarTemplateGroup MainGroup = new CalendarTemplateGroup("All", new[] {DtmGroup.AllDtmGroups, TCRGroups.AllTCRGroups}, Enumerable.Empty<CalendarTemplate>());
    }
}