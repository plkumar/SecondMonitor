namespace SecondMonitor.ViewModels.WheelDiameterWizard
{
    public class PreparationStageViewModel : AbstractViewModel
    {
        private bool _isThrottleReleased;
        private bool _isBrakeReleased;
        private bool _isInHighGear;
        private bool _isClutchDepressed;

        public bool IsThrottleReleased
        {
            get => _isThrottleReleased;
            set => SetProperty(ref _isThrottleReleased, value);
        }

        public bool IsBrakeReleased
        {
            get => _isBrakeReleased;
            set => SetProperty(ref _isBrakeReleased, value);
        }

        public bool IsInHighGear
        {
            get => _isInHighGear;
            set => SetProperty(ref _isInHighGear, value);
        }

        public bool IsClutchDepressed
        {
            get => _isClutchDepressed;
            set => SetProperty(ref _isClutchDepressed, value);
        }
    }
}