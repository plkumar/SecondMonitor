namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Extractors
{
    using System;
    using DataModel.BasicProperties;
    using DataModel.Telemetry;
    using SecondMonitor.ViewModels.Settings;

    public class SpeedToLongGAllPointsExtractor : AbstractScatterPlotExtractor
    {
        public SpeedToLongGAllPointsExtractor(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        public override string YUnit => Velocity.GetUnitSymbol(VelocityUnits);
        public override string XUnit => "G";
        public override double YMajorTickSize => VelocityUnits == VelocityUnits.Mph ? 100 : Math.Round(Velocity.FromKph(100).GetValueInUnits(VelocityUnits),0);
        public override double XMajorTickSize => 1;
        protected override double GetXValue(TimedTelemetrySnapshot snapshot)
        {
            return snapshot.PlayerData.CarInfo.Acceleration.ZinG;
        }

        protected override double GetYValue(TimedTelemetrySnapshot snapshot)
        {
            return snapshot.PlayerData.Speed.GetValueInUnits(VelocityUnits);
        }
    }
}