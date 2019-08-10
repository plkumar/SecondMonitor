namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using DataModel.Telemetry;
    using OxyPlot;
    using Providers;

    public class ScatterPlotSeries
    {
        private readonly List<ScatterPlotPoint> _dataPoints;

        public ScatterPlotSeries(OxyColor color, string seriesName)
        {
            Color = color;
            SeriesName = seriesName;
            _dataPoints = new List<ScatterPlotPoint>();
        }

        public OxyColor Color { get; }
        public string SeriesName { get; }
        public IReadOnlyCollection<ScatterPlotPoint> DataPoints => _dataPoints.AsReadOnly();

        public void AddDataPoint(double x, double y, TimedTelemetrySnapshot telemetryPoint)
        {
            _dataPoints.Add( new ScatterPlotPoint(x, y, telemetryPoint));
        }

        public void AddDataPoints(IEnumerable<(double x, double y, TimedTelemetrySnapshot telemetryPoint)> points)
        {
            _dataPoints.AddRange(points.Select(x => new ScatterPlotPoint(x.x, x.y, x.telemetryPoint)));
        }

    }
}