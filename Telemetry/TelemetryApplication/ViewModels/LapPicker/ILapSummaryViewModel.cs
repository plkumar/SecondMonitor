namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.LapPicker
{
    using System;
    using System.Windows.Media;
    using SecondMonitor.ViewModels;
    using TelemetryManagement.DTO;

    public interface ILapSummaryViewModel : IViewModel<LapSummaryDto>
    {
        string LapNumber { get; }
        TimeSpan LapTime { get; }
        TimeSpan Sector1Time { get; }
        TimeSpan Sector2Time { get; }
        TimeSpan Sector3Time { get; }
        bool Selected { get; set; }
        Color LapColor { get; set; }
        SolidColorBrush LapColorBrush { get; }

        int Stint { get; set; }

    }
}