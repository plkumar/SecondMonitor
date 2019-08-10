namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Controllers.Synchronization;
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class SpeedToRakeProvider : AbstractStintScatterPlotProvider
    {
        public SpeedToRakeProvider(SpeedToRakeExtractor dataExtractor, ILoadedLapsCache loadedLapsCache, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(loadedLapsCache, dataExtractor, dataPointSelectionSynchronization)
        {

        }

        public override string ChartName => "Rake / Speed";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

    }
}