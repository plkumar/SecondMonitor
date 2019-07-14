namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Extractors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.Snapshot.Systems;
    using DataModel.Telemetry;
    using Filter;
    using OxyPlot;
    using SecondMonitor.ViewModels.Settings;
    using TelemetryManagement.DTO;

    public abstract class AbstractWheelScatterPlotDataExtractor : AbstractMultiPointScatterPlotExtractor
    {
        private static readonly OxyColor FrontLeftColor = OxyColor.Parse("#ff9900");
        private static readonly OxyColor FrontRightColor = OxyColor.Parse("#99ffcc");
        private static readonly OxyColor RearLeftColor = OxyColor.Parse("#0066ff");
        private static readonly OxyColor RearRightColor = OxyColor.Parse("#55aa00");

        private readonly List<ITelemetryFilter> _filters;

        private Func<TimedTelemetrySnapshot, WheelInfo> _activeWheelExtractor;

        protected AbstractWheelScatterPlotDataExtractor(ISettingsProvider settingsProvider, IEnumerable<ITelemetryFilter> filters) : base(settingsProvider)
        {
            _filters = filters.ToList();
        }

        public ScatterPlotSeries ExtractFrontLeft(IEnumerable<LapTelemetryDto> loadedLaps, string title, OxyColor color)
        {
            _activeWheelExtractor = ((x) => x.PlayerData.CarInfo.WheelsInfo.FrontLeft);
            return ExtractSeries(loadedLaps, _filters, title, color);
        }

        public ScatterPlotSeries ExtractFrontLeft(IEnumerable<LapTelemetryDto> loadedLaps)
        {
            return ExtractFrontLeft(loadedLaps, "Front Left", FrontLeftColor);
        }

        public ScatterPlotSeries ExtractFrontRight(IEnumerable<LapTelemetryDto> loadedLaps, string title, OxyColor color)
        {
            _activeWheelExtractor = ((x) => x.PlayerData.CarInfo.WheelsInfo.FrontRight);
            return ExtractSeries(loadedLaps, _filters, title, color);
        }

        public ScatterPlotSeries ExtractFrontRight(IEnumerable<LapTelemetryDto> loadedLaps)
        {
            return ExtractFrontRight(loadedLaps, "Front Right", FrontRightColor);
        }

        public ScatterPlotSeries ExtractRearLeft(IEnumerable<LapTelemetryDto> loadedLaps, string title, OxyColor color)
        {
            _activeWheelExtractor = ((x) => x.PlayerData.CarInfo.WheelsInfo.RearLeft);
            return ExtractSeries(loadedLaps, _filters, title, color);
        }

        public ScatterPlotSeries ExtractRearLeft(IEnumerable<LapTelemetryDto> loadedLaps)
        {
            return ExtractRearLeft(loadedLaps, "Rear Left", RearLeftColor);
        }

        public ScatterPlotSeries ExtractRearRight(IEnumerable<LapTelemetryDto> loadedLaps, string title, OxyColor color)
        {
            _activeWheelExtractor = ((x) => x.PlayerData.CarInfo.WheelsInfo.RearRight);
            return ExtractSeries(loadedLaps, _filters, title, color);
        }

        public ScatterPlotSeries ExtractRearRight(IEnumerable<LapTelemetryDto> loadedLaps)
        {
            return ExtractRearRight(loadedLaps, "Rear Right", RearRightColor);
        }

        protected override double GetXValue(TimedTelemetrySnapshot snapshot)
        {
            return GetXWheelValue(_activeWheelExtractor(snapshot), snapshot);
        }

        protected override double GetYValue(TimedTelemetrySnapshot snapshot)
        {
            return GetYWheelValue(_activeWheelExtractor(snapshot), snapshot);
        }

        protected abstract double GetXWheelValue(WheelInfo wheelInfo, TimedTelemetrySnapshot snapshot);

        protected abstract double GetYWheelValue(WheelInfo wheelInfo, TimedTelemetrySnapshot snapshot);

        protected override IEnumerable<(double x, double y)> GetDataPoints(TimedTelemetrySnapshot snapshot)
        {
            return snapshot.PlayerData.CarInfo.WheelsInfo.AllWheels.Select(x => (GetXWheelValue(x, snapshot), GetYWheelValue(x, snapshot)));
        }
    }
}