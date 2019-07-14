namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Extractors;
    using Filter;
    using SecondMonitor.ViewModels.Colors;
    using SecondMonitor.ViewModels.Colors.Extensions;
    using Settings.DTO;
    using TelemetryManagement.DTO;
    using ViewModels.AggregatedCharts;
    using ViewModels.AggregatedCharts.ScatterPlot;
    using ViewModels.LoadedLapCache;

    public abstract class AbstractGearsChartProvider : AbstractAggregatedChartProvider
    {
        private readonly AbstractGearFilteredScatterPlotExtractor _dataExtractor;

        protected AbstractGearsChartProvider(ILoadedLapsCache loadedLapsCache, AbstractGearFilteredScatterPlotExtractor dataExtractor) : base(loadedLapsCache)
        {
            _dataExtractor = dataExtractor;
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
            IGrouping<int, LapTelemetryDto>[] lapsInStints = lapsStintGrouping as IGrouping<int, LapTelemetryDto>[] ?? lapsStintGrouping.ToArray();
            string title = BuildTitleForAllStints(lapsInStints);
            int maxGear = lapsInStints.SelectMany(x => x).SelectMany(x => x.DataPoints).Where(x => !string.IsNullOrWhiteSpace(x.PlayerData.CarInfo.CurrentGear) && x.PlayerData.CarInfo.CurrentGear != "R" && x.PlayerData.CarInfo.CurrentGear != "N").Max(x => int.Parse(x.PlayerData.CarInfo.CurrentGear));

            CompositeAggregatedChartsViewModel viewModel = new CompositeAggregatedChartsViewModel() { Title = title };

            ScatterPlotChartViewModel mainViewModel = new ScatterPlotChartViewModel() { Title = "All Gear" };
            mainViewModel.FromModel(CreateScatterPlotAllGear(lapsInStints, maxGear));
            viewModel.MainAggregatedChartViewModel = mainViewModel;

            for (int i = 1; i <= maxGear; i++)
            {
                ScatterPlot scatterPlot = CreateScatterPlot(lapsInStints, i);
                if (scatterPlot.ScatterPlotSeries.Count == 0)
                {
                    continue;
                }

                ScatterPlotChartViewModel child = new ScatterPlotChartViewModel() { Title = $"Gear {i}" };
                child.FromModel(scatterPlot);
                viewModel.AddChildAggregatedChildViewModel(child);
            }

            return new List<IAggregatedChartViewModel>() { viewModel };
        }

        private IReadOnlyCollection<IAggregatedChartViewModel> CreateChartForEachStint(IEnumerable<IGrouping<int, LapTelemetryDto>> lapsStintGrouping, AggregatedChartSettingsDto aggregatedChartSettings)
        {
            List<IAggregatedChartViewModel> charts = new List<IAggregatedChartViewModel>();

            foreach (IGrouping<int, LapTelemetryDto> lapsGrouped in lapsStintGrouping)
            {

                string title = BuildChartTitle(lapsGrouped, aggregatedChartSettings);

                int maxGear = lapsGrouped.SelectMany(x => x.DataPoints).Where(x => !string.IsNullOrWhiteSpace(x.PlayerData.CarInfo.CurrentGear) && x.PlayerData.CarInfo.CurrentGear != "R" && x.PlayerData.CarInfo.CurrentGear != "N").Max(x => int.Parse(x.PlayerData.CarInfo.CurrentGear));

                CompositeAggregatedChartsViewModel viewModel = new CompositeAggregatedChartsViewModel() { Title = title };

                ScatterPlotChartViewModel mainViewModel = new ScatterPlotChartViewModel() { Title = "All Gear" };
                mainViewModel.FromModel(CreateScatterPlotAllGear(lapsGrouped, maxGear));

                viewModel.MainAggregatedChartViewModel = mainViewModel;

                for (int i = 1; i <= maxGear; i++)
                {
                    ScatterPlot scatterPlot = CreateScatterPlot(lapsGrouped, i);
                    if (scatterPlot.ScatterPlotSeries.Count == 0)
                    {
                        continue;
                    }

                    ScatterPlotChartViewModel child = new ScatterPlotChartViewModel() { Title = $"Gear {i}" };
                    child.FromModel(scatterPlot);
                    viewModel.AddChildAggregatedChildViewModel(child);
                }

                charts.Add(viewModel);
            }

            return charts;
        }


        protected ScatterPlot CreateScatterPlot(IEnumerable<LapTelemetryDto> loadedLaps, int gear)
        {
            AxisDefinition xAxis = new AxisDefinition(_dataExtractor.XMajorTickSize, _dataExtractor.XMajorTickSize / 5, _dataExtractor.XUnit);
            AxisDefinition yAxis = new AxisDefinition(_dataExtractor.YMajorTickSize, _dataExtractor.YMajorTickSize / 4, _dataExtractor.YUnit);
            ScatterPlot scatterPlot = new ScatterPlot($"Gear: {gear}", xAxis, yAxis);

            scatterPlot.AddScatterPlotSeries(_dataExtractor.ExtractSeriesForGear(loadedLaps, gear.ToString()));
            return scatterPlot;
        }

        protected ScatterPlot CreateScatterPlotAllGear(IEnumerable<LapTelemetryDto> loadedLaps, int maxGear)
        {
            AxisDefinition xAxis = new AxisDefinition(_dataExtractor.XMajorTickSize, _dataExtractor.XMajorTickSize / 5, _dataExtractor.XUnit);
            AxisDefinition yAxis = new AxisDefinition(_dataExtractor.YMajorTickSize, _dataExtractor.YMajorTickSize / 4, _dataExtractor.YUnit);
            ScatterPlot scatterPlot = new ScatterPlot("All Gears", xAxis, yAxis);

            for (int i = 1; i <= maxGear; i++)
            {
                scatterPlot.AddScatterPlotSeries(_dataExtractor.ExtractSeriesForGear(loadedLaps, i.ToString()));
            }

            return scatterPlot;

        }

        private ScatterPlot CreateScatterPlotAllGear(IGrouping<int, LapTelemetryDto>[] lapsInStints, int maxGear)
        {
            IColorPaletteProvider colorPaletteProvider = new BasicColorPaletteProvider();
            AxisDefinition xAxis = new AxisDefinition(_dataExtractor.XMajorTickSize, _dataExtractor.XMajorTickSize / 5, _dataExtractor.XUnit);
            AxisDefinition yAxis = new AxisDefinition(_dataExtractor.YMajorTickSize, _dataExtractor.YMajorTickSize / 4, _dataExtractor.YUnit);
            ScatterPlot scatterPlot = new ScatterPlot("All Gears", xAxis, yAxis);
            foreach (IGrouping<int, LapTelemetryDto> stint in lapsInStints)
            {
                string seriesTitle = $"Laps: {string.Join(", ", stint.Select(x => x.LapSummary.CustomDisplayName))} - Stint: {stint.Key}";
                scatterPlot.AddScatterPlotSeries(_dataExtractor.ExtractSeries(stint, new List<ITelemetryFilter>(), seriesTitle, colorPaletteProvider.GetNext().ToOxyColor()));
            }

            return scatterPlot;
        }

        private ScatterPlot CreateScatterPlot(IGrouping<int, LapTelemetryDto>[] lapsInStints, int gear)
        {
            IColorPaletteProvider colorPaletteProvider = new BasicColorPaletteProvider();
            AxisDefinition xAxis = new AxisDefinition(_dataExtractor.XMajorTickSize, _dataExtractor.XMajorTickSize / 5, _dataExtractor.XUnit);
            AxisDefinition yAxis = new AxisDefinition(_dataExtractor.YMajorTickSize, _dataExtractor.YMajorTickSize / 4, _dataExtractor.YUnit);
            ScatterPlot scatterPlot = new ScatterPlot($"Gear: {gear}", xAxis, yAxis);

            foreach (IGrouping<int, LapTelemetryDto> stint in lapsInStints)
            {
                string seriesTitle = $"Laps: {string.Join(", ", stint.Select(x => x.LapSummary.CustomDisplayName))} - Stint: {stint.Key}";
                scatterPlot.AddScatterPlotSeries(_dataExtractor.ExtractSeriesForGear(stint, gear.ToString(), seriesTitle, colorPaletteProvider.GetNext().ToOxyColor()));
            }


            return scatterPlot;
        }
    }
}