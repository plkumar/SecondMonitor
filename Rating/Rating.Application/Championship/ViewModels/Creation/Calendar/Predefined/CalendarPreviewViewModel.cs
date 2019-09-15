namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar.Predefined
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Championship.Calendar;
    using SecondMonitor.ViewModels;

    public class CalendarPreviewViewModel : AbstractViewModel<CalendarTemplate>
    {
        private string _title;
        private int _eventCount;
        private IReadOnlyCollection<string> _events;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public int EventCount
        {
            get => _eventCount;
            set => SetProperty(ref _eventCount, value);
        }

        public IReadOnlyCollection<string> Events
        {
            get => _events;
            set => SetProperty(ref _events, value);
        }


        protected override void ApplyModel(CalendarTemplate model)
        {
            Title = model.CalendarName;
            EventCount = model.Events.Count;
            Events = model.Events.Select(x => x.TrackTemplate.TrackName).ToList();
        }

        public override CalendarTemplate SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}