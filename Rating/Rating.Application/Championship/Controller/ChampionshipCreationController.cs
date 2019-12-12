namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Common.Championship.Calendar.Templates.CalendarGroups;
    using Common.DataModel.Championship;
    using Contracts.Commands;
    using DataModel.SimulatorContent;
    using DataModel.TrackMap;
    using Rating.Controller.SimulatorRating;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Controllers;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.SimulatorContent;
    using SimdataManagement;
    using ViewModels.Creation;
    using ViewModels.Creation.Calendar;
    using ViewModels.Creation.Calendar.Predefined;

    public class ChampionshipCreationController : AbstractChildController<IChampionshipOverviewController>, IChampionshipCreationController
    {
        private readonly IWindowService _windowService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ISimulatorContentController _simulatorContentController;
        private readonly ITrackTemplateToSimTrackMapper _trackTemplateToSimTrackMapper;
        private readonly IChampionshipFactory _championshipFactory;
        private readonly MapsLoader _mapsLoader;
        private Window _dialogWindow;
        private Action<ChampionshipDto> _newChampionshipCallback;
        private Action _cancellationCallback;
        private bool _championshipCreated;
        private ChampionshipCreationViewModel _championshipCreationViewModel;
        private string _selectedSimulator;

        public ChampionshipCreationController(IWindowService windowService, IViewModelFactory viewModelFactory, ISimulatorContentController simulatorContentController, IMapsLoaderFactory mapsLoaderFactory, ITrackTemplateToSimTrackMapper trackTemplateToSimTrackMapper,
             IChampionshipFactory championshipFactory)
        {
            _windowService = windowService;
            _viewModelFactory = viewModelFactory;
            _simulatorContentController = simulatorContentController;
            _trackTemplateToSimTrackMapper = trackTemplateToSimTrackMapper;
            _championshipFactory = championshipFactory;
            _mapsLoader = mapsLoaderFactory.Create();
        }

        public override Task StartControllerAsync()
        {
            return Task.CompletedTask;
        }

        public override Task StopControllerAsync()
        {
            _dialogWindow?.Close();
            _trackTemplateToSimTrackMapper.SaveTrackMappings();
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
            _championshipCreationViewModel.CalendarDefinitionViewModel.CalendarViewModel.SelectPredefinedCalendarCommand = new RelayCommand(SelectPredefinedCalendar);
            _championshipCreationViewModel.CalendarDefinitionViewModel.CalendarViewModel.RandomCalendarCommand = new RelayCommand(CreateRandomCalendar);
            _championshipCreationViewModel.OkCommand = new RelayCommand(CreateNewChampionship);
            _championshipCreationViewModel.CancelCommand = new RelayCommand(CancelChampionshipCreation);

            _dialogWindow = _windowService.OpenWindow(_championshipCreationViewModel, "New Championship", WindowState.Maximized, SizeToContent.Manual, WindowStartupLocation.CenterOwner, DialogWindowClosed);
        }

        private void CancelChampionshipCreation()
        {
            _dialogWindow.Close();
        }

        private void CreateNewChampionship()
        {
            _championshipCreated = true;
            ChampionshipDto newChampionshipDto = _championshipFactory.Create(_championshipCreationViewModel);
            _newChampionshipCallback(newChampionshipDto);
            //_dialogWindow.Close();
        }

        private void CreateRandomCalendar()
        {
            Random random = new Random();
            int randomEvents = _championshipCreationViewModel.CalendarDefinitionViewModel.CalendarViewModel.RandomEventsCount;
            List<ExistingTrackTemplateViewModel> tracksToChooseFrom =  _championshipCreationViewModel.CalendarDefinitionViewModel.AvailableTracksViewModel.TrackTemplateViewModels.OfType<ExistingTrackTemplateViewModel>().ToList();
            if (tracksToChooseFrom.Count == 0)
            {
                return;
            }

            _championshipCreationViewModel.CalendarDefinitionViewModel.CalendarViewModel.ClearCalendar();

            for (int i = 0; i < randomEvents; i++)
            {
                int newTrackIndex = random.Next(tracksToChooseFrom.Count - 1);
                _championshipCreationViewModel.CalendarDefinitionViewModel.CalendarViewModel.AppendNewEntry(tracksToChooseFrom[newTrackIndex]);
                tracksToChooseFrom.RemoveAt(newTrackIndex);
                if (tracksToChooseFrom.Count == 0)
                {
                    tracksToChooseFrom = _championshipCreationViewModel.CalendarDefinitionViewModel.AvailableTracksViewModel.TrackTemplateViewModels.OfType<ExistingTrackTemplateViewModel>().ToList();
                }
            }
        }

        private void SelectPredefinedCalendar()
        {
            var calendarSelection = _viewModelFactory.Create<PredefinedCalendarSelectionViewModel>();
            calendarSelection.FromModel(AllGroups.MainGroup);
            _windowService.OpenDialog(calendarSelection, "Select Predefined Calendar", WindowState.Maximized, SizeToContent.Manual, WindowStartupLocation.CenterOwner);
            if (calendarSelection.DialogResult == true && calendarSelection.SelectedItem is CalendarTemplateViewModel selectedViewModel)
            {
                if (calendarSelection.UseEventNames)
                {
                    _championshipCreationViewModel.ChampionshipTitle = selectedViewModel.Title;
                }
                _championshipCreationViewModel.CalendarDefinitionViewModel.CalendarViewModel.ApplyCalendarTemplate(selectedViewModel.OriginalModel, calendarSelection.UseEventNames, calendarSelection.AutoReplaceTracks);
            }
        }

        private void ConfirmSimulatorSelection()
        {
            if (string.IsNullOrWhiteSpace(_championshipCreationViewModel.SelectedSimulator))
            {
                return;
            }
            _championshipCreationViewModel.IsSimulatorSelectionEnabled = false;
            _selectedSimulator = _championshipCreationViewModel.SelectedSimulator;

            _championshipCreationViewModel.CalendarDefinitionViewModel.CalendarViewModel.SimulatorName = _selectedSimulator;

            var allTracks = _simulatorContentController.GetAllTracksForSimulator(_selectedSimulator).OrderBy(x => x.Name);
            List<AbstractTrackTemplateViewModel> tracksTemplates = new List<AbstractTrackTemplateViewModel> {_viewModelFactory.Create<GenericTrackTemplateViewModel>()};
            tracksTemplates[0].UseTemplateInCalendarCommand = new RelayCommand(UseTemplateInCalendar);
            foreach (Track currentTrack in allTracks)
            {
                var newViewModel = _viewModelFactory.Create<ExistingTrackTemplateViewModel>();
                newViewModel.TrackName = currentTrack.Name;
                newViewModel.LayoutLengthMeters = currentTrack.LapDistance;
                newViewModel.UseTemplateInCalendarCommand = new RelayCommand(UseTemplateInCalendar);
                bool hasMap = _mapsLoader.TryLoadMap(_selectedSimulator, currentTrack.Name, out TrackMapDto trackMapDto);
                if (hasMap)
                {
                    newViewModel.TrackGeometryViewModel.FromModel(trackMapDto.TrackGeometry);
                }
                tracksTemplates.Add(newViewModel);
            }

            _championshipCreationViewModel.CalendarDefinitionViewModel.AvailableTracksViewModel.TrackTemplateViewModels = tracksTemplates;
        }

        private void UseTemplateInCalendar()
        {
            _championshipCreationViewModel.CalendarDefinitionViewModel.CalendarViewModel.AppendNewEntry(_championshipCreationViewModel.CalendarDefinitionViewModel.AvailableTracksViewModel.SelectedTrackTemplateViewModel);
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