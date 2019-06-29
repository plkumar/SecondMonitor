namespace SecondMonitor.TelemetryPresentation.Behavior
{
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using WindowsControls.WinForms.PlotViewWrapper;
    using Telemetry.TelemetryApplication.ViewModels.AggregatedCharts.Histogram;
    using Template;
    using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

    public class BandSelectionByClickBehavior : Behavior<HostChartWrapper>
    {
        public static readonly DependencyProperty HistogramChartViewModelProperty = DependencyProperty.Register(
            "HistogramChartViewModel", typeof(HistogramChartViewModel), typeof(BandSelectionByClickBehavior));

        public HistogramChartViewModel HistogramChartViewModel
        {
            get => (HistogramChartViewModel)GetValue(HistogramChartViewModelProperty);
            set => SetValue(HistogramChartViewModelProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                ((PlotViewWrapper) AssociatedObject.FormsHost.Child).GetPlotView().MouseClick += AssociatedObjectOnMouseUp;
            }
        }

        private void AssociatedObjectOnMouseUp(object sender, MouseEventArgs e)
        {
            if (HistogramChartViewModel == null || e.Button == MouseButtons.Left|| !Keyboard.IsKeyDown(Key.LeftShift))
            {
                return;
            }

            HistogramChartViewModel.ToggleSelection(new Point(e.X, e.Y));
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.MouseLeftButtonDown -= AssociatedObjectOnMouseUp;
            }
            base.OnDetaching();
        }

        private void AssociatedObjectOnMouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}