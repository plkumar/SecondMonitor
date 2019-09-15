namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar.Predefined
{
    using Common.Championship.Calendar;
    using SecondMonitor.ViewModels;

    public class CalendarTemplateViewModel : AbstractViewModel<CalendarTemplate>
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected override void ApplyModel(CalendarTemplate model)
        {
            Title = model.CalendarName;
        }

        public override CalendarTemplate SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}