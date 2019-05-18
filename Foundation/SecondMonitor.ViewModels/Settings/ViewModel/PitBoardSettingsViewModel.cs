namespace SecondMonitor.ViewModels.Settings.ViewModel
{
    using Model;

    public class PitBoardSettingsViewModel : AbstractViewModel<PitBoardSettings>
    {
        private bool _isEnabled;
        private int _displaySeconds;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public int DisplaySeconds
        {
            get => _displaySeconds;
            set => SetProperty(ref _displaySeconds, value);
        }

        protected override void ApplyModel(PitBoardSettings model)
        {
            IsEnabled = model.IsEnabled;
            DisplaySeconds = model.DisplaySeconds;
        }

        public override PitBoardSettings SaveToNewModel()
        {
            return new PitBoardSettings()
            {
                DisplaySeconds = DisplaySeconds,
                IsEnabled = IsEnabled,
            };
        }
    }
}