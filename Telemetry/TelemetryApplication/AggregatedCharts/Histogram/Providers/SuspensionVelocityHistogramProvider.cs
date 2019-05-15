namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Providers
{
    using System;
    using System.Linq;
    using DataModel.BasicProperties;
    using Extractors;
    using Filter;
    using SecondMonitor.ViewModels.Factory;

    using ViewModels.AggregatedCharts.Histogram;
    using ViewModels.LoadedLapCache;

    public class SuspensionVelocityHistogramProvider : AbstractWheelHistogramProvider<SuspensionVelocityWheelsChartViewModel, SuspensionVelocityHistogramChartViewModel>
    {
        private readonly SuspensionVelocityHistogramDataExtractor _suspensionVelocityHistogramDataExtractor;
        private readonly SuspensionVelocityFilter _suspensionVelocityFilter;

        public SuspensionVelocityHistogramProvider(SuspensionVelocityHistogramDataExtractor suspensionVelocityHistogramDataExtractor, ILoadedLapsCache loadedLapsCache, IViewModelFactory viewModelFactory, SuspensionVelocityFilter suspensionVelocityFilter)
            : base(suspensionVelocityHistogramDataExtractor, loadedLapsCache, viewModelFactory, new []{suspensionVelocityFilter} )
        {
            _suspensionVelocityHistogramDataExtractor = suspensionVelocityHistogramDataExtractor;
            _suspensionVelocityFilter = suspensionVelocityFilter;
        }

        public override string ChartName => "Suspension Velocity Histogram";

        public override AggregatedChartKind Kind => AggregatedChartKind.Histogram;
        protected override void OnNewViewModel(SuspensionVelocityWheelsChartViewModel newViewModel)
        {
            newViewModel.Maximum = _suspensionVelocityHistogramDataExtractor.DefaultMaximum;
            newViewModel.Minimum = _suspensionVelocityHistogramDataExtractor.DefaultMinimum;
        }

        protected override void ApplyHistogramLimits(Histogram flHistogram, Histogram frHistogram, Histogram rlHistogram, Histogram rrHistogram, SuspensionVelocityWheelsChartViewModel viewModel)
        {
            double maxY = Math.Max(flHistogram.MaximumY, Math.Max(frHistogram.MaximumY, Math.Max(rlHistogram.MaximumY, rrHistogram.MaximumY)));
            flHistogram.MaximumY = maxY;
            frHistogram.MaximumY = maxY;
            rlHistogram.MaximumY = maxY;
            rrHistogram.MaximumY = maxY;

            flHistogram.MinimumX = viewModel.Minimum;
            frHistogram.MinimumX = viewModel.Minimum;
            rlHistogram.MinimumX = viewModel.Minimum;
            rrHistogram.MinimumX = viewModel.Minimum;

            flHistogram.MaximumX = viewModel.Maximum;
            frHistogram.MaximumX = viewModel.Maximum;
            rlHistogram.MaximumX = viewModel.Maximum;
            rrHistogram.MaximumX = viewModel.Maximum;
        }

        protected override void BeforeHistogramFilling(SuspensionVelocityWheelsChartViewModel wheelsChart)
        {
            _suspensionVelocityFilter.MinimumVelocity =  wheelsChart.Minimum;
            _suspensionVelocityFilter.MaximumVelocity = wheelsChart.Maximum;
            _suspensionVelocityFilter.VelocityUnits = _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall;
            base.BeforeHistogramFilling(wheelsChart);
        }
    }
}