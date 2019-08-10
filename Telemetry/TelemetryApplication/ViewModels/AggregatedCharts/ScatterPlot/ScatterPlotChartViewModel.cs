namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.AggregatedCharts.ScatterPlot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using AggregatedCharts;
    using Contracts.Commands;
    using Controllers.Synchronization;
    using DataModel.Extensions;
    using DataModel.Telemetry;
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using SecondMonitor.ViewModels;
    using TelemetryApplication.AggregatedCharts.ScatterPlot;

    public class ScatterPlotChartViewModel : AbstractViewModel<ScatterPlot>, IAggregatedChartViewModel
    {
        private const int SelectedSize = 8;
        private const int DeselectedSize = 3;

        private readonly IDataPointSelectionSynchronization _dataPointSelectionSynchronization;
        private readonly Dictionary<ScatterPoint, TimedTelemetrySnapshot> _scatterPointToDataPointMap;
        private static readonly OxyColor BaseColor = OxyColors.White;

        private PlotModel _plotModel;
        private int _dataPointsCount;
        private string _title;
        private double _averageBand;
        private string _averageUnits;
        private bool _showAverage;
        private Axis _xAxis;
        private Axis _yAxis;
        private LineSeries _averageSeries;
        private RectangleAnnotation _selectionAnnotation;
        private readonly Dictionary<TimedTelemetrySnapshot, HashSet<ScatterPoint>> _telemetryPointToScatterPointMap;
        private bool _internalSelection;


        public ScatterPlotChartViewModel(IDataPointSelectionSynchronization dataPointSelectionSynchronization)
        {
            _telemetryPointToScatterPointMap = new Dictionary<TimedTelemetrySnapshot, HashSet<ScatterPoint>>();
            _dataPointSelectionSynchronization = dataPointSelectionSynchronization;
            _scatterPointToDataPointMap = new Dictionary<ScatterPoint, TimedTelemetrySnapshot>();
            RefreshAverageCommand = new RelayCommand(CreateAverageSeries);
        }

        public PlotModel PlotModel
        {
            get => _plotModel;
            set => SetProperty(ref _plotModel, value);
        }

        public int DataPointsCount
        {
            get => _dataPointsCount;
            set => SetProperty(ref _dataPointsCount, value);
        }

        public double AverageBand
        {
            get => _averageBand;
            set => SetProperty(ref _averageBand, value);
        }

        public bool ShowAverage
        {
            get => _showAverage;
            set
            {
                SetProperty(ref _showAverage, value);
                if (_averageSeries == null)
                {
                    return;
                }

                _averageSeries.IsVisible = value;
                PlotModel.InvalidatePlot(false);
            }
        }

        public ICommand RefreshAverageCommand { get; }

        public string AverageUnits
        {
            get => _averageUnits;
            set => SetProperty(ref _averageUnits, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private void BuildPlotModel()
        {
            PlotModel model = new PlotModel
            {
                Title = OriginalModel.Title,
                IsLegendVisible = OriginalModel.IsLegendVisible,
                TextColor = BaseColor,
                PlotAreaBorderColor = BaseColor,
                LegendBorder = OxyColors.DarkRed,
                LegendBorderThickness = 1,
                LegendPlacement = LegendPlacement.Outside,
            };


            _xAxis = CreateAxis(OriginalModel.XAxis, AxisPosition.Bottom);
            _yAxis = CreateAxis(OriginalModel.YAxis, AxisPosition.Left);
            IEnumerable<ScatterSeries> series = OriginalModel.ScatterPlotSeries.Select(BuildScatterSeries);

            series.ForEach(model.Series.Add);
            model.Axes.Add(_xAxis);
            model.Axes.Add(_yAxis);
            OriginalModel.Annotations.ForEach(model.Annotations.Add);
            PlotModel = model;

            DataPointsCount = OriginalModel.ScatterPlotSeries.Select(x => x.DataPoints.Count).Sum();

            _dataPointSelectionSynchronization.OnPointsSelected += DataPointSelectionSynchronizationOnOnPointsSelected;
            _dataPointSelectionSynchronization.OnPointsDeselected += DataPointSelectionSynchronizationOnOnPointsDeselected;
        }

        private LinearAxis CreateAxis(AxisDefinition axisDefinition, AxisPosition position)
        {
            LinearAxis axis = new LinearAxis { Title = axisDefinition.Title, AxislineColor = BaseColor, Position = position, MajorStep = axisDefinition.MajorTick, MinorGridlineStyle = LineStyle.None, MinorGridlineColor = BaseColor, MinorGridlineThickness = 0.5,
                MinorStep = axisDefinition.MinorTick, MajorGridlineStyle = LineStyle.Solid, MajorGridlineColor = BaseColor, TicklineColor = BaseColor, Unit = axisDefinition.Unit,
                ExtraGridlineStyle = LineStyle.Solid, MinimumPadding = 0.2, MaximumPadding = 0.2, ExtraGridlineColor = OxyColors.Red, ExtraGridlineThickness = 2, ExtraGridlines = new double[] { 0 } };

            if (axisDefinition.UseCustomRange)
            {
                axis.Minimum = axisDefinition.Minimum;
                axis.Maximum = axisDefinition.Maximum;
            }

            return axis;
        }

        protected ScatterSeries BuildScatterSeries(ScatterPlotSeries scatterPlotSeries)
        {
            ScatterSeries scatterSeries = new ScatterSeries(){Title = scatterPlotSeries.SeriesName, MarkerFill = scatterPlotSeries.Color, MarkerType = MarkerType.Circle, MarkerSize = DeselectedSize, TrackerFormatString = "{0}\n" + OriginalModel.XAxis.Unit + ": {2}\n" + OriginalModel.YAxis.Unit + ": {4}", SelectionMode = SelectionMode.Multiple};
            scatterSeries.Points.AddRange(BuildScatterPoints(scatterPlotSeries.DataPoints));

            return scatterSeries;
        }

        private IEnumerable<ScatterPoint> BuildScatterPoints(IEnumerable<ScatterPlotPoint> dataPoints)
        {
            foreach (ScatterPlotPoint scatterPlotPoint in dataPoints)
            {
                var newPoint = new ScatterPoint(scatterPlotPoint.X, scatterPlotPoint.Y);
                _scatterPointToDataPointMap[newPoint] = scatterPlotPoint.TelemetryPoint;
                if (!_telemetryPointToScatterPointMap.TryGetValue(scatterPlotPoint.TelemetryPoint, out HashSet<ScatterPoint> telemetrySet))
                {
                    telemetrySet = new HashSet<ScatterPoint>() {newPoint};
                    _telemetryPointToScatterPointMap[scatterPlotPoint.TelemetryPoint] = telemetrySet;
                    yield return newPoint;
                }

                telemetrySet.Add(newPoint);
                yield return newPoint;
            }
        }

        protected override void ApplyModel(ScatterPlot model)
        {
            AverageBand = model.XAxis.MinorTick;
            AverageUnits = model.XAxis.Unit;
            BuildPlotModel();
            CreateAverageSeries();
            ShowAverage = false;
        }

        private void CreateAverageSeries()
        {
            if (OriginalModel == null)
            {
                return;
            }

            if (_averageSeries != null)
            {
                PlotModel?.Series.Remove(_averageSeries);
            }

            List<DataPoint> dataPoints = OriginalModel.ScatterPlotSeries.SelectMany(GetAverageDataPoint).ToList();
            _averageSeries =  new LineSeries
            {
                Title = "Average",
                Color = OxyColors.YellowGreen,
                TextColor = OxyColors.Yellow,
                CanTrackerInterpolatePoints = true,
                StrokeThickness = 2,
                TrackerFormatString = "{0}\n" + OriginalModel.XAxis.Unit + ": {2}\n" + OriginalModel.YAxis.Unit + ": {4}"
            };
            _averageSeries.Points.AddRange(dataPoints);
            PlotModel.Series.Add(_averageSeries);
            SelectTelemetryPoints(_dataPointSelectionSynchronization.SelectedPoints);
            PlotModel.InvalidatePlot(true);
        }

        private List<DataPoint> GetAverageDataPoint(ScatterPlotSeries series)
        {
            double currentBand = double.MinValue;
            List<ScatterPlotPoint> pointsInBand = new List<ScatterPlotPoint>();
            List<DataPoint> pointsToReturn = new List<DataPoint>();
            foreach (ScatterPlotPoint point in series.DataPoints.OrderBy(x => x.X))
            {
                if (point.X - currentBand > AverageBand)
                {
                    if (pointsInBand.Count > 0)
                    {
                        double averageX = pointsInBand.Select(x => x.X).Average();
                        double averageY = pointsInBand.Select(x => x.Y).Average();
                        pointsToReturn.Add(new DataPoint(averageX, averageY));
                        pointsInBand.Clear();
                    }

                    if (pointsToReturn.Count == 0)
                    {
                        currentBand = point.X;
                    }

                    while (point.X - currentBand > AverageBand)
                    {
                        currentBand += AverageBand;
                    }
                }
                pointsInBand.Add(point);
            }

            if (pointsInBand.Count > 0)
            {
                double averageX = pointsInBand.Select(x => x.X).Average();
                double averageY = pointsInBand.Select(x => x.Y).Average();
                pointsToReturn.Add(new DataPoint(averageX, averageY));
                pointsInBand.Clear();
            }

            return pointsToReturn;
        }

        public void SelectPointsInArea(Point point1, Point point2)
        {
            if (_xAxis == null || _yAxis == null)
            {
                return;
            }

            RemoveSelectionRectangle();

            DataPoint dp1 = _xAxis.InverseTransform(point1.X, point1.Y, _yAxis);
            DataPoint dp2 = _xAxis.InverseTransform(point2.X, point2.Y, _yAxis);
            double minimumX = Math.Min(dp1.X, dp2.X);
            double minimumY = Math.Min(dp1.Y, dp2.Y);
            double maximumX = Math.Max(dp1.X, dp2.X);
            double maximumY = Math.Max(dp1.Y, dp2.Y);

            var eligiblePoints = PlotModel.Series.OfType<ScatterSeries>().SelectMany(x => x.ActualPoints).Where(x => x.X >= minimumX && x.X <= maximumX && x.Y >= minimumY && x.Y <= maximumY).ToList();
            eligiblePoints.ForEach(x => x.Size = SelectedSize);
            var eligibleDataPoints = eligiblePoints.Select(x => _scatterPointToDataPointMap[x]).ToList();
            _internalSelection = true;
            _dataPointSelectionSynchronization.SelectPoints(eligibleDataPoints);
            _internalSelection = false;

        }

        public void DeSelectPointsInArea(Point point1, Point point2)
        {
            if (_xAxis == null || _yAxis == null)
            {
                return;
            }

            RemoveSelectionRectangle();

            DataPoint dp1 = _xAxis.InverseTransform(point1.X, point1.Y, _yAxis);
            DataPoint dp2 = _xAxis.InverseTransform(point2.X, point2.Y, _yAxis);
            double minimumX = Math.Min(dp1.X, dp2.X);
            double minimumY = Math.Min(dp1.Y, dp2.Y);
            double maximumX = Math.Max(dp1.X, dp2.X);
            double maximumY = Math.Max(dp1.Y, dp2.Y);

            var eligiblePoints = PlotModel.Series.OfType<ScatterSeries>().SelectMany(x => x.ActualPoints).Where(x => x.X >= minimumX && x.X <= maximumX && x.Y >= minimumY && x.Y <= maximumY).ToList();
            eligiblePoints.ForEach(x => x.Size = DeselectedSize);
            var eligibleDataPoints = eligiblePoints.Select(x => _scatterPointToDataPointMap[x]).ToList();
            _internalSelection = true;
            _dataPointSelectionSynchronization.DeSelectPoints(eligibleDataPoints);
            _internalSelection = false;

        }

        private void SelectTelemetryPoints(IEnumerable<TimedTelemetrySnapshot> timedTelemetrySnapshots)
        {
            var eligiblePoints = timedTelemetrySnapshots.Select(x => _telemetryPointToScatterPointMap.TryGetValue(x, out HashSet<ScatterPoint> value) ? value : null).Where(x => x != null);
            eligiblePoints.SelectMany(x => x).ForEach(x => x.Size = SelectedSize);
        }

        private void DeSelectTelemetryPoints(IEnumerable<TimedTelemetrySnapshot> timedTelemetrySnapshots)
        {
            var eligiblePoints = timedTelemetrySnapshots.Select(x => _telemetryPointToScatterPointMap.TryGetValue(x, out HashSet<ScatterPoint> value) ? value : null).Where(x => x != null);
            eligiblePoints.SelectMany(x => x).ForEach(x => x.Size = DeselectedSize);
        }

        public void ShowSelectionRectangle(Point point1, Point point2, bool isDeselect)
        {
            if (_xAxis == null || _yAxis == null)
            {
                return;
            }

            RemoveSelectionRectangle();

            DataPoint dp1 = _xAxis.InverseTransform(point1.X, point1.Y, _yAxis);
            DataPoint dp2 = _xAxis.InverseTransform(point2.X, point2.Y, _yAxis);
            double minimumX = Math.Min(dp1.X, dp2.X);
            double minimumY = Math.Min(dp1.Y, dp2.Y);
            double maximumX = Math.Max(dp1.X, dp2.X);
            double maximumY = Math.Max(dp1.Y, dp2.Y);
            _selectionAnnotation = new RectangleAnnotation() {Fill = isDeselect ? OxyColor.FromAColor(99, OxyColors.Red) : OxyColor.FromAColor(99, OxyColors.Blue), MinimumX = minimumX, MinimumY = minimumY, MaximumX = maximumX, MaximumY = maximumY};
            _plotModel.Annotations.Add(_selectionAnnotation);
            PlotModel.InvalidatePlot(false);
        }

        public void MoveSelectionRectangle(Point point1, Point point2)
        {
            if (_xAxis == null || _yAxis == null || _selectionAnnotation == null)
            {
                return;
            }

            DataPoint dp1 = _xAxis.InverseTransform(point1.X, point1.Y, _yAxis);
            DataPoint dp2 = _xAxis.InverseTransform(point2.X, point2.Y, _yAxis);
            double minimumX = Math.Min(dp1.X, dp2.X);
            double minimumY = Math.Min(dp1.Y, dp2.Y);
            double maximumX = Math.Max(dp1.X, dp2.X);
            double maximumY = Math.Max(dp1.Y, dp2.Y);
            _selectionAnnotation.MinimumX = minimumX;
            _selectionAnnotation.MinimumY = minimumY;
            _selectionAnnotation.MaximumX = maximumX;
            _selectionAnnotation.MaximumY = maximumY;
            PlotModel.InvalidatePlot(false);
        }

        public void HideSelectionRectangle()
        {
            if (_xAxis == null || _yAxis == null)
            {
                return;
            }

            RemoveSelectionRectangle();
        }

        private void RemoveSelectionRectangle()
        {
            if (_selectionAnnotation == null)
            {
                return;
            }

            PlotModel.Annotations.Remove(_selectionAnnotation);
            _selectionAnnotation = null;
            _plotModel.InvalidatePlot(false);
        }

        public override ScatterPlot SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }

        private void DataPointSelectionSynchronizationOnOnPointsDeselected(object sender, TimedTelemetryArgs e)
        {
            if (_internalSelection)
            {
                return;
            }
            DeSelectTelemetryPoints(e.TelemetrySnapshots);
            _plotModel.InvalidatePlot(false);
        }

        private void DataPointSelectionSynchronizationOnOnPointsSelected(object sender, TimedTelemetryArgs e)
        {
            if (_internalSelection)
            {
                return;
            }
            SelectTelemetryPoints(e.TelemetrySnapshots);
            _plotModel.InvalidatePlot(false);
        }

        public void Dispose()
        {
            PlotModel = null;
            _dataPointSelectionSynchronization.OnPointsDeselected -= DataPointSelectionSynchronizationOnOnPointsDeselected;
            _dataPointSelectionSynchronization.OnPointsSelected -= DataPointSelectionSynchronizationOnOnPointsSelected;
        }
    }
}