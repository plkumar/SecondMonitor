namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot
{
    using DataModel.Telemetry;

    public class ScatterPlotPoint
    {
        public ScatterPlotPoint(double x, double y, TimedTelemetrySnapshot telemetryPoint)
        {
            X = x;
            Y = y;
            TelemetryPoint = telemetryPoint;
        }

        public double X { get; }
        public double Y { get; }
        public TimedTelemetrySnapshot TelemetryPoint { get; }
    }
}