namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar
{
    using System.Windows.Input;
    using SecondMonitor.ViewModels;

    public abstract class AbstractTrackTemplateViewModel : AbstractViewModel
    {
        public string TrackName { get; set; }

        public ICommand UseTemplateInCalendarCommand
        {
            get;
            set;
        }
    }
}