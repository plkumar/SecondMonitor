namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Controllers.Synchronization;
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class TyreLoadToCamberProvider : AbstractWheelChartProvider
    {
        public TyreLoadToCamberProvider(TyreLoadToCamberExtractor dataExtractor, ILoadedLapsCache loadedLaps, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(dataExtractor, loadedLaps, dataPointSelectionSynchronization)
        {
        }

        public override string ChartName => "Tyre Load / Camber";
    }
}