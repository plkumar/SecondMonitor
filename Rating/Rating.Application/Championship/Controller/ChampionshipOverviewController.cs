namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.Events;
    using Contracts.Commands;
    using Operations;
    using Pool;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Controllers;
    using SecondMonitor.ViewModels.Factory;
    using ViewModels.Events;
    using ViewModels.Overview;

    public class ChampionshipOverviewController : AbstractChildController<IChampionshipController>, IChampionshipOverviewController
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IChildControllerFactory _childControllerFactory;
        private readonly IChampionshipsPool _championshipsPool;
        private readonly IWindowService _windowService;
        private readonly IDialogService _dialogService;
        private readonly IChampionshipManipulator _championshipManipulator;
        private IChampionshipCreationController _championshipCreationController;
        private Window _overviewWindow;
        private ChampionshipsOverviewViewModel _championshipOverviewViewModel;

        public ChampionshipOverviewController(IViewModelFactory viewModelFactory, IChildControllerFactory childControllerFactory, IChampionshipsPool championshipsPool, IWindowService windowService, IDialogService dialogService, IChampionshipManipulator championshipManipulator)
        {
            _viewModelFactory = viewModelFactory;
            _childControllerFactory = childControllerFactory;
            _championshipsPool = championshipsPool;
            _windowService = windowService;
            _dialogService = dialogService;
            _championshipManipulator = championshipManipulator;
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
            var sessionCompletedViewmodel = _viewModelFactory.Create<SessionCompletedViewModel>();
            (EventDto eventDto, SessionDto sessionDto) = championship.GetLastSessionWithResults();
            var lastResult = sessionDto.SessionResult;
            if (lastResult == null)
            {
                return;
            }

            sessionCompletedViewmodel.Title = "Session Completed";

            var podiumViewModel = _viewModelFactory.Create<PodiumViewModel>();
            podiumViewModel.FromModel(lastResult);

            var driversFinishViewModel = _viewModelFactory.Create<DriversFinishViewModel>();
            driversFinishViewModel.Header = "Session Results";
            driversFinishViewModel.FromModel(lastResult);

            var driversNewStandingsViewModel = _viewModelFactory.Create<DriversNewStandingsViewModel>();

            driversNewStandingsViewModel.ChampionshipName = championship.ChampionshipName;
            driversNewStandingsViewModel.EventName = eventDto.EventName;
            driversNewStandingsViewModel.EventIndex = $"({championship.Events.IndexOf(eventDto) + 1} / {championship.TotalEvents})";
            driversNewStandingsViewModel.SessionName = sessionDto.Name;
            driversNewStandingsViewModel.SessionIndex = $"({eventDto.Sessions.IndexOf(sessionDto) + 1} / {eventDto.Sessions.Count})";

            driversNewStandingsViewModel.FromModel(lastResult);

            sessionCompletedViewmodel.Screens.Add(podiumViewModel);
            sessionCompletedViewmodel.Screens.Add(driversFinishViewModel);
            sessionCompletedViewmodel.Screens.Add(driversNewStandingsViewModel);

            Window window = _windowService.OpenWindow(sessionCompletedViewmodel, "Session Completed", WindowState.Maximized, SizeToContent.Manual, WindowStartupLocation.CenterOwner);
            sessionCompletedViewmodel.CloseCommand = new RelayCommand(() => CloseSessionCompletedWindow(window));
        }

        private void CloseSessionCompletedWindow(Window window)
        {
            window.Close();
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