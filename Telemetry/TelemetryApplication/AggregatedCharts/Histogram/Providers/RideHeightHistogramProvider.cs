namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Providers
{
    using System.Linq;
    using Extractors;
    using Filter;
    using SecondMonitor.ViewModels.Factory;
    using ViewModels.AggregatedCharts.Histogram;
    using ViewModels.LoadedLapCache;

    public class RideHeightHistogramProvider : AbstractWheelHistogramProvider<WheelsHistogramChartViewModel, HistogramChartViewModel>
    {
        public RideHeightHistogramProvider(RideHeightHistogramExtractor abstractWheelHistogramDataExtractor, ILoadedLapsCache loadedLapsCache, IViewModelFactory viewModelFactory) : base(abstractWheelHistogramDataExtractor, loadedLapsCache, viewModelFactory, Enumerable.Empty<IWheelTelemetryFilter>())
        {
        }

        public override string ChartName => "Ride Height (Wheels)";

        public override AggregatedChartKind Kind => AggregatedChartKind.Histogram;
        protected override void OnNewViewModel(WheelsHistogramChartViewModel newViewModel)
        {

        }
    }
}