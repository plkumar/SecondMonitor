namespace SecondMonitor.ViewModels.SplashScreen
{
    public class SplashScreenViewModel : AbstractViewModel
    {
        private string _primaryInformation;
        private string _secondaryInformation;

        public string PrimaryInformation
        {
            get => _primaryInformation;
            set => SetProperty(ref _primaryInformation, value);
        }

        public string SecondaryInformation
        {
            get => _secondaryInformation;
            set => SetProperty(ref _secondaryInformation, value);
        }
    }
}