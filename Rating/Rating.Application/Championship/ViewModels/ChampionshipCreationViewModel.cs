namespace SecondMonitor.Rating.Application.Championship.ViewModels
{
    using System.Windows.Input;
    using SecondMonitor.ViewModels;

    public class ChampionshipCreationViewModel : AbstractViewModel
    {
        private bool _isSimulatorSelectionEnabled;

        public bool IsSimulatorSelectionEnabled
        {
            get => _isSimulatorSelectionEnabled;
            set => SetProperty(ref _isSimulatorSelectionEnabled, value);
        }

        public string[] AvailableSimulators { get; set; }

        public string SelectedSimulator { get; set; }

        public ICommand ConfirmSimulatorCommand { get; set; }
    }
}