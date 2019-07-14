namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class TyreLoadToLatGProvider : AbstractWheelChartProvider
    {
        public TyreLoadToLatGProvider(TyreLoadToLatGExtractor dataExtractor, ILoadedLapsCache loadedLaps) : base(dataExtractor, loadedLaps)
        {
        }

        public override string ChartName => "Tyre Load / Lateral Acceleration";
    }
}