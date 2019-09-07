namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Threading.Tasks;
    using System.Windows;
    using Common.DataModel.Championship;
    using Contracts.Commands;
    using Pool;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Controllers;
    using SecondMonitor.ViewModels.Factory;
    using ViewModels;

    public class ChampionshipOverviewController : IChampionshipOverviewController
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IChildControllerFactory _childControllerFactory;
        private readonly IChampionshipsPool _championshipsPool;
        private readonly IWindowService _windowService;
        private IChampionshipCreationController _championshipCreationController;
        private Window _overviewWindow;

        public ChampionshipOverviewController(IViewModelFactory viewModelFactory, IChildControllerFactory childControllerFactory, IChampionshipsPool championshipsPool, IWindowService windowService)
        {
            _viewModelFactory = viewModelFactory;
            _childControllerFactory = childControllerFactory;
            _championshipsPool = championshipsPool;
            _windowService = windowService;
        }

        public Task StartControllerAsync()
        {
            return Task.CompletedTask;
        }

        public Task StopControllerAsync()
        {
            return Task.CompletedTask;
        }

        public void OpenChampionshipOverviewWindow()
        {
            if (_overviewWindow != null)
            {
                _overviewWindow.Focus();
                return;
            }

            var overviewViewModel = _viewModelFactory.Create<ChampionshipsOverviewViewModel>();
            overviewViewModel.CreateNewCommand = new AsyncCommand(CreateNewChampionship);
            overviewViewModel.FromModel(_championshipsPool.GetAllChampionshipDtos());
            _overviewWindow = _windowService.OpenWindow(overviewViewModel, "All Championships", WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner, WindowClosed);
        }

        private async Task CreateNewChampionship()
        {
            if (_championshipCreationController != null)
            {
                _championshipCreationController.TryFocusCreationWindow();
                return;
            }

            _championshipCreationController = _childControllerFactory.Create<IChampionshipCreationController>();
            await _championshipCreationController.StartControllerAsync();
            _championshipCreationController.OpenChampionshipCreationDialog(NewChampionshipCreated, ChampionshipCreationCancelled);
        }

        private void ChampionshipCreationCancelled()
        {
            _championshipCreationController.StopControllerAsync();
            _championshipCreationController = null;
        }

        private void NewChampionshipCreated(ChampionshipDto newChampionshipDto)
        {
            _championshipsPool.AddNewChampionship(newChampionshipDto);
            _championshipCreationController.StopControllerAsync();
            _championshipCreationController = null;
        }

        private async void WindowClosed()
        {
            if (_overviewWindow.Content is ChampionshipsOverviewViewModel championshipsOverviewViewModel)
            {
                championshipsOverviewViewModel.CreateNewCommand = null;
                championshipsOverviewViewModel.OpenSelectedCommand = null;
                championshipsOverviewViewModel.DeleteSelectedCommand = null;
            }

            if (_championshipCreationController != null)
            {
                await _championshipCreationController.StopControllerAsync();
            }

            _overviewWindow = null;
        }
    }
}