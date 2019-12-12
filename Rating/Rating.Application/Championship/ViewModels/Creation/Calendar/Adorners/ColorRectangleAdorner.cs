namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar.Adorners
{
    using System.Windows;
    using System.Windows.Media;
    using GongSolutions.Wpf.DragDrop;

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
}