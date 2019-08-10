namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Controllers.Synchronization;
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class SpeedToRpmChartProvider : AbstractGearsChartProvider
    {
        public override string ChartName => "Speed vs RPM";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

        public SpeedToRpmChartProvider(ILoadedLapsCache loadedLapsCache, SpeedToRpmScatterPlotExtractor speedToRpmScatterPlotExtractor, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(loadedLapsCache, speedToRpmScatterPlotExtractor, dataPointSelectionSynchronization)
        {
        }

    }
}