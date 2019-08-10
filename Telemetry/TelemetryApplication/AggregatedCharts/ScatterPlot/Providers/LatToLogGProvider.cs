namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Controllers.Synchronization;
    using Extractors;
    using Filter;
    using SecondMonitor.ViewModels.Colors;
    using SecondMonitor.ViewModels.Colors.Extensions;
    using Settings.DTO;
    using TelemetryManagement.DTO;
    using ViewModels.AggregatedCharts;
    using ViewModels.AggregatedCharts.ScatterPlot;
    using ViewModels.LoadedLapCache;

    public class LatToLogGProvider : AbstractAggregatedChartProvider
    {
        private readonly LateralToLongGExtractor _dataExtractor;
        private readonly ThrottlePositionFilter _throttlePositionFilter;
        private readonly IDataPointSelectionSynchronization _dataPointSelectionSynchronization;
        private readonly List<ITelemetryFilter> _filters;

        public override string ChartName => "Traction Circle";

        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

        public LatToLogGProvider(ILoadedLapsCache loadedLapsCache, LateralToLongGExtractor dataExtractor, ThrottlePositionFilter throttlePositionFilter, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(loadedLapsCache)
        {
            _dataExtractor = dataExtractor;
            _throttlePositionFilter = throttlePositionFilter;
            _dataPointSelectionSynchronization = dataPointSelectionSynchronization;
            _filters = new List<ITelemetryFilter>() { throttlePositionFilter };
        }

        public override IReadOnlyCollection<IAggregatedChartViewModel> CreateAggregatedChartViewModels(AggregatedChartSettingsDto aggregatedChartSettings)
        {
            IEnumerable<IGrouping<int, LapTelemetryDto>> lapsStintGrouping = GetLapsGrouped(aggregatedChartSettings);
            if (aggregatedChartSettings.StintRenderingKind == StintRenderingKind.SingleChart)
            {
                return CreateChartForAllStints(lapsStintGrouping);
            }
            else
            {
                return CreateChartForEachStint(lapsStintGrouping, aggregatedChartSettings);
            }

        }

        private IReadOnlyCollection<IAggregatedChartViewModel> CreateChartForAllStints(IEnumerable<IGrouping<int, LapTelemetryDto>> lapsStintGrouping)
        {
            IColorPaletteProvider colorPaletteProvider = new BasicColorPaletteProvider();
            List<IAggregatedChartViewModel> charts = new List<IAggregatedChartViewModel>();
            IEnumerable<IGrouping<int, LapTelemetryDto>> lapsInStints = lapsStintGrouping as IGrouping<int, LapTelemetryDto>[] ?? lapsStintGrouping.ToArray();

            string title = BuildTitleForAllStints(lapsInStints);
            double maxG = 0;

            AxisDefinition xAxis = new AxisDefinition(_dataExtractor.XMajorTickSize, _dataExtractor.XMajorTickSize / 4, _dataExtractor.XUnit, "Lat Acc");
            AxisDefinition yAxis = new AxisDefinition(_dataExtractor.YMajorTickSize, _dataExtractor.YMajorTickSize / 4, _dataExtractor.YUnit, "Long Acc");
            ScatterPlot scatterPlot = new ScatterPlot(title, xAxis, yAxis);

            foreach (IGrouping<int, LapTelemetryDto> lapsInStint in lapsInStints)
            {
                string seriesTitle = $"Laps: {string.Join(", ", lapsInStint.Select(x => x.LapSummary.CustomDisplayName))} - Stint: {lapsInStint.Key}";
                ScatterPlotSeries newSeries = _dataExtractor.ExtractSeries(lapsInStint, Enumerable.Empty<ITelemetryFilter>().ToList(), seriesTitle, colorPaletteProvider.GetNext().ToOxyColor());
                scatterPlot.AddScatterPlotSeries(newSeries);
                maxG = Math.Max(maxG, newSeries.DataPoints.Max(x => Math.Abs(x.Y)));
            }

            SetAxisRanges(maxG, xAxis, yAxis);
            ScatterPlotChartViewModel viewModel = new ScatterPlotChartViewModel(_dataPointSelectionSynchronization) {Title = "Lateral / Longitudinal G"};
            viewModel.FromModel(scatterPlot);
            charts.Add(viewModel);
            return charts;
        }

        private IReadOnlyCollection<IAggregatedChartViewModel> CreateChartForEachStint(IEnumerable<IGrouping<int, LapTelemetryDto>> lapsStintGrouping, AggregatedChartSettingsDto aggregatedChartSettings)
        {
            IColorPaletteProvider colorPaletteProvider = new RedGreenGradientPalette();
            List<IAggregatedChartViewModel> charts = new List<IAggregatedChartViewModel>();
            foreach (IGrouping<int, LapTelemetryDto> lapsInStint in lapsStintGrouping)
            {
                string title = BuildChartTitle(lapsInStint, aggregatedChartSettings);
                double maxG = 0;
                AxisDefinition xAxis = new AxisDefinition(_dataExtractor.XMajorTickSize, _dataExtractor.XMajorTickSize / 4, _dataExtractor.XUnit, "Lat Acc");
                AxisDefinition yAxis = new AxisDefinition(_dataExtractor.YMajorTickSize, _dataExtractor.YMajorTickSize / 4, _dataExtractor.YUnit, "Long Acc");
                ScatterPlot scatterPlot = new ScatterPlot(title, xAxis, yAxis);
                ScatterPlotSeries newSeries;
                double throttlePortion = 1.0 / colorPaletteProvider.PaletteSize;

                for (int i = 0; i < colorPaletteProvider.PaletteSize - 1; i++)
                {
                    _throttlePositionFilter.Minimum = i * throttlePortion;
                    _throttlePositionFilter.Maximum = (i + 1) * throttlePortion;
                    string seriesTitle = $"Throttle - {i * throttlePortion * 100:F2}% - {(i + 1) * throttlePortion * 100:F2}%";
                    newSeries = _dataExtractor.ExtractSeries(lapsInStint, _filters, seriesTitle, colorPaletteProvider.GetNext().ToOxyColor());
                    if (newSeries == null)
                    {
                        continue;
                    }
                    scatterPlot.AddScatterPlotSeries(newSeries);
                    maxG = Math.Max(maxG, newSeries.DataPoints.Max(x => Math.Abs(x.Y)));
                }


                _throttlePositionFilter.Minimum = 1;
                _throttlePositionFilter.Maximum = double.MaxValue;
                newSeries = _dataExtractor.ExtractSeries(lapsInStint, _filters, "Throttle - 100%", colorPaletteProvider.GetNext().ToOxyColor());
                maxG = Math.Max(maxG, newSeries.DataPoints.Max(x => Math.Abs(x.Y)));
                scatterPlot.AddScatterPlotSeries(newSeries);

                SetAxisRanges(maxG, xAxis, yAxis);
                ScatterPlotChartViewModel viewModel = new ScatterPlotChartViewModel(_dataPointSelectionSynchronization) { Title = "Lateral / Longitudinal G" };
                viewModel.FromModel(scatterPlot);
                charts.Add(viewModel);
            }

            return charts;
        }

        private void SetAxisRanges(double maximumG, AxisDefinition xAxis, AxisDefinition yAxis)
        {
            double maximum = Math.Ceiling(maximumG);
            xAxis.UseCustomRange = true;
            yAxis.UseCustomRange = true;
            xAxis.Minimum = -maximum;
            xAxis.Maximum = maximum;
            yAxis.Minimum = -maximum;
            yAxis.Maximum = maximum;
        }
    }
}