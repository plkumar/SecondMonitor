namespace SecondMonitor.TelemetryPresentation.Behavior
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using System.Windows.Interactivity;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using WindowsControls.WinForms.ColorExtenstion;
    using WindowsControls.WinForms.PlotViewWrapper;
    using OxyPlot;
    using OxyPlot.Axes;
    using Telemetry.TelemetryApplication.ViewModels.GraphPanel;
    using Template;
    using Axis = OxyPlot.Axes.Axis;
    using HorizontalAlignment = System.Windows.HorizontalAlignment;
    using VerticalAlignment = System.Windows.VerticalAlignment;

    public class ShowSelectedDistanceBehavior : Behavior<HostChartWrapper>
    {
        private Dictionary<string, (System.Windows.Forms.Panel rectangle, TranslateTransform transform)> _lapRectangles;

        public static readonly DependencyProperty GraphViewModelProperty = DependencyProperty.Register(
            "GraphViewModel", typeof(AbstractGraphViewModel), typeof(ShowSelectedDistanceBehavior), new PropertyMetadata() {PropertyChangedCallback = GraphViewModelPropertyChangedCallback});

        public ShowSelectedDistanceBehavior()
        {
            _lapRectangles = new Dictionary<string, (System.Windows.Forms.Panel rectangle, TranslateTransform transform)>();
        }

        public AbstractGraphViewModel GraphViewModel
        {
            get => (AbstractGraphViewModel) GetValue(GraphViewModelProperty);
            set => SetValue(GraphViewModelProperty, value);
        }

        protected override void OnAttached()
        {
            SubscribeToDataContextChange();
        }

        private void AddRectangle(string lapId, System.Drawing.Color color, double xValue)
        {
            TranslateTransform transform = new TranslateTransform();
            System.Windows.Forms.Panel rectangle = new System.Windows.Forms.Panel()
            {
                Top = 0,
                Height = 300,
                Width = 2,
                BackColor = color,
                Anchor = AnchorStyles.Left,
            };

            PlotViewWrapper plotView = GetPlotViewWrapper();
            plotView.GetMainPanel().Controls.Add(rectangle);
            rectangle.BringToFront();
            _lapRectangles[lapId] = (rectangle, transform);
            UpdateRectangle(rectangle, transform, xValue, color);
        }

        private void RemoveRectangle(string lapId)
        {
            if (!_lapRectangles.TryGetValue(lapId, out (System.Windows.Forms.Panel rectangle, TranslateTransform transform) value))
            {
                return;
            }

            PlotViewWrapper plotView = GetPlotViewWrapper();
            plotView.GetMainPanel().Controls.Remove(value.rectangle);
            _lapRectangles.Remove(lapId);
        }

        private PlotViewWrapper GetPlotViewWrapper()
        {
            return AssociatedObject.FormsHost.Child as PlotViewWrapper;
        }

        protected override void OnDetaching()
        {
            UnSubscribeToDataContextChange();
        }

        private void SubscribeToDataContextChange()
        {
            AssociatedObject.DataContextChanged += AssociatedObjectOnDataContextChanged;
            SubscribeToPlot();
        }

        private void SubscribeToPlot()
        {
            if (AssociatedObject?.PlotModel == null)
            {
                return;
            }

            AssociatedObject.PlotModel.Axes.CollectionChanged += AxesOnCollectionChanged;
        }

        private void AxesOnCollectionChanged(object sender, ElementCollectionChangedEventArgs<Axis> e)
        {
            foreach (Axis eAddedItem in e.AddedItems)
            {
                eAddedItem.AxisChanged += AxisChange;
            }
        }

        private void AxisChange(object sender, AxisChangedEventArgs e)
        {
            UpdateRectangles();
        }

        private void UnSubscribeToDataContextChange()
        {
            AssociatedObject.DataContextChanged -= AssociatedObjectOnDataContextChanged;
        }

        private void AssociatedObjectOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void GraphViewModelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ShowSelectedDistanceBehavior showSelectedDistance))
            {
                return;
            }

            if (e.OldValue is AbstractGraphViewModel oldGraphViewModel)
            {
                oldGraphViewModel.PropertyChanged -= showSelectedDistance.GraphViewModelPropertyChanged;
            }

            if (e.NewValue is AbstractGraphViewModel newValue)
            {
                newValue.PropertyChanged += showSelectedDistance.GraphViewModelPropertyChanged;
            }
        }

        private void GraphViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(AbstractGraphViewModel.SelectedDistances))
            {
                return;
            }

            UpdateRectangles();
        }


        private void UpdateRectangles()
        {
            List<string> keysToRemove = new List<string>();
            foreach (KeyValuePair<string, (System.Windows.Forms.Panel rectangle, TranslateTransform transform)> lapRectanglesValue in _lapRectangles)
            {
                if (!GraphViewModel.SelectedDistances.TryGetValue(lapRectanglesValue.Key, out (double x, Color color) lapDistance))
                {
                    keysToRemove.Add(lapRectanglesValue.Key);
                    continue;
                }
                UpdateRectangle(lapRectanglesValue.Value.rectangle,lapRectanglesValue.Value.transform, lapDistance.x, lapDistance.color.ToWinFormColor());
            }

            foreach (string lapId in GraphViewModel.SelectedDistances.Keys.Where(x => !_lapRectangles.Keys.Contains(x)))
            {
                AddRectangle(lapId, GraphViewModel.SelectedDistances[lapId].color.ToWinFormColor(), GraphViewModel.SelectedDistances[lapId].x);
            }

            keysToRemove.ForEach(RemoveRectangle);
        }

        private void UpdateRectangle(System.Windows.Forms.Panel rectangle, TranslateTransform translateTransform, double xValue, System.Drawing.Color color)
        {
            PlotModel model = GetPlotModel();
            if (model == null || model.Axes.Count != 2)
            {
                return;
            }

            Axis xAxis = model.Axes.FirstOrDefault(x => x.Position == AxisPosition.Bottom);

            if (xAxis == null)
            {
                return;
            }

            if (rectangle.BackColor != color )
            {
                rectangle.BackColor = color;
            }

            rectangle.Height = (int)model.PlotArea.Height;
            if (xAxis.ActualMinimum > xValue || xValue > xAxis.ActualMaximum)
            {
                rectangle.Visible = false;
                return;
            }

            rectangle.Visible = true;
            double plotRange = xAxis.ActualMaximum - xAxis.ActualMinimum;
            double selectedDistancePortion = (xValue - xAxis.ActualMinimum) / plotRange;
            rectangle.Top = (int)model.PlotArea.Top;
            rectangle.Left = (int) (model.PlotArea.Left + model.PlotArea.Width * selectedDistancePortion);
        }

        private PlotModel GetPlotModel()
        {
            return AssociatedObject.PlotModel;
        }
    }
}