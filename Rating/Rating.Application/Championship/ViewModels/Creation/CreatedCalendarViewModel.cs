namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;
    using Calendar;
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

    public abstract class ColorRectangleAdorner : DropTargetAdorner
    {
        protected ColorRectangleAdorner(UIElement adornedElement, DropInfo dropInfo) : base(adornedElement, dropInfo)
        {
        }

        protected abstract Color RenderColor { get; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            FrameworkElement frameworkElement = AdornedElement as FrameworkElement;
            Rect adornedElementRect = new Rect(new Point(0, 0), new Size(frameworkElement.ActualWidth, frameworkElement.ActualHeight));

            // Some arbitrary drawing implements.
            SolidColorBrush renderBrush = new SolidColorBrush(RenderColor);
            renderBrush.Opacity = 0.5;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
            drawingContext.DrawRectangle(renderBrush, renderPen, adornedElementRect);
        }
    }

    public class AllowDropAdorner : ColorRectangleAdorner
    {
        public AllowDropAdorner(UIElement adornedElement, DropInfo dropInfo) : base(adornedElement, dropInfo)
        {
        }


        protected override Color RenderColor => Colors.Green;
    }

    public class ForbidDropAdorner : ColorRectangleAdorner
    {
        public ForbidDropAdorner(UIElement adornedElement, DropInfo dropInfo) : base(adornedElement, dropInfo)
        {
        }


        protected override Color RenderColor => Colors.Red;
    }
}