namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts.Commands;
    using Extractors;
    using Filter;
    using SecondMonitor.ViewModels.Factory;
    using TelemetryManagement.DTO;
    using ViewModels.AggregatedCharts;
    using ViewModels.AggregatedCharts.Histogram;
    using ViewModels.LoadedLapCache;

    public abstract class AbstractWheelHistogramProvider<T, TX> : IAggregatedChartProvider where T : WheelsHistogramChartViewModel, new() where TX : HistogramChartViewModel
    {
        private readonly AbstractWheelHistogramDataExtractor _abstractWheelHistogramDataExtractor;
        private readonly ILoadedLapsCache _loadedLapsCache;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly List<IWheelTelemetryFilter> _filters;

        protected AbstractWheelHistogramProvider(AbstractWheelHistogramDataExtractor abstractWheelHistogramDataExtractor, ILoadedLapsCache loadedLapsCache, IViewModelFactory viewModelFactory, IEnumerable<IWheelTelemetryFilter> filters)
        {
            _abstractWheelHistogramDataExtractor = abstractWheelHistogramDataExtractor;
            _loadedLapsCache = loadedLapsCache;
            _viewModelFactory = viewModelFactory;
            _filters = filters.ToList();
        }

        public abstract string ChartName { get; }
        public abstract AggregatedChartKind Kind { get; }

        public virtual IAggregatedChartViewModel CreateAggregatedChartViewModel()
        {
            List<LapTelemetryDto> loadedLaps = _loadedLapsCache.LoadedLaps.ToList();
            string title = $"{ChartName} - Laps: {string.Join(", ", loadedLaps.Select(x => x.LapSummary.CustomDisplayName))}";

            T wheelsHistogram = _viewModelFactory.Create<T>();

            wheelsHistogram.Title = title;
            wheelsHistogram.BandSize = _abstractWheelHistogramDataExtractor.DefaultBandSize;
            wheelsHistogram.Unit = _abstractWheelHistogramDataExtractor.YUnit;

            wheelsHistogram.RefreshCommand = new RelayCommand(() => FillHistogramViewmodel(loadedLaps, wheelsHistogram.BandSize, wheelsHistogram));
            OnNewViewModel(wheelsHistogram);

            FillHistogramViewmodel(loadedLaps, _abstractWheelHistogramDataExtractor.DefaultBandSize, wheelsHistogram);

            return wheelsHistogram;
        }

        protected abstract void OnNewViewModel(T newViewModel);


        protected void FillHistogramViewmodel(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, T wheelsChart)
        {
            BeforeHistogramFilling(wheelsChart);
            _filters.ForEach(x => x.FilterFrontLeft());
            Histogram flHistogram = _abstractWheelHistogramDataExtractor.ExtractHistogramFrontLeft(loadedLaps, bandSize, _filters);
            _filters.ForEach(x => x.FilterFrontRight());
            Histogram frHistogram = _abstractWheelHistogramDataExtractor.ExtractHistogramFrontRight(loadedLaps, bandSize, _filters);
            _filters.ForEach(x => x.FilterRearLeft());
            Histogram rlHistogram = _abstractWheelHistogramDataExtractor.ExtractHistogramRearLeft(loadedLaps, bandSize, _filters);
            _filters.ForEach(x => x.FilterRearRight());
            Histogram rrHistogram = _abstractWheelHistogramDataExtractor.ExtractHistogramRearRight(loadedLaps, bandSize, _filters);

            ApplyHistogramLimits(flHistogram, frHistogram, rlHistogram, rrHistogram, wheelsChart);

            TX flViewModel = _viewModelFactory.Create<TX>();
            flViewModel.FromModel(flHistogram);

            TX frViewModel = _viewModelFactory.Create<TX>();
            frViewModel.FromModel(frHistogram);

            TX rlViewModel = _viewModelFactory.Create<TX>();
            rlViewModel.FromModel(rlHistogram);

            TX rrViewModel = _viewModelFactory.Create<TX>();
            rrViewModel.FromModel(rrHistogram);

            wheelsChart.FrontLeftChartViewModel = flViewModel;
            wheelsChart.FrontRightChartViewModel = frViewModel;
            wheelsChart.RearLeftChartViewModel = rlViewModel;
            wheelsChart.RearRightChartViewModel = rrViewModel;
        }

        protected virtual void BeforeHistogramFilling(T wheelsChart)
        {
        }

        protected virtual void ApplyHistogramLimits(Histogram flHistogram, Histogram frHistogram, Histogram rlHistogram, Histogram rrHistogram, T viewModel)
        {
            double maxY = Math.Max(flHistogram.MaximumY, Math.Max(frHistogram.MaximumY, Math.Max(rlHistogram.MaximumY, rrHistogram.MaximumY)));
            flHistogram.MaximumY = maxY;
            frHistogram.MaximumY = maxY;
            rlHistogram.MaximumY = maxY;
            rrHistogram.MaximumY = maxY;

            double maxX = Math.Max(flHistogram.MaximumX, Math.Max(frHistogram.MaximumX, Math.Max(rlHistogram.MaximumX, rrHistogram.MaximumX)));
            flHistogram.MaximumX = maxX;
            frHistogram.MaximumX = maxX;
            rlHistogram.MaximumX = maxX;
            rrHistogram.MaximumX = maxX;

            double minX = Math.Min(flHistogram.MinimumX, Math.Min(frHistogram.MinimumX, Math.Min(rlHistogram.MinimumX, rrHistogram.MinimumX)));
            flHistogram.MinimumX = minX;
            frHistogram.MinimumX = minX;
            rlHistogram.MinimumX = minX;
            rrHistogram.MinimumX = minX;
        }
    }
}