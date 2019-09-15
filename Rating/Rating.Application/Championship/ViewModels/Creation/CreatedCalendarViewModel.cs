namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using Calendar;
    using Calendar.Adorners;
    using Contracts.Commands;
    using GongSolutions.Wpf.DragDrop;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class CreatedCalendarViewModel : AbstractViewModel, IDropTarget
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ICalendarEntryViewModelFactory _calendarEntryViewModelFactory;
        private int _totalEvents;

        public CreatedCalendarViewModel(IViewModelFactory viewModelFactory, ICalendarEntryViewModelFactory calendarEntryViewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            _calendarEntryViewModelFactory = calendarEntryViewModelFactory;

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

        public ObservableCollection<AbstractCalendarEntryViewModel> CalendarEntries { get; }

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

        public void DragOver(IDropInfo dropInfo)
        {
            var target = (dropInfo.VisualTarget as FrameworkElement)?.DataContext;
            if (target is CreatedCalendarViewModel)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
                dropInfo.NotHandled = false;
            }

            if (target is EditableCalendarEntryViewModel || target is ExistingTrackCalendarEntryViewModel)
            {
                dropInfo.DropTargetAdorner = typeof(ForbidDropAdorner);
                dropInfo.Effects = DragDropEffects.None;
                dropInfo.NotHandled = false;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            var target = (dropInfo.VisualTarget as FrameworkElement)?.DataContext;
            if (dropInfo.Data is AbstractCalendarEntryViewModel abstractCalendarEntry && ReferenceEquals(target, this))
            {
                MoveCalendarEntry(abstractCalendarEntry, dropInfo.InsertIndex);
                return;
            }

            if (dropInfo.Data is AbstractTrackTemplateViewModel trackTemplate && ReferenceEquals(target, this))
            {
                CreateEntry(trackTemplate, dropInfo.InsertIndex);
            }
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

        private void CreateEntry(AbstractTrackTemplateViewModel trackTemplate, int insertionIndex)
        {
            var newEntry = _calendarEntryViewModelFactory.Create(trackTemplate);
            newEntry.DeleteEntryCommand = new RelayCommand(() => DeleteCalendarEntry(newEntry));
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