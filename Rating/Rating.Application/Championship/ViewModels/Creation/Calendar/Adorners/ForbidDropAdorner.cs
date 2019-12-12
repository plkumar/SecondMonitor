namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar.Adorners
{
    using System.Windows;
    using System.Windows.Media;
    using GongSolutions.Wpf.DragDrop;

    public class ForbidDropAdorner : ColorRectangleAdorner
    {
        public ForbidDropAdorner(UIElement adornedElement, DropInfo dropInfo) : base(adornedElement, dropInfo)
        {
        }

        protected override Color RenderColor => Colors.Red;
    }
}