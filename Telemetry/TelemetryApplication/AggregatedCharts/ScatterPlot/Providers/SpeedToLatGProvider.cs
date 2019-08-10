namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Controllers.Synchronization;
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class SpeedToLatGProvider : AbstractStintScatterPlotProvider
    {
        public SpeedToLatGProvider(ILoadedLapsCache loadedLapsCache, SpeedToLatGAllPointsExtractor dataExtractor, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(loadedLapsCache, dataExtractor, dataPointSelectionSynchronization)
        {
        }

        public override string ChartName => "Lat vs Speed";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;
    }
}