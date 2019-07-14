namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Extractors
{
    using System.Collections.Generic;
    using DataModel.BasicProperties;
    using DataModel.Snapshot.Systems;
    using DataModel.Telemetry;
    using Filter;
    using SecondMonitor.ViewModels.Settings;

    public class CamberToLateralGExtractor : AbstractWheelScatterPlotDataExtractor
    {
        public CamberToLateralGExtractor(ISettingsProvider settingsProvider, IEnumerable<ITelemetryFilter> filters) : base(settingsProvider, filters)
        {
        }

        public override string YUnit => Angle.GetUnitsSymbol(AngleUnits);

        public override string XUnit => "G";

        public override double XMajorTickSize => 0.5;

        public override double YMajorTickSize => Angle.GetFromDegrees(1).GetValueInUnits(AngleUnits);

        protected override double GetXWheelValue(WheelInfo wheelInfo, TimedTelemetrySnapshot snapshot)
        {
            return snapshot.PlayerData.CarInfo.Acceleration.XinG;
        }

        protected override double GetYWheelValue(WheelInfo wheelInfo, TimedTelemetrySnapshot snapshot)
        {
            return wheelInfo.Camber.GetValueInUnits(AngleUnits);
        }
    }
}