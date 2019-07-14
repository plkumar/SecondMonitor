namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class TyreLoadToLongGProvider : AbstractWheelChartProvider
    {
        public TyreLoadToLongGProvider(TyreLoadToLongGExtractor dataExtractor, ILoadedLapsCache loadedLaps) : base(dataExtractor, loadedLaps)
        {
        }

        public override string ChartName => "Tyre Load / Longitudinal Acceleration ";
    }
}