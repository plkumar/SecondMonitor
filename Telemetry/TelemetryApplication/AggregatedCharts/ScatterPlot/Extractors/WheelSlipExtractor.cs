namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Extractors
{
    using System;
    using DataModel.BasicProperties;
    using DataModel.Snapshot.Systems;
    using DataModel.Telemetry;
    using Filter;
    using SecondMonitor.ViewModels.Settings;

    public class WheelSlipExtractor : AbstractWheelScatterPlotDataExtractor
    {

        public WheelSlipExtractor(ISettingsProvider settingsProvider, ThrottlePositionFilter throttlePositionFilter, BrakePositionFilter brakePositionFilter) : base(settingsProvider, new ITelemetryFilter[]{throttlePositionFilter, brakePositionFilter})
        {
            ThrottlePositionFilter = throttlePositionFilter;
            BrakePositionFilter = brakePositionFilter;
        }

        public override string YUnit => string.Empty;
        public override string XUnit => Velocity.GetUnitSymbol(VelocityUnits);
        public override double XMajorTickSize => VelocityUnits == VelocityUnits.Mph ? 25 : Math.Round(Velocity.FromKph(25).GetValueInUnits(VelocityUnits), 0);
        public override double YMajorTickSize => 0.25;
        public ThrottlePositionFilter ThrottlePositionFilter { get; }
        public BrakePositionFilter BrakePositionFilter { get; }

        protected override double GetXWheelValue(WheelInfo wheelInfo, TimedTelemetrySnapshot snapshot)
        {
            return snapshot.PlayerData.Speed.GetValueInUnits(VelocityUnits);
        }

        protected override double GetYWheelValue(WheelInfo wheelInfo, TimedTelemetrySnapshot snapshot)
        {
            return wheelInfo.Slip;
        }
    }
}