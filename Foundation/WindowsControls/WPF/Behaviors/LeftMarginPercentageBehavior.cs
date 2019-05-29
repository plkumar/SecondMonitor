namespace SecondMonitor.WindowsControls.WPF.Behaviors
{
    using System.Windows;
    using System.Windows.Interactivity;

    public class LeftMarginPercentageBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty PercentageProperty = DependencyProperty.Register(
            "Percentage", typeof(double), typeof(LeftMarginPercentageBehavior), new FrameworkPropertyMetadata() { PropertyChangedCallback = OnPercentagesPropertyChanged });

        private FrameworkElement _parentElement;

        public double Percentage
        {
            get => (double) GetValue(PercentageProperty);
            set => SetValue(PercentageProperty, value);
        }

        protected override void OnAttached()
        {
            _parentElement = (FrameworkElement)LogicalTreeHelper.GetParent(AssociatedObject);
        }

        private void UpdateXPosition()
        {
            if (AssociatedObject == null || _parentElement == null)
            {
                return;
            }

            double xPosition = _parentElement.ActualWidth * ((Percentage) / 100) - AssociatedObject.ActualWidth;
            if (xPosition >= 0)
            {
                AssociatedObject.Margin = new Thickness(xPosition,0,0,0);
            }
        }

        private static void OnPercentagesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LeftMarginPercentageBehavior xPositionInversePercentageBehavior)
            {
                xPositionInversePercentageBehavior.UpdateXPosition();
            }
        }
    }
}