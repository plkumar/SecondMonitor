namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.SettingsWindow
{
    using System.Collections.Generic;
    using SecondMonitor.ViewModels;
    using Settings.DTO;

    public interface IAggregatedChartSettingsViewModel : IViewModel<AggregatedChartSettingsDto>
    {
        List<string> AllowedStintRenderingKind { get; }

        string SelectedStintRenderingKind { get; }
    }
}