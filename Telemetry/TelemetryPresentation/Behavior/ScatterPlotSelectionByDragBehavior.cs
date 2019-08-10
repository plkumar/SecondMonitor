namespace SecondMonitor.TelemetryPresentation.Behavior
{
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using WindowsControls.WinForms.PlotViewWrapper;
    using Telemetry.TelemetryApplication.ViewModels.AggregatedCharts.ScatterPlot;
    using Template;
    using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

    public class ScatterPlotSelectionByDragBehavior : Behavior<HostChartWrapper>
    {
        public static readonly DependencyProperty ScatterPlotChartViewModelProperty = DependencyProperty.Register("ScatterPlotChartViewModel", typeof(ScatterPlotChartViewModel), typeof(ScatterPlotSelectionByDragBehavior));

        private bool _isTracking;
        private Point _startTrackingPoint;

        public ScatterPlotChartViewModel ScatterPlotChartViewModel
        {
            get => (ScatterPlotChartViewModel)GetValue(ScatterPlotChartViewModelProperty);
            set => SetValue(ScatterPlotChartViewModelProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                ((PlotViewWrapper)AssociatedObject.FormsHost.Child).GetPlotView().MouseDown += AssociatedObjectOnMouseDown;
                ((PlotViewWrapper)AssociatedObject.FormsHost.Child).GetPlotView().MouseUp += AssociatedObjectOnMouseUp;
                ((PlotViewWrapper)AssociatedObject.FormsHost.Child).GetPlotView().MouseMove += AssociatedObjectOnMouseMove;
            }
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs e)
        {
            if (ScatterPlotChartViewModel == null || !_isTracking)
            {
                return;
            }

            if (e.Button != MouseButtons.Right || !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.LeftAlt)))
            {
                _isTracking = false;
                ScatterPlotChartViewModel.HideSelectionRectangle();
                return;
            }

            ScatterPlotChartViewModel.MoveSelectionRectangle(_startTrackingPoint, new Point(e.X, e.Y));
        }

        private void AssociatedObjectOnMouseUp(object sender, MouseEventArgs e)
        {
            if (ScatterPlotChartViewModel == null || e.Button == MouseButtons.Left || !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.LeftAlt)) || !_isTracking)
            {
                return;
            }

            _isTracking = false;
            ScatterPlotChartViewModel.HideSelectionRectangle();
            if (Keyboard.IsKeyDown(Key.LeftAlt))
            {
                ScatterPlotChartViewModel.DeSelectPointsInArea(_startTrackingPoint, new Point(e.X, e.Y));
            }
            else
            {
                ScatterPlotChartViewModel.SelectPointsInArea(_startTrackingPoint, new Point(e.X, e.Y));
            }
        }

        private void AssociatedObjectOnMouseDown(object sender, MouseEventArgs e)
        {
            if (ScatterPlotChartViewModel == null || e.Button == MouseButtons.Left || !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.LeftAlt)) || _isTracking)
            {
                return;
            }

            _isTracking = true;
            _startTrackingPoint = new Point(e.X, e.Y);
            ScatterPlotChartViewModel.ShowSelectionRectangle(_startTrackingPoint, new Point(e.X + 10, e.Y + 10), Keyboard.IsKeyDown(Key.LeftAlt));
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                ((PlotViewWrapper)AssociatedObject.FormsHost.Child).GetPlotView().MouseDown -= AssociatedObjectOnMouseDown;
                ((PlotViewWrapper)AssociatedObject.FormsHost.Child).GetPlotView().MouseUp -= AssociatedObjectOnMouseUp;
            }
            base.OnDetaching();
        }
    }
}