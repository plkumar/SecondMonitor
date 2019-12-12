namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar
{
    using System.Windows.Input;
    using SecondMonitor.ViewModels;

    public abstract class AbstractCalendarEntryViewModel : AbstractViewModel
    {
        private string _trackName;
        private int _eventNumber;

        private string _customEventName;
        private string _originalEventName;

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

        public string CustomEventName
        {
            get => string.IsNullOrEmpty(_customEventName) ? _originalEventName : _customEventName;
            set => SetProperty(ref _customEventName, value);
        }

        public string OriginalEventName
        {
            get => _originalEventName;
            set => SetProperty(ref _originalEventName, value, nameof(CustomEventName));
        }

        public double LayoutLength { get; set; }

        public ICommand DeleteEntryCommand { get; set; }

    }
}