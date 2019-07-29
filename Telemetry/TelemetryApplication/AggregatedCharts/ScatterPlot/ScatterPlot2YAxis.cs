namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot
{
    using System.Collections.Generic;
    using SecondMonitor.ViewModels.Properties;

    public class ScatterPlot2YAxis : ScatterPlot
    {
        private readonly List<ScatterPlotSeries> _scatterPlotY2Series;

        public ScatterPlot2YAxis(string title, AxisDefinition xAxis, AxisDefinition yAxis, AxisDefinition y2Axis) : base(title, xAxis, yAxis)
        {
            Y2Axis = y2Axis;
            _scatterPlotY2Series = new List<ScatterPlotSeries>();
        }

        public AxisDefinition Y2Axis { get; }

        public IReadOnlyCollection<ScatterPlotSeries> ScatterPlotY2Series => _scatterPlotY2Series.AsReadOnly();

        public void AddScatterPlotY2Series([CanBeNull]ScatterPlotSeries newSeries)
        {
            if (newSeries == null)
            {
                return;
            }

            _scatterPlotY2Series.Add(newSeries);
        }
    }
}