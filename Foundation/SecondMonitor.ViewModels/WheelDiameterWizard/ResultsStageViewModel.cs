namespace SecondMonitor.ViewModels.WheelDiameterWizard
{
    using System.Windows.Input;

    public class ResultsStageViewModel : AbstractViewModel
    {
        private ICommand _okCommand;
        private ICommand _cancelCommand;
        private ICommand _restartCommand;
        private string _distanceUnits;
        private double _frontWheelDiameter;
        private double _rearWheelDiameter;

        public ICommand OkCommand
        {
            get => _okCommand;
            set => SetProperty(ref _okCommand, value);
        }

        public ICommand CancelCommand
        {
            get => _cancelCommand;
            set => SetProperty(ref _cancelCommand, value);
        }

        public ICommand RestartCommand
        {
            get => _restartCommand;
            set => SetProperty(ref _restartCommand, value);
        }

        public string DistanceUnits
        {
            get => _distanceUnits;
            set => SetProperty(ref _distanceUnits, value);
        }

        public double FrontWheelDiameter
        {
            get => _frontWheelDiameter;
            set => SetProperty(ref _frontWheelDiameter, value);
        }

        public double RearWheelDiameter
        {
            get => _rearWheelDiameter;
            set => SetProperty(ref _rearWheelDiameter, value);
        }
    }
}