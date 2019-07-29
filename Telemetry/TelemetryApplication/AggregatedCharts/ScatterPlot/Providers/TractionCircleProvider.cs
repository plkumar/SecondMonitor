namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Histogram.Providers;
    using Settings.DTO;
    using ViewModels.AggregatedCharts;

    public class TractionCircleProvider : IAggregatedChartProvider
    {
        private readonly LatToLogGProvider _latToLogGProvider;
        private readonly SpeedToLatGProvider _speedToLatGProvider;
        private readonly SpeedToLongGProvider _speedToLongGProvider;
        private readonly SpeedInTurnsHistogramProvider _speedInTurnsHistogramProvider;
        public string ChartName => "Traction Circle";
        public AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

        public TractionCircleProvider(LatToLogGProvider latToLogGProvider, SpeedToLatGProvider speedToLatGProvider, SpeedToLongGProvider speedToLongGProvider, SpeedInTurnsHistogramProvider speedInTurnsHistogramProvider)
        {
            _latToLogGProvider = latToLogGProvider;
            _speedToLatGProvider = speedToLatGProvider;
            _speedToLatGProvider.IsLegendVisible = false;
            _speedToLongGProvider = speedToLongGProvider;
            _speedToLongGProvider.IsLegendVisible = false;
            _speedInTurnsHistogramProvider = speedInTurnsHistogramProvider;
        }

        public IReadOnlyCollection<IAggregatedChartViewModel> CreateAggregatedChartViewModels(AggregatedChartSettingsDto aggregatedChartSettings)
        {
            var histogramChartSettings = aggregatedChartSettings.StintRenderingKind == StintRenderingKind.SingleChart ? new AggregatedChartSettingsDto() : aggregatedChartSettings;
            var mainViewModels = _latToLogGProvider.CreateAggregatedChartViewModels(aggregatedChartSettings).ToList();
            var speedInTurnsModels = _speedInTurnsHistogramProvider.CreateAggregatedChartViewModels(histogramChartSettings).ToList();
            var latGViewModels = _speedToLatGProvider.CreateAggregatedChartViewModels(aggregatedChartSettings).ToList();
            var longGViewModels = _speedToLongGProvider.CreateAggregatedChartViewModels(aggregatedChartSettings).ToList();

            var compositeViewModels = new List<IAggregatedChartViewModel>();

            for (int i = 0; i < mainViewModels.Count; i++)
            {
                var newCompositeViewModel = new CompositeAggregatedChartsViewModel {Title = mainViewModels[i].Title, MainAggregatedChartViewModel = mainViewModels[i]};
                if (i < speedInTurnsModels.Count)
                {
                    newCompositeViewModel.AddChildAggregatedChildViewModel(speedInTurnsModels[i]);
                }


                if (i < latGViewModels.Count)
                {
                    newCompositeViewModel.AddChildAggregatedChildViewModel(latGViewModels[i]);
                }

                if (i < longGViewModels.Count)
                {
                    newCompositeViewModel.AddChildAggregatedChildViewModel(longGViewModels[i]);
                }

                compositeViewModels.Add(newCompositeViewModel);
            }

            return compositeViewModels;
        }
    }
}