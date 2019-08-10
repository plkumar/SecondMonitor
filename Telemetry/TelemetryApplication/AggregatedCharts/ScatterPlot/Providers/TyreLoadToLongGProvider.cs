namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Controllers.Synchronization;
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class TyreLoadToLongGProvider : AbstractWheelChartProvider
    {
        public TyreLoadToLongGProvider(TyreLoadToLongGExtractor dataExtractor, ILoadedLapsCache loadedLaps, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(dataExtractor, loadedLaps, dataPointSelectionSynchronization)
        {
        }

        public override string ChartName => "Tyre Load / Longitudinal Acceleration ";
    }
}