namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Session.SessionLength
{
    using DataModel.BasicProperties;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Settings;

    public class DistanceLengthDefinitionViewModel : AbstractViewModel, ISessionLengthDefinitionViewModel
    {
        private int _length;

        public DistanceLengthDefinitionViewModel(ISettingsProvider settingsProvider)
        {
            Length = 100;
            Units = Distance.GetUnitsSymbol(settingsProvider.DisplaySettingsViewModel.DistanceUnits);
        }

        public string LengthKind => "Distance";

        public int Length
        {
            get => _length;
            set => SetProperty(ref _length, value);
        }

        public string Units { get; }
    }
}