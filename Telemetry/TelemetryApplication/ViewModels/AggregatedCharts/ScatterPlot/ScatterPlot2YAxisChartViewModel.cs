namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.AggregatedCharts.ScatterPlot
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using Contracts.Commands;
    using DataModel.Extensions;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using SecondMonitor.ViewModels;
    using TelemetryApplication.AggregatedCharts.ScatterPlot;

    public class ScatterPlot2YAxisChartViewModel : AbstractViewModel<ScatterPlot2YAxis>, IAggregatedChartViewModel
    {
        private static readonly OxyColor BaseColor = OxyColors.White;

        private PlotModel _plotModel;
        private int _dataPointsCount;
        private string _title;
        private double _averageBand;
        private string _averageUnits;
        private bool _showAverage;
        private LineSeries _averageSeries;

        public ScatterPlot2YAxisChartViewModel()
        {
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
                IsLegendVisible = true,
                TextColor = BaseColor,
                PlotAreaBorderColor = BaseColor,
                LegendBorder = OxyColors.DarkRed,
                LegendBorderThickness = 1,
                LegendPlacement = LegendPlacement.Outside,
            };


            LinearAxis xAxis = CreateAxis(OriginalModel.XAxis, AxisPosition.Bottom, "X1");
            LinearAxis yAxis = CreateAxis(OriginalModel.YAxis, AxisPosition.Left, "Y1");
            LinearAxis y2Axis = CreateAxis(OriginalModel.Y2Axis, AxisPosition.Right, "Y2");
            IEnumerable<ScatterSeries> series = OriginalModel.ScatterPlotSeries.Select(x => BuildScatterSeries(x, OriginalModel.YAxis, "Y1"));
            IEnumerable<ScatterSeries> y2Series = OriginalModel.ScatterPlotY2Series.Select(x => BuildScatterSeries(x, OriginalModel.Y2Axis, "Y2"));

            bool isInternalChange = false;
            yAxis.AxisChanged += (s, e) =>
            {
                if (isInternalChange)
                {
                    return;
                }

                isInternalChange = true;
                y2Axis.Zoom(yAxis.ActualMinimum, yAxis.ActualMaximum);
                PlotModel.InvalidatePlot(false);
                isInternalChange = false;
            };

            y2Axis.AxisChanged += (s, e) =>
            {
                if (isInternalChange)
                {
                    return;
                }

                isInternalChange = true;
                yAxis.Zoom(y2Axis.ActualMinimum, y2Axis.ActualMaximum);
                PlotModel.InvalidatePlot(false);
                isInternalChange = false;
            };

            series.ForEach(model.Series.Add);
            y2Series.ForEach(model.Series.Add);
            model.Axes.Add(xAxis);
            model.Axes.Add(yAxis);
            model.Axes.Add(y2Axis);
            OriginalModel.Annotations.ForEach(model.Annotations.Add);
            PlotModel = model;


            DataPointsCount = OriginalModel.ScatterPlotSeries.Select(x => x.DataPoints.Count).Sum();
        }

        private static LinearAxis CreateAxis(AxisDefinition axisDefinition, AxisPosition position, string yAxisKey)
        {
            LinearAxis axis = new LinearAxis
            {
                Title = axisDefinition.Title,
                AxislineColor = BaseColor,
                Position = position,
                MajorStep = axisDefinition.MajorTick,
                MinorGridlineStyle = LineStyle.None,
                MinorGridlineColor = BaseColor,
                MinorGridlineThickness = 0.5,
                MinorStep = axisDefinition.MinorTick,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = BaseColor,
                TicklineColor = BaseColor,
                Unit = axisDefinition.Unit,
                ExtraGridlineStyle = LineStyle.Solid,
                MinimumPadding = 0.2,
                MaximumPadding = 0.2,
                ExtraGridlineColor = OxyColors.Red,
                ExtraGridlineThickness = 2,
                ExtraGridlines = new double[] { 0 },
                Key = yAxisKey,
            };

            if (axisDefinition.UseCustomRange)
            {
                axis.Minimum = axisDefinition.Minimum;
                axis.Maximum = axisDefinition.Maximum;
            }

            return axis;
        }

        protected ScatterSeries BuildScatterSeries(ScatterPlotSeries scatterPlotSeries, AxisDefinition yAxisDefinition,  string yAxisKey)
        {
            ScatterSeries scatterSeries = new ScatterSeries() { Title = scatterPlotSeries.SeriesName, MarkerFill = scatterPlotSeries.Color, MarkerType = MarkerType.Circle, MarkerSize = 3, TrackerFormatString = "{0}\n" + OriginalModel.XAxis.Unit + ": {2}\n" + yAxisDefinition.Unit + ": {4}", YAxisKey = yAxisKey};
            scatterSeries.Points.AddRange(scatterPlotSeries.DataPoints.Select(x => new ScatterPoint(x.X, x.Y)));

            return scatterSeries;
        }

        protected override void ApplyModel(ScatterPlot2YAxis model)
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
            _averageSeries = new LineSeries
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



        public override ScatterPlot2YAxis SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            PlotModel = null;
        }
    }
}