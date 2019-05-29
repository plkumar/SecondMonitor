namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.AggregatedCharts.Histogram
{
    public class SuspensionVelocityWheelsChartViewModel : WheelsHistogramChartViewModel
    {
        public double Minimum { get; set; }
        public double Maximum { get; set; }

        public double FastSlowBoundary { get; set; }
    }
}
