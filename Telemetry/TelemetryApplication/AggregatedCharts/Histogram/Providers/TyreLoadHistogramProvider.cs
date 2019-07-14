namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Providers
{
    using System.Collections.Generic;
    using Extractors;
    using Filter;
    using SecondMonitor.ViewModels.Factory;
    using ViewModels.AggregatedCharts.Histogram;
    using ViewModels.LoadedLapCache;

    public class TyreLoadHistogramProvider : AbstractWheelHistogramProvider<WheelsHistogramChartViewModel, HistogramChartWithStatisticsViewModel>
    {
        public TyreLoadHistogramProvider(TyreLoadHistogramExtractor abstractWheelHistogramDataExtractor, ILoadedLapsCache loadedLapsCache, IViewModelFactory viewModelFactory, IEnumerable<IWheelTelemetryFilter> filters) : base(abstractWheelHistogramDataExtractor, loadedLapsCache, viewModelFactory, filters)
        {
        }

        public override string ChartName => "Tyre Load";
        public override AggregatedChartKind Kind => AggregatedChartKind.Histogram;
        protected override bool ResetCommandVisible => false;
        protected override void OnNewViewModel(WheelsHistogramChartViewModel newViewModel)
        {
        }
    }
}