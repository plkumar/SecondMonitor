namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Threading.Tasks;
    using System.Windows;
    using Common.DataModel.Championship;
    using Contracts.Commands;
    using Operations;
    using Pool;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Controllers;
    using SecondMonitor.ViewModels.Factory;
    using ViewModels.Overview;

    public class ChampionshipOverviewController : AbstractChildController<IChampionshipController>, IChampionshipOverviewController
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IChildControllerFactory _childControllerFactory;
        private readonly IChampionshipsPool _championshipsPool;
        private readonly IWindowService _windowService;
        private readonly IDialogService _dialogService;
        private readonly IChampionshipManipulator _championshipManipulator;
        private readonly IChampionshipDialogProvider _championshipDialogProvider;
        private IChampionshipCreationController _championshipCreationController;
        private Window _overviewWindow;
        private ChampionshipsOverviewViewModel _championshipOverviewViewModel;

        public ChampionshipOverviewController(IViewModelFactory viewModelFactory, IChildControllerFactory childControllerFactory, IChampionshipsPool championshipsPool, IWindowService windowService, IDialogService dialogService, IChampionshipManipulator championshipManipulator, IChampionshipDialogProvider championshipDialogProvider)
        {
            _viewModelFactory = viewModelFactory;
            _childControllerFactory = childControllerFactory;
            _championshipsPool = championshipsPool;
            _windowService = windowService;
            _dialogService = dialogService;
            _championshipManipulator = championshipManipulator;
            _championshipDialogProvider = championshipDialogProvider;
        }

        public override Task StartControllerAsync()
        {
            _championshipsPool.ChampionshipAdded += ChampionshipsPoolOnChampionshipAdded;
            _championshipsPool.ChampionshipRemoved += ChampionshipsPoolOnChampionshipRemoved;
            _championshipsPool.ChampionshipUpdated += ChampionshipsPoolOnChampionshipUpdated;
            return Task.CompletedTask;
        }

        public override Task StopControllerAsync()
        {
            _championshipsPool.ChampionshipAdded -= ChampionshipsPoolOnChampionshipAdded;
            _championshipsPool.ChampionshipRemoved -= ChampionshipsPoolOnChampionshipRemoved;
            _championshipsPool.ChampionshipUpdated -= ChampionshipsPoolOnChampionshipUpdated;
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
            _championshipOverviewViewModel.RemoveSelectedCommand = new RelayCommand(RemoveSelectedChampionship);
            _championshipOverviewViewModel.NextRaceOverviewViewModel.DnfSessionCommand = new RelayCommand(DnfSelectedChampionshipSession);
            _championshipOverviewViewModel.OpenSelectedCommand = new RelayCommand(OpenSelectedChampionship);
            _championshipOverviewViewModel.FromModel(_championshipsPool.GetAllChampionshipDtos());
            _overviewWindow = _windowService.OpenWindow(_championshipOverviewViewModel, "All Championships", WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner, WindowClosed);
        }

        private void OpenSelectedChampionship()
        {
            if (_championshipOverviewViewModel.SelectedChampionship == null)
            {
                return;
            }

            OpenChampionshipDetailsWindow(_championshipOverviewViewModel.SelectedChampionship.OriginalModel);
        }

        public void OpenChampionshipDetailsWindow(ChampionshipDto championship)
        {
            var detailViewModel = _viewModelFactory.Create<ChampionshipDetailViewModel>();
            detailViewModel.FromModel(championship);
            _windowService.OpenWindow(detailViewModel, "Championships Details", WindowState.Maximized, SizeToContent.Manual, WindowStartupLocation.CenterOwner);
        }

        private void RemoveSelectedChampionship()
        {
            if (_championshipOverviewViewModel.SelectedChampionship == null)
            {
                return;
            }

            if (!_dialogService.ShowYesNoDialog("Confirmation", "Remove Selected Championship?"))
            {
                return;
            }

            _championshipsPool.RemoveChampionship(_championshipOverviewViewModel.SelectedChampionship.OriginalModel);
        }

        private void ChampionshipsPoolOnChampionshipRemoved(object sender, ChampionshipEventArgs e)
        {
            _championshipOverviewViewModel.RemoveChampionship(e.ChampionshipDto);
        }

        private void ChampionshipsPoolOnChampionshipAdded(object sender, ChampionshipEventArgs e)
        {
            _championshipOverviewViewModel?.InsertChampionshipFirst(e.ChampionshipDto);
        }

        private void ChampionshipsPoolOnChampionshipUpdated(object sender, ChampionshipEventArgs e)
        {
            _championshipOverviewViewModel?.RemoveChampionship(e.ChampionshipDto);
            _championshipOverviewViewModel?.InsertChampionshipFirst(e.ChampionshipDto);
        }

        private void DnfSelectedChampionshipSession()
        {
            if (_championshipOverviewViewModel.SelectedChampionship == null)
            {
                return;
            }

            ChampionshipDto selectedChampionship = _championshipOverviewViewModel.SelectedChampionship.OriginalModel;

            _championshipManipulator.CommitLastSessionResults(selectedChampionship);
            _championshipsPool.UpdateChampionship(selectedChampionship);
            ShowLastEvenResultWindow(selectedChampionship);
        }

        private void ShowLastEvenResultWindow(ChampionshipDto championship)
        {
            _championshipDialogProvider.ShowLastEvenResultWindow(championship);
        }

        private async Task CreateNewChampionship()
        {
            if (_championshipCreationController != null)
            {
                _championshipCreationController.TryFocusCreationWindow();
                return;
            }

            _championshipCreationController = _childControllerFactory.Create<IChampionshipCreationController, IChampionshipOverviewController>(this);
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