namespace SecondMonitor.ViewModels.Settings.ViewModel
{
    using Model;

    public class RatingSettingsViewModel : AbstractViewModel<RatingSettings>
    {
        private bool _isEnabled;
        private string _selectedReferenceRatingProvider;
        private string[] _availableReferenceRatingProviders;
        private int _graceLapsCount;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public int GraceLapsCount
        {
            get => _graceLapsCount;
            set => SetProperty(ref _graceLapsCount, value);
        }

        public string SelectedReferenceRatingProvider
        {
            get => _selectedReferenceRatingProvider;
            set => SetProperty(ref _selectedReferenceRatingProvider, value);
        }

        public string[] AvailableReferenceRatingProviders
        {
            get => _availableReferenceRatingProviders;
            set => SetProperty(ref _availableReferenceRatingProviders, value);
        }

        protected override void ApplyModel(RatingSettings model)
        {
            IsEnabled = model.IsEnabled;
            SelectedReferenceRatingProvider = model.SelectedReferenceRatingProvider;
            GraceLapsCount = model.GraceLapsCount;
        }

        public override RatingSettings SaveToNewModel()
        {
            return new RatingSettings()
            {
                IsEnabled = IsEnabled,
                SelectedReferenceRatingProvider = SelectedReferenceRatingProvider,
                GraceLapsCount = GraceLapsCount
            };
        }
    }
}