namespace SecondMonitor.ViewModels.WheelDiameterWizard
{
    public class AccelerationStageViewModel : AbstractViewModel
    {
        private string _velocityUnits;
        private double _targetSpeed;
        private double _currentSpeed;

        public string VelocityUnits
        {
            get => _velocityUnits;
            set => SetProperty(ref _velocityUnits, value);
        }

        public double TargetSpeed
        {
            get => _targetSpeed;
            set => SetProperty(ref _targetSpeed, value);
        }

        public double CurrentSpeed
        {
            get => _currentSpeed;
            set => SetProperty(ref _currentSpeed, value);
        }
    }
}