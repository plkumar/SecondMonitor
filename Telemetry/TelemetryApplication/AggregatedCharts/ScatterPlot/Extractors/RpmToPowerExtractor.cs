namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Extractors
{
    using System;
    using DataModel.BasicProperties;
    using DataModel.BasicProperties.Units;
    using DataModel.Telemetry;
    using SecondMonitor.ViewModels.Settings;

    public class RpmToPowerExtractor : AbstractScatterPlotExtractor
    {
        public RpmToPowerExtractor(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        public override string YUnit => Power.GetUnitSymbol(PowerUnits);
        public override string XUnit => "RPM";
        public override double XMajorTickSize => 1000;
        public override double YMajorTickSize => PowerUnits == PowerUnits.HP ? 100 : Math.Round(Power.FromKw(100).GetValueInUnit(PowerUnits), 0);
        protected override double GetXValue(TimedTelemetrySnapshot snapshot)
        {
            return snapshot.PlayerData.CarInfo.EngineRpm;
        }

        protected override double GetYValue(TimedTelemetrySnapshot snapshot)
        {
            return snapshot.PlayerData.CarInfo.EnginePower.GetValueInUnit(PowerUnits);
        }
    }
}