namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Session.SessionLength
{
    using SecondMonitor.ViewModels;

    public class LapsLengthDefinitionViewModel : AbstractViewModel, ISessionLengthDefinitionViewModel
    {
        private int _laps;

        public LapsLengthDefinitionViewModel()
        {
            Laps = 10;
        }

        public string LengthKind => "Laps";
        public string GetTextualDescription(double layoutLength)
        {
            return $"{Laps} Laps";
        }

        public int Laps
        {
            get => _laps;
            set => SetProperty(ref _laps, value);
        }
    }
}