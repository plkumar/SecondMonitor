namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using Common.DataModel.Championship;
    using Rating.Controller.SimulatorRating;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;
    using ViewModels;

    public class ChampionshipCreationController : IChampionshipCreationController
    {
        private readonly IWindowService _windowService;
        private readonly IViewModelFactory _viewModelFactory;
        private Window _dialogWindow;
        private Action<ChampionshipDto> _newChampionshipCallback;
        private Action _cancellationCallback;
        private bool _championshipCreated;

        public ChampionshipCreationController(IWindowService windowService, IViewModelFactory viewModelFactory)
        {
            _windowService = windowService;
            _viewModelFactory = viewModelFactory;
        }

        public Task StartControllerAsync()
        {
            return Task.CompletedTask;
        }

        public Task StopControllerAsync()
        {
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
            var champCreationViewModel = _viewModelFactory.Create<ChampionshipCreationViewModel>();
            champCreationViewModel.IsSimulatorSelectionEnabled = true;
            champCreationViewModel.AvailableSimulators = SimulatorRatingControllerFactory.SupportedSimulators;

            _dialogWindow = _windowService.OpenWindow(champCreationViewModel, "New Championship", WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner, DialogWindowClosed);
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