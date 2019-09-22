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
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.SimulatorContent;
    using SimdataManagement;
    using ViewModels.Creation;
    using ViewModels.Creation.Calendar;
    using ViewModels.Creation.Calendar.Predefined;

    public class ChampionshipCreationController : IChampionshipCreationController
    {
        private readonly IWindowService _windowService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ISimulatorContentController _simulatorContentController;
        private readonly ITrackTemplateToSimTrackMapper _trackTemplateToSimTrackMapper;
        private readonly MapsLoader _mapsLoader;
        private Window _dialogWindow;
        private Action<ChampionshipDto> _newChampionshipCallback;
        private Action _cancellationCallback;
        private bool _championshipCreated;
        private ChampionshipCreationViewModel _championshipCreationViewModel;
        private string _selectedSimulator;

        public ChampionshipCreationController(IWindowService windowService, IViewModelFactory viewModelFactory, ISimulatorContentController simulatorContentController, IMapsLoaderFactory mapsLoaderFactory, ITrackTemplateToSimTrackMapper trackTemplateToSimTrackMapper )
        {
            _windowService = windowService;
            _viewModelFactory = viewModelFactory;
            _simulatorContentController = simulatorContentController;
            _trackTemplateToSimTrackMapper = trackTemplateToSimTrackMapper;
            _mapsLoader = mapsLoaderFactory.Create();
        }

        public Task StartControllerAsync()
        {
            return Task.CompletedTask;
        }

        public Task StopControllerAsync()
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

            _dialogWindow = _windowService.OpenWindow(_championshipCreationViewModel, "New Championship", WindowState.Maximized, SizeToContent.Manual, WindowStartupLocation.CenterOwner, DialogWindowClosed);
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
            _windowService.OpenDialog(calendarSelection, "Select Predefined Calendar", WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner);
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
            foreach (Track currentTrack in allTracks)
            {
                var newViewModel = _viewModelFactory.Create<ExistingTrackTemplateViewModel>();
                newViewModel.TrackName = currentTrack.Name;
                newViewModel.LayoutLengthMeters = currentTrack.LapDistance;
                bool hasMap = _mapsLoader.TryLoadMap(_selectedSimulator, currentTrack.Name, out TrackMapDto trackMapDto);
                if (hasMap)
                {
                    newViewModel.TrackGeometryViewModel.FromModel(trackMapDto.TrackGeometry);
                }
                tracksTemplates.Add(newViewModel);
            }

            _championshipCreationViewModel.CalendarDefinitionViewModel.AvailableTracksViewModel.TrackTemplateViewModels = tracksTemplates;
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