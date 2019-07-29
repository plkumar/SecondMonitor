namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Extractors
{
    using System;
    using DataModel.BasicProperties.Units;
    using DataModel.Telemetry;
    using SecondMonitor.ViewModels.Settings;

    public class RpmToTorqueExtractor : AbstractScatterPlotExtractor
    {
        public RpmToTorqueExtractor(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        public override string YUnit => Torque.GetUnitSymbol(TorqueUnits);
        public override string XUnit => "RPM";
        public override double XMajorTickSize => 1000;
        public override double YMajorTickSize => TorqueUnits == TorqueUnits.lbf ? 200 :  Math.Round(Torque.FromNm(100).GetValueInUnit(TorqueUnits), 0);
        protected override double GetXValue(TimedTelemetrySnapshot snapshot)
        {
            return snapshot.PlayerData.CarInfo.EngineRpm;
        }

        protected override double GetYValue(TimedTelemetrySnapshot snapshot)
        {
            return snapshot.PlayerData.CarInfo.EngineTorque.GetValueInUnit(TorqueUnits);
        }
    }
}