namespace SecondMonitor.ViewModels.WheelDiameterWizard
{
    public class MeasurementPhaseViewModel : AbstractViewModel
    {
        private string _timeLeftFormatted;

        public string TimeLeftFormatted
        {
            get => _timeLeftFormatted;
            set => SetProperty(ref _timeLeftFormatted, value);
        }
    }
}