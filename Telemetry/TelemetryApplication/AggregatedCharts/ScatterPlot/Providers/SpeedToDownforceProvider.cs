namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Controllers.Synchronization;
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class SpeedToDownforceProvider : AbstractStintScatterPlotProvider
    {
        public SpeedToDownforceProvider(SpeedToDownforceExtractor dataExtractor, ILoadedLapsCache loadedLapsCache, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(loadedLapsCache, dataExtractor, dataPointSelectionSynchronization)
        {
        }

        public override string ChartName => "Downforce / Speed";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

    }
}