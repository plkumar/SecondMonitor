namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.MapView
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using WindowsControls.WPF.DriverPosition;
    using Controllers.Synchronization;
    using DataModel.Snapshot.Drivers;
    using DataModel.Telemetry;
    using DataModel.TrackMap;
    using SecondMonitor.ViewModels;
    using TelemetryManagement.DTO;

    public interface IMapViewViewModel : IViewModel, IDisposable
    {
        FullMapControl SituationOverviewControl { get; }

        bool? ShowAllOverlays { get; set; }
        bool ShowBrakeOverlay { get; set; }
        bool ShowThrottleOverlay { get; set; }
        bool ShowClutchOverlay { get; set; }
        bool ShowShiftPoints { get; set; }
        bool ShowColoredSectors { get; set; }

        void LoadTrack(ITrackMap trackMapDto);

        void RemoveDriver(IDriverInfo driverInfo);
        void UpdateDrivers(params IDriverInfo[] driversInfo);
        Task AddPathsForLap(LapTelemetryDto lapTelemetry);
        void DeselectPoints(IEnumerable<TimedTelemetrySnapshot> points);
        void SelectPoints(IEnumerable<TimedTelemetrySnapshot> points);
        void RemovePathsForLap(LapSummaryDto lapTelemetry);

        void SelectTelemetryPointsInArea(Point pt1, Point pt2);
        void DeselectTelemetryPointsInArea(Point pt1, Point pt2);



    }
}