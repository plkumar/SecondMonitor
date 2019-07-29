namespace SecondMonitor.ViewModels.Settings.ViewModel
{
    using Model;

    public class TrackRecordsSettingsViewModel : AbstractViewModel<TrackRecordsSettings>
    {
        private bool _isEnabled;
        private bool _displayRecordForCurrentSessionType;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }


        public bool DisplayRecordForCurrentSessionType
        {
            get => _displayRecordForCurrentSessionType;
            set => SetProperty(ref _displayRecordForCurrentSessionType, value);
        }

        protected override void ApplyModel(TrackRecordsSettings model)
        {
            IsEnabled = model.IsEnabled;
            DisplayRecordForCurrentSessionType = model.DisplayRecordForCurrentSessionType;
        }

        public override TrackRecordsSettings SaveToNewModel()
        {
            return new TrackRecordsSettings()
            {
                DisplayRecordForCurrentSessionType = DisplayRecordForCurrentSessionType,
                IsEnabled = IsEnabled
            };
        }
    }
}
