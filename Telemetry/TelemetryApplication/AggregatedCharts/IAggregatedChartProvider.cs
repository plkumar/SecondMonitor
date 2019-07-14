namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts
{
    using System.Collections.Generic;
    using Settings.DTO;
    using ViewModels.AggregatedCharts;

    public interface IAggregatedChartProvider
    {
        string ChartName { get; }
        AggregatedChartKind Kind { get; }

        IReadOnlyCollection<IAggregatedChartViewModel> CreateAggregatedChartViewModels(AggregatedChartSettingsDto aggregatedChartSettings);
    }
}