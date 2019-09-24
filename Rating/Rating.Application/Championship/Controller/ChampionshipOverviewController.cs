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
        private ChampionshipsOverviewViewModel _championshipOverviewViewModel;

        public ChampionshipOverviewController(IViewModelFactory viewModelFactory, IChildControllerFactory childControllerFactory, IChampionshipsPool championshipsPool, IWindowService windowService)
        {
            _viewModelFactory = viewModelFactory;
            _childControllerFactory = childControllerFactory;
            _championshipsPool = championshipsPool;
            _windowService = windowService;
        }

        public Task StartControllerAsync()
        {
            _championshipsPool.ChampionshipAdded += ChampionshipsPoolOnChampionshipAdded;
            return Task.CompletedTask;
        }

        private void ChampionshipsPoolOnChampionshipAdded(object sender, ChampionshipEventArgs e)
        {
            _championshipOverviewViewModel?.InsertChampionshipFirst(e.ChampionshipDto);
        }

        public Task StopControllerAsync()
        {
            _championshipsPool.ChampionshipAdded -= ChampionshipsPoolOnChampionshipAdded;
            return Task.CompletedTask;
        }

        public void OpenChampionshipOverviewWindow()
        {
            if (_overviewWindow != null)
            {
                _overviewWindow.Focus();
                return;
            }

            _championshipOverviewViewModel = _viewModelFactory.Create<ChampionshipsOverviewViewModel>();
            _championshipOverviewViewModel.CreateNewCommand = new AsyncCommand(CreateNewChampionship);
            _championshipOverviewViewModel.FromModel(_championshipsPool.GetAllChampionshipDtos());
            _overviewWindow = _windowService.OpenWindow(_championshipOverviewViewModel, "All Championships", WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner, WindowClosed);
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
           _championshipOverviewViewModel.CreateNewCommand = null;
           _championshipOverviewViewModel.OpenSelectedCommand = null;
           _championshipOverviewViewModel.DeleteSelectedCommand = null;

           _championshipOverviewViewModel = null;

            if (_championshipCreationController != null)
            {
                await _championshipCreationController.StopControllerAsync();
            }

            _overviewWindow = null;
        }
    }
}