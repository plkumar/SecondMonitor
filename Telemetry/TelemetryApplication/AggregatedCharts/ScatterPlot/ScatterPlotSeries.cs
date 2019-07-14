namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using OxyPlot;

    public class ScatterPlotSeries
    {
        private readonly List<Point> _dataPoints;

        public ScatterPlotSeries(OxyColor color, string seriesName)
        {
            Color = color;
            SeriesName = seriesName;
            _dataPoints = new List<Point>();
        }

        public OxyColor Color { get; }
        public string SeriesName { get; }
        public IReadOnlyCollection<Point> DataPoints => _dataPoints.AsReadOnly();

        public void AddDataPoint(double x, double y)
        {
            _dataPoints.Add(new Point(x, y));
        }

        public void AddDataPoints(IEnumerable<(double x, double y)> points)
        {
            _dataPoints.AddRange(points.Select(x => new Point(x.x, x.y)));
        }

    }
}