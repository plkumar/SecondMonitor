namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation
{
    using System.Windows;
    using System.Windows.Controls;
    using GongSolutions.Wpf.DragDrop;
    using SecondMonitor.ViewModels;

    public class CalendarEntryViewModel : AbstractViewModel, IDropTarget
    {
        public void DragOver(IDropInfo dropInfo)
        {
            var target = (dropInfo.VisualTarget as FrameworkElement)?.DataContext;
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
}