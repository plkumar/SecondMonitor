namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Extractors
{
    using System;
    using System.Collections.Generic;
    using DataModel.BasicProperties;
    using DataModel.Snapshot.Systems;
    using DataModel.Telemetry;
    using Filter;
    using SecondMonitor.ViewModels.Settings;

    public class TyreLoadToLongGExtractor : AbstractWheelScatterPlotDataExtractor
    {
        public TyreLoadToLongGExtractor(ISettingsProvider settingsProvider, IEnumerable<ITelemetryFilter> filters) : base(settingsProvider, filters)
        {
        }

        public override string YUnit => Force.GetUnitSymbol(ForceUnits);
        public override string XUnit => "G";
        public override double XMajorTickSize => 1;
        public override double YMajorTickSize => Math.Round(Force.GetFromNewtons(1000).GetValueInUnits(ForceUnits));
        protected override double GetXWheelValue(WheelInfo wheelInfo, TimedTelemetrySnapshot snapshot)
        {
            return snapshot.PlayerData.CarInfo.Acceleration.ZinG;
        }

        protected override double GetYWheelValue(WheelInfo wheelInfo, TimedTelemetrySnapshot snapshot)
        {
            return wheelInfo.TyreLoad.GetValueInUnits(ForceUnits);
        }
    }
}