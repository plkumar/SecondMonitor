namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Session.SessionLength
{
    using System;
    using DataModel.BasicProperties;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Settings;

    public class DistanceLengthDefinitionViewModel : AbstractViewModel, ISessionLengthDefinitionViewModel
    {
        private readonly ISettingsProvider _settingsProvider;
        private int _length;

        public DistanceLengthDefinitionViewModel(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
            Length = 100;
            Units = Distance.GetUnitsSymbol(settingsProvider.DisplaySettingsViewModel.DistanceUnits);
        }

        public string LengthKind => "Distance";

        public string GetTextualDescription(double layoutLength)
        {
            if (layoutLength == 0)
            {
                return $"{Length}{Units}";
            }

            double distanceInMeters = Distance.CreateByUnits(Length, _settingsProvider.DisplaySettingsViewModel.DistanceUnits).InMeters;
            int lapsRequired = (int)Math.Round(distanceInMeters / layoutLength);
            return $"{lapsRequired} Laps";
        }

        public int Length
        {
            get => _length;
            set => SetProperty(ref _length, value);
        }

        public string Units { get; }
    }
}