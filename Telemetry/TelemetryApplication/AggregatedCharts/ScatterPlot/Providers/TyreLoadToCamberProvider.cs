namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class TyreLoadToCamberProvider : AbstractWheelChartProvider
    {
        public TyreLoadToCamberProvider(TyreLoadToCamberExtractor dataExtractor, ILoadedLapsCache loadedLaps) : base(dataExtractor, loadedLaps)
        {
        }

        public override string ChartName => "Tyre Load / Camber";
    }
}