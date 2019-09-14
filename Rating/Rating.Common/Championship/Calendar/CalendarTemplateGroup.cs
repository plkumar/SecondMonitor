namespace SecondMonitor.Rating.Common.Championship.Calendar
{
    using System.Collections.Generic;
    using System.Linq;

    public class CalendarTemplateGroup
    {
        public CalendarTemplateGroup(string groupName, IEnumerable<CalendarTemplateGroup> childGroups, IEnumerable<CalendarTemplate> childCalendars)
        {
            GroupName = groupName;
            ChildGroups = childGroups.ToList();
            ChildCalendars = childCalendars.ToList();
        }

        public string GroupName { get; }

        public IReadOnlyCollection<CalendarTemplateGroup> ChildGroups { get; }
        public IReadOnlyCollection<CalendarTemplate> ChildCalendars { get; }
    }
}