namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using Calendar;
    using Calendar.Adorners;
    using Common.Championship.Calendar;
    using Contracts.Commands;
    using Controller;
    using GongSolutions.Wpf.DragDrop;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class CreatedCalendarViewModel : AbstractViewModel, IDropTarget
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ICalendarEntryViewModelFactory _calendarEntryViewModelFactory;
        private readonly ITrackTemplateToSimTrackMapper _trackTemplateToSimTrackMapper;
        private int _totalEvents;
        private ObservableCollection<AbstractCalendarEntryViewModel> _calendarEntries;

        public CreatedCalendarViewModel(IViewModelFactory viewModelFactory, ICalendarEntryViewModelFactory calendarEntryViewModelFactory, ITrackTemplateToSimTrackMapper trackTemplateToSimTrackMapper)
        {
            _viewModelFactory = viewModelFactory;
            _calendarEntryViewModelFactory = calendarEntryViewModelFactory;
            _trackTemplateToSimTrackMapper = trackTemplateToSimTrackMapper;

            CalendarEntries = new ObservableCollection<AbstractCalendarEntryViewModel>();
            CalendarEntries.Add(CreateEditableCalendarEntryViewModel("Slovakia Ring"));
            CalendarEntries.Add(CreateEditableCalendarEntryViewModel("Hockeinheim"));
            CalendarEntries.Add(CreateEditableCalendarEntryViewModel("Road America"));
            CalendarEntries.Add(CreateEditableCalendarEntryViewModel("Slovakia Ring 1"));
            CalendarEntries.Add(CreatePlaceholderCalendarEntryViewModel("Laguna Seca"));
            CalendarEntries.Add(CreateEditableCalendarEntryViewModel("Hockeinheim 2"));
            CalendarEntries.Add(CreateEditableCalendarEntryViewModel("Road America 3"));

            RecalculateEventNumbers();

        }

        public ObservableCollection<AbstractCalendarEntryViewModel> CalendarEntries
        {
            get => _calendarEntries;
            set => SetProperty(ref _calendarEntries, value);
        }

        public string SimulatorName { get; set; }

        public int TotalEvents
        {
            get => _totalEvents;
            set => SetProperty(ref _totalEvents, value);
        }

        public ICommand SelectPredefinedCalendarCommand
        {
            get;
            set;
        }

        public void ApplyCalendarTemplate(CalendarTemplate calendarTemplate, bool useCalendarEventNames, bool autoReplaceKnownTracks)
        {
            ObservableCollection<AbstractCalendarEntryViewModel> newCalendarEntries = new ObservableCollection<AbstractCalendarEntryViewModel>(calendarTemplate.Events.Select(x =>
            {
                var newEntry = _calendarEntryViewModelFactory.Create(x, SimulatorName, useCalendarEventNames, autoReplaceKnownTracks);
                newEntry.DeleteEntryCommand = new RelayCommand(() => DeleteCalendarEntry(newEntry));
                return newEntry;
            }));
            CalendarEntries = newCalendarEntries;
            RecalculateEventNumbers();
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var target = (dropInfo.VisualTarget as FrameworkElement)?.DataContext;
            if (target is CreatedCalendarViewModel)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
                dropInfo.NotHandled = false;
                return;
            }

            if (target is CalendarPlaceholderEntryViewModel && dropInfo.Data is ExistingTrackTemplateViewModel)
            {
                dropInfo.DropTargetAdorner = typeof(AllowDropAdorner);
                dropInfo.Effects = DragDropEffects.Copy;
                dropInfo.NotHandled = false;
                return;
            }

            /*if (dropInfo.TargetItem is AbstractCalendarEntryViewModel && (target is EditableCalendarEntryViewModel || target is ExistingTrackCalendarEntryViewModel || target is CalendarPlaceholderEntryViewModel))
            {*/
                dropInfo.DropTargetAdorner = typeof(ForbidDropAdorner);
                dropInfo.Effects = DragDropEffects.None;
                dropInfo.NotHandled = false;
            /*}*/
        }

        public void Drop(IDropInfo dropInfo)
        {
            var target = (dropInfo.VisualTarget as FrameworkElement)?.DataContext;
            if (dropInfo.Data is AbstractCalendarEntryViewModel abstractCalendarEntry && ReferenceEquals(target, this))
            {
                MoveCalendarEntry(abstractCalendarEntry, dropInfo.InsertIndex);
                return;
            }

            if (target is CalendarPlaceholderEntryViewModel calendarPlaceholder && dropInfo.Data is ExistingTrackTemplateViewModel existingTrackTemplateViewModel)
            {
                ReplacePlaceHolder(calendarPlaceholder, existingTrackTemplateViewModel);
                return;
            }

            if (dropInfo.Data is AbstractTrackTemplateViewModel trackTemplate && ReferenceEquals(target, this))
            {
                CreateEntry(trackTemplate, dropInfo.InsertIndex);
            }
        }

        private void ReplacePlaceHolder(CalendarPlaceholderEntryViewModel calendarPlaceholder, ExistingTrackTemplateViewModel existingTrackTemplateViewModel)
        {
            int index = _calendarEntries.IndexOf(calendarPlaceholder);
            _calendarEntries.Remove(calendarPlaceholder);
            CreateEntry(existingTrackTemplateViewModel, index, calendarPlaceholder.CustomEventName);
            _trackTemplateToSimTrackMapper.RegisterSimulatorTrackName(SimulatorName, calendarPlaceholder.TrackName, existingTrackTemplateViewModel.TrackName);

        }

        private void DeleteCalendarEntry(AbstractCalendarEntryViewModel entryToDelete)
        {
            CalendarEntries.Remove(entryToDelete);
            RecalculateEventNumbers();
        }

        private void RecalculateEventNumbers()
        {
            TotalEvents = CalendarEntries.Count;
            for (int i = 0; i < CalendarEntries.Count; i++)
            {
                CalendarEntries[i].EventNumber = i + 1;
                CalendarEntries[i].OriginalEventName = "Event " + (i + 1);
            }
        }

        private CalendarPlaceholderEntryViewModel CreatePlaceholderCalendarEntryViewModel(string trackName)
        {
            CalendarPlaceholderEntryViewModel newViewModel = new CalendarPlaceholderEntryViewModel()
            {
                TrackName = trackName,
            };
            newViewModel.DeleteEntryCommand = new RelayCommand(() => DeleteCalendarEntry(newViewModel));
            return newViewModel;
        }

        private EditableCalendarEntryViewModel CreateEditableCalendarEntryViewModel(string trackName)
        {
            EditableCalendarEntryViewModel newViewModel = new EditableCalendarEntryViewModel()
            {
                TrackName = trackName,
            };
            newViewModel.DeleteEntryCommand = new RelayCommand(() => DeleteCalendarEntry(newViewModel));
            return newViewModel;
        }

        private void CreateEntry(AbstractTrackTemplateViewModel trackTemplate, int insertionIndex, string customEventName = "")
        {
            var newEntry = _calendarEntryViewModelFactory.Create(trackTemplate);
            newEntry.DeleteEntryCommand = new RelayCommand(() => DeleteCalendarEntry(newEntry));
            newEntry.CustomEventName = customEventName;
            CalendarEntries.Insert(insertionIndex, newEntry);
            RecalculateEventNumbers();
        }

        private void MoveCalendarEntry(AbstractCalendarEntryViewModel entry, int insertPosition)
        {
            int oldIndex = CalendarEntries.IndexOf(entry);
            if (oldIndex < insertPosition)
            {
                CalendarEntries.Move(oldIndex, Math.Max(0, insertPosition - 1));
            }
            else
            {
                CalendarEntries.Move(oldIndex, Math.Min(CalendarEntries.Count - 1, insertPosition));
            }

            RecalculateEventNumbers();
        }
    }
}