namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using Common.DataModel.Championship;
    using Contracts.Commands;
    using DataModel.SimulatorContent;
    using DataModel.TrackMap;
    using Rating.Controller.SimulatorRating;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.SimulatorContent;
    using SimdataManagement;
    using ViewModels.Creation;

    public class ChampionshipCreationController : IChampionshipCreationController
    {
        private readonly IWindowService _windowService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ISimulatorContentController _simulatorContentController;
        private readonly MapsLoader _mapsLoader;
        private Window _dialogWindow;
        private Action<ChampionshipDto> _newChampionshipCallback;
        private Action _cancellationCallback;
        private bool _championshipCreated;
        private ChampionshipCreationViewModel _championshipCreationViewModel;
        private string _selectedSimulator;

        public ChampionshipCreationController(IWindowService windowService, IViewModelFactory viewModelFactory, ISimulatorContentController simulatorContentController, IMapsLoaderFactory mapsLoaderFactory)
        {
            _windowService = windowService;
            _viewModelFactory = viewModelFactory;
            _simulatorContentController = simulatorContentController;
            _mapsLoader = mapsLoaderFactory.Create();
        }

        public Task StartControllerAsync()
        {
            return Task.CompletedTask;
        }

        public Task StopControllerAsync()
        {
            _dialogWindow?.Close();
            return Task.CompletedTask;
        }

        public void TryFocusCreationWindow()
        {
            _dialogWindow?.Focus();
        }

        public void OpenChampionshipCreationDialog(Action<ChampionshipDto> newChampionshipCallback, Action cancellationCallback)
        {
            _newChampionshipCallback = newChampionshipCallback;
            _cancellationCallback = cancellationCallback;
            _championshipCreationViewModel = _viewModelFactory.Create<ChampionshipCreationViewModel>();
            _championshipCreationViewModel.IsSimulatorSelectionEnabled = true;
            _championshipCreationViewModel.AvailableSimulators = SimulatorRatingControllerFactory.SupportedSimulators;
            _championshipCreationViewModel.ConfirmSimulatorCommand = new RelayCommand(ConfirmSimulatorSelection);

            _dialogWindow = _windowService.OpenWindow(_championshipCreationViewModel, "New Championship", WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner, DialogWindowClosed);
        }

        private void ConfirmSimulatorSelection()
        {
            if (string.IsNullOrWhiteSpace(_championshipCreationViewModel.SelectedSimulator))
            {
                return;
            }
            _championshipCreationViewModel.IsSimulatorSelectionEnabled = false;
            _selectedSimulator = _championshipCreationViewModel.SelectedSimulator;

            var allTracks = _simulatorContentController.GetAllTracksForSimulator(_selectedSimulator);
            foreach (Track currentTrack in allTracks)
            {
                bool hasMap = _mapsLoader.TryLoadMap(_selectedSimulator, currentTrack.Name, out TrackMapDto trackMapDto);
            }
        }

        private void DialogWindowClosed()
        {
            _dialogWindow = null;
            if (!_championshipCreated)
            {
                _cancellationCallback();
            }
        }
    }
}