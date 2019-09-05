namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Threading.Tasks;
    using System.Windows;
    using Pool;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;
    using ViewModels;

    public class ChampionshipOverviewController : IChampionshipOverviewController
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IChampionshipsPool _championshipsPool;
        private readonly IWindowService _windowService;

        private Window _overviewWindow;

        public ChampionshipOverviewController(IViewModelFactory viewModelFactory, IChampionshipsPool championshipsPool, IWindowService windowService)
        {
            _viewModelFactory = viewModelFactory;
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
            overviewViewModel.FromModel(_championshipsPool.GetAllChampionshipDtos());
            _overviewWindow = _windowService.OpenWindow(overviewViewModel, "All Championships", WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner, WindowClosed);
        }

        private void WindowClosed()
        {
            if (_overviewWindow.Content is ChampionshipsOverviewViewModel championshipsOverviewViewModel)
            {
                championshipsOverviewViewModel.CreateNewCommand = null;
                championshipsOverviewViewModel.OpenSelectedCommand = null;
                championshipsOverviewViewModel.DeleteSelectedCommand = null;
            }

            _overviewWindow = null;
        }
    }
}