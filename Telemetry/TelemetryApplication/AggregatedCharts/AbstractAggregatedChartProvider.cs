namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts
{
    using System.Collections.Generic;
    using System.Linq;
    using Settings.DTO;
    using TelemetryManagement.DTO;
    using ViewModels.AggregatedCharts;
    using ViewModels.LoadedLapCache;

    public abstract class AbstractAggregatedChartProvider : IAggregatedChartProvider
    {
        private readonly ILoadedLapsCache _loadedLapsCache;

        protected AbstractAggregatedChartProvider(ILoadedLapsCache loadedLapsCache)
        {
            _loadedLapsCache = loadedLapsCache;
        }

        public abstract string ChartName { get; }

        public abstract AggregatedChartKind Kind { get; }


        public abstract IReadOnlyCollection<IAggregatedChartViewModel> CreateAggregatedChartViewModels(AggregatedChartSettingsDto aggregatedChartSettings);

        protected IEnumerable<IGrouping<int, LapTelemetryDto>> GetLapsGrouped(AggregatedChartSettingsDto aggregatedChartSettingsDto)
        {
            return aggregatedChartSettingsDto.StintRenderingKind == StintRenderingKind.None ? _loadedLapsCache.LoadedLaps.GroupBy(x => 0) : _loadedLapsCache.LoadedLaps.GroupBy(x => x.LapSummary.Stint);
        }

        protected string BuildChartTitle(IGrouping<int, LapTelemetryDto> lapGrouping, AggregatedChartSettingsDto aggregatedChartSettings)
        {
            if (aggregatedChartSettings.StintRenderingKind == StintRenderingKind.None)
            {
                return $"{ChartName} - Laps: {string.Join(", ", lapGrouping.Select(x => x.LapSummary.CustomDisplayName))}";
            }
            else
            {
                return $"{ChartName} - Laps: {string.Join(", ", lapGrouping.Select(x => x.LapSummary.CustomDisplayName))} - Stint: {lapGrouping.Key}";
            }
        }

        protected string BuildSeriesTitle(IGrouping<int, LapTelemetryDto> lapGrouping, AggregatedChartSettingsDto aggregatedChartSettings)
        {
            if (aggregatedChartSettings.StintRenderingKind == StintRenderingKind.None)
            {
                return $"Laps: {string.Join(", ", lapGrouping.Select(x => x.LapSummary.CustomDisplayName))}";
            }
            else
            {
                return $"Laps: {string.Join(", ", lapGrouping.Select(x => x.LapSummary.CustomDisplayName))} - Stint: {lapGrouping.Key}";
            }
        }

        protected string BuildTitleForAllStints(IEnumerable<IGrouping<int, LapTelemetryDto>> lapsInStints)
        {
            return $"{ChartName} - Stints: {string.Join(", ", lapsInStints.Select(x => x.Key).OrderBy(x => x))}";
        }

        protected static string BuildSeriesTitle(IGrouping<int, LapTelemetryDto> lapsInStint)
        {
            return $"Laps: {string.Join(", ", lapsInStint.Select(x => x.LapSummary.CustomDisplayName))} - Stint: {lapsInStint.Key}";
        }
    }
}