namespace SecondMonitor.Rating.Common.Championship.Calendar
{
    using System.Collections.Generic;
    using System.Linq;

    public class CalendarTemplate
    {
        public CalendarTemplate(string calendarName, IEnumerable<EventTemplate> events)
        {
            CalendarName = calendarName;
            Events = events.ToList();
        }

        public IReadOnlyCollection<EventTemplate> Events;

        public string CalendarName { get; }

    }
}