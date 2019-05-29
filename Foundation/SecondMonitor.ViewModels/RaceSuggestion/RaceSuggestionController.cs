namespace SecondMonitor.ViewModels.RaceSuggestion
{
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using DataModel.SimulatorContent;
    using DataModel.Snapshot;
    using SimulatorContent;

    public class RaceSuggestionController : IRaceSuggestionController
    {
        private readonly ISimulatorContentRepository _simulatorContentRepository;
        private SimulatorsContent _simulatorsContent;
        private SimulatorContent _selectedSimulatorContent;
        private string _selectedSimulator;

        public RaceSuggestionController(IRaceSuggestionViewModel raceSuggestionViewModel, ISimulatorContentRepository simulatorContentRepository )
        {
            _simulatorContentRepository = simulatorContentRepository;
            RaceSuggestionViewModel = raceSuggestionViewModel;
            RaceSuggestionViewModel.RandomizeSimulator = false;
        }

        public void Visit(SimulatorDataSet simulatorDataSet)
        {

        }

        public void Reset()
        {

        }

        public IRaceSuggestionViewModel RaceSuggestionViewModel { get; }

        public Task StartControllerAsync()
        {
            _simulatorsContent = _simulatorContentRepository.LoadOrCreateSimulatorsContent();
            RaceSuggestionViewModel.PropertyChanged += RaceSuggestionViewModelOnPropertyChanged;
            return Task.CompletedTask;
        }

        private void RaceSuggestionViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IRaceSuggestionViewModel.SelectedSimulator))
            {
                return;
            }

            _selectedSimulator = RaceSuggestionViewModel.SelectedSimulator;
            FillSimulatorContent();
        }

        private void FillSimulatorContent()
        {
            _selectedSimulatorContent = _simulatorsContent.SimulatorContents.FirstOrDefault(x => x.SimulatorName == _selectedSimulator);

            if (_selectedSimulatorContent == null)
            {
                return;
            }

            RaceSuggestionViewModel.AvailableTracks = _selectedSimulatorContent.Tracks.Select(x => x.Name).OrderBy(x => x);
            RaceSuggestionViewModel.AvailableClasses = _selectedSimulatorContent.Classes.Select(x => x.ClassName).OrderBy(x => x);

        }

        public Task StopControllerAsync()
        {
            RaceSuggestionViewModel.PropertyChanged -= RaceSuggestionViewModelOnPropertyChanged;
            return  Task.CompletedTask;
        }
    }
}