namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar
{
    using System.Windows.Input;
    using SecondMonitor.ViewModels;

    public abstract class AbstractCalendarEntryViewModel : AbstractViewModel
    {
        private string _trackName;
        private int _eventNumber;

        public int EventNumber
        {
            get => _eventNumber;
            set => SetProperty(ref _eventNumber, value);
        }

        public string TrackName
        {
            get => _trackName;
            set => SetProperty(ref _trackName, value);
        }

        public ICommand DeleteEntryCommand { get; set; }
    }
}