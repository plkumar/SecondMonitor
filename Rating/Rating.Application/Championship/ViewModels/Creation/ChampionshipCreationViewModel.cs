namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation
{
    using System.Windows.Input;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class ChampionshipCreationViewModel : AbstractViewModel
    {
        private bool _isSimulatorSelectionEnabled;

        public ChampionshipCreationViewModel(IViewModelFactory viewModelFactory)
        {
            CalendarDefinitionViewModel = viewModelFactory.Create<CalendarDefinitionViewModel>();
        }

        public bool IsSimulatorSelectionEnabled
        {
            get => _isSimulatorSelectionEnabled;
            set => SetProperty(ref _isSimulatorSelectionEnabled, value);
        }

        public CalendarDefinitionViewModel CalendarDefinitionViewModel { get;}

        public string[] AvailableSimulators { get; set; }

        public string SelectedSimulator { get; set; }

        public ICommand ConfirmSimulatorCommand { get; set; }
    }
}