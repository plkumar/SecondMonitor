namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts.Commands;
    using Extractors;
    using Filter;
    using SecondMonitor.ViewModels.Factory;
    using Settings.DTO;
    using TelemetryManagement.DTO;
    using ViewModels.AggregatedCharts;
    using ViewModels.AggregatedCharts.Histogram;
    using ViewModels.LoadedLapCache;

    public abstract class AbstractWheelHistogramProvider<T, TX> : AbstractAggregatedChartProvider where T : WheelsHistogramChartViewModel, new() where TX : HistogramChartViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;

        protected AbstractWheelHistogramProvider(AbstractWheelHistogramDataExtractor abstractWheelHistogramDataExtractor, ILoadedLapsCache loadedLapsCache, IViewModelFactory viewModelFactory, IEnumerable<IWheelTelemetryFilter> filters) : base(loadedLapsCache)
        {
            AbstractWheelHistogramDataExtractor = abstractWheelHistogramDataExtractor;
            _viewModelFactory = viewModelFactory;
            Filters = filters.ToList();
        }

        protected abstract bool ResetCommandVisible { get; }

        protected List<IWheelTelemetryFilter> Filters { get; }
        protected AbstractWheelHistogramDataExtractor AbstractWheelHistogramDataExtractor { get; }


        public override IReadOnlyCollection<IAggregatedChartViewModel> CreateAggregatedChartViewModels(AggregatedChartSettingsDto aggregatedChartSettings)
        {
            List<IAggregatedChartViewModel> charts = new List<IAggregatedChartViewModel>();
            var groupedByStint = GetLapsGrouped(aggregatedChartSettings);
            foreach (IGrouping<int, LapTelemetryDto> lapsInStint in groupedByStint)
            {
                string title = BuildChartTitle(lapsInStint, aggregatedChartSettings);

                T wheelsHistogram = _viewModelFactory.Create<T>();

                wheelsHistogram.Title = title;
                wheelsHistogram.BandSize = AbstractWheelHistogramDataExtractor.DefaultBandSize;
                wheelsHistogram.Unit = AbstractWheelHistogramDataExtractor.YUnit;

                TX flViewModel = _viewModelFactory.Create<TX>();
                TX frViewModel = _viewModelFactory.Create<TX>();
                TX rlViewModel = _viewModelFactory.Create<TX>();
                TX rrViewModel = _viewModelFactory.Create<TX>();

                wheelsHistogram.FrontLeftChartViewModel = flViewModel;
                wheelsHistogram.FrontRightChartViewModel = frViewModel;
                wheelsHistogram.RearLeftChartViewModel = rlViewModel;
                wheelsHistogram.RearRightChartViewModel = rrViewModel;

                OnNewViewModel(wheelsHistogram);
                wheelsHistogram.RefreshCommand = new RelayCommand(() => RefreshHistogram(lapsInStint.ToList(), wheelsHistogram.BandSize, wheelsHistogram));
                wheelsHistogram.ResetParametersCommand = new RelayCommand(() => ResetHistogramParameters(lapsInStint.ToList(), wheelsHistogram.BandSize, wheelsHistogram));
                wheelsHistogram.IsResetParametersCommandVisible = ResetCommandVisible;


                FillHistogramViewmodel(lapsInStint.ToList(), AbstractWheelHistogramDataExtractor.DefaultBandSize, wheelsHistogram);

                charts.Add(wheelsHistogram);
            }

            return charts;
        }

        protected virtual void  ResetHistogramParameters(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, T wheelsChart)
        {
            RefreshHistogram(loadedLaps, bandSize, wheelsChart);
        }

        protected abstract void OnNewViewModel(T newViewModel);

        protected virtual void RefreshHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, T wheelsChart)
        {
            FillHistogramViewmodel(loadedLaps, bandSize, wheelsChart);
        }

        protected void FillHistogramViewmodel(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, T wheelsChart)
        {
            BeforeHistogramFilling(wheelsChart);
            ExtractFlHistogram(loadedLaps, bandSize, wheelsChart);


            Histogram flHistogram = ExtractFlHistogram(loadedLaps, bandSize, wheelsChart);
            Histogram frHistogram = ExtractFrHistogram(loadedLaps, bandSize, wheelsChart);
            Histogram rlHistogram = ExtractRlHistogram(loadedLaps, bandSize, wheelsChart);
            Histogram rrHistogram = ExtractRrHistogram(loadedLaps, bandSize, wheelsChart);

            ApplyHistogramLimits(flHistogram, frHistogram, rlHistogram, rrHistogram, wheelsChart);

            ((TX)wheelsChart.FrontLeftChartViewModel).FromModel(flHistogram);
            ((TX)wheelsChart.FrontRightChartViewModel).FromModel(frHistogram);
            ((TX)wheelsChart.RearLeftChartViewModel).FromModel(rlHistogram);
            ((TX)wheelsChart.RearRightChartViewModel).FromModel(rrHistogram);
        }

        protected virtual Histogram ExtractFlHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, T wheelsChart)
        {
            Filters.ForEach(x => x.FilterFrontLeft());
            return AbstractWheelHistogramDataExtractor.ExtractHistogramFrontLeft(loadedLaps, bandSize, Filters);
        }

        protected virtual Histogram ExtractFrHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, T wheelsChart)
        {
            Filters.ForEach(x => x.FilterFrontRight());
            return AbstractWheelHistogramDataExtractor.ExtractHistogramFrontRight(loadedLaps, bandSize, Filters);
        }

        protected virtual Histogram ExtractRlHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, T wheelsChart)
        {
            Filters.ForEach(x => x.FilterRearLeft());
            return AbstractWheelHistogramDataExtractor.ExtractHistogramRearLeft(loadedLaps, bandSize, Filters);
        }

        protected virtual Histogram ExtractRrHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, T wheelsChart)
        {
            Filters.ForEach(x => x.FilterRearRight());
            return AbstractWheelHistogramDataExtractor.ExtractHistogramRearRight(loadedLaps, bandSize, Filters);
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