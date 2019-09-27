namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Session.SessionLength
{
    using System.Linq;
    using SecondMonitor.ViewModels;

    public class TimeLengthDefinitionViewModel : AbstractViewModel, ISessionLengthDefinitionViewModel
    {
        private int _length;
        private bool _extraLap;
        private string _selectedUnits;

        public TimeLengthDefinitionViewModel()
        {
            AvailableUnits = new[] { "Minutes","Hours" };
            SelectedUnits = AvailableUnits.First();
            _extraLap = false;
            _length = 30;
        }

        public string LengthKind => "Time";
        public string GetTextualDescription(double layoutLength)
        {
            string description = $"{Length} {SelectedUnits}";
            if (ExtraLap)
            {
                description += ", plus one lap";
            }

            return description;
        }

        public string[] AvailableUnits { get; }

        public int Length
        {
            get => _length;
            set => SetProperty(ref _length, value);
        }

        public bool ExtraLap
        {
            get => _extraLap;
            set => SetProperty(ref _extraLap, value);
        }

        public string SelectedUnits
        {
            get => _selectedUnits;
            set => SetProperty(ref _selectedUnits, value);
        }
    }
}