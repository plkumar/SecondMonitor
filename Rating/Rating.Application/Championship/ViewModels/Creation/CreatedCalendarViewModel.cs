namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;
    using GongSolutions.Wpf.DragDrop;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class CreatedCalendarViewModel : AbstractViewModel, IDropTarget
    {
        private readonly IViewModelFactory _viewModelFactory;

        public CreatedCalendarViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            CalendarEntries = new ObservableCollection<CalendarEntryViewModel> {_viewModelFactory.Create<CalendarEntryViewModel>(), _viewModelFactory.Create<CalendarEntryViewModel>()};
        }

        public ObservableCollection<CalendarEntryViewModel> CalendarEntries { get; }

        public void DragOver(IDropInfo dropInfo)
        {
            var target = (dropInfo.VisualTarget as FrameworkElement)?.DataContext;
            if (target is CreatedCalendarViewModel)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
                dropInfo.NotHandled = false;
            }

            if (target is CalendarEntryViewModel)
            {
                dropInfo.DropTargetAdorner = typeof(TestAdorner);
                dropInfo.Effects = DragDropEffects.Move;
                dropInfo.EffectText = "Foo";
            }
        }

        public void Drop(IDropInfo dropInfo)
        {

        }
    }


    public class TestAdorner : DropTargetAdorner
    {
        public TestAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }

        public TestAdorner(UIElement adornedElement, DropInfo dropInfo) : base(adornedElement, dropInfo)
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            // Some arbitrary drawing implements.
            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            renderBrush.Opacity = 0.5;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
            drawingContext.DrawRectangle(renderBrush, renderPen, adornedElementRect);
        }

    }
}