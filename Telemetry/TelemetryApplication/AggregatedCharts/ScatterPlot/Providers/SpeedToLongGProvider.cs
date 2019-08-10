namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Controllers.Synchronization;
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class SpeedToLongGProvider : AbstractStintScatterPlotProvider
    {
        public SpeedToLongGProvider(ILoadedLapsCache loadedLapsCache, SpeedToLongGAllPointsExtractor dataExtractor, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(loadedLapsCache, dataExtractor, dataPointSelectionSynchronization)
        {
        }

        public override string ChartName => "Long vs Speed";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;
    }
}