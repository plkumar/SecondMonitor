namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar.Adorners
{
    using System.Windows;
    using System.Windows.Media;
    using GongSolutions.Wpf.DragDrop;

    public class AllowDropAdorner : ColorRectangleAdorner
    {
        public AllowDropAdorner(UIElement adornedElement, DropInfo dropInfo) : base(adornedElement, dropInfo)
        {
        }


        protected override Color RenderColor => Colors.Green;
    }
}