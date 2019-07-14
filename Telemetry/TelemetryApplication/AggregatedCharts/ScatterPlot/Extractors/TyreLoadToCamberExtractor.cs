namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Extractors
{
    using System;
    using System.Collections.Generic;
    using DataModel.BasicProperties;
    using DataModel.Snapshot.Systems;
    using DataModel.Telemetry;
    using Filter;
    using SecondMonitor.ViewModels.Settings;

    public class TyreLoadToCamberExtractor : AbstractWheelScatterPlotDataExtractor
    {
        public TyreLoadToCamberExtractor(ISettingsProvider settingsProvider, IEnumerable<ITelemetryFilter> filters) : base(settingsProvider, filters)
        {
        }

        public override string YUnit => Angle.GetUnitsSymbol(AngleUnits);
        public override string XUnit => Force.GetUnitSymbol(ForceUnits);
        public override double XMajorTickSize => Math.Round(Force.GetFromNewtons(1000).GetValueInUnits(ForceUnits));
        public override double YMajorTickSize => Angle.GetFromDegrees(1).GetValueInUnits(AngleUnits);
        protected override double GetXWheelValue(WheelInfo wheelInfo, TimedTelemetrySnapshot snapshot)
        {
            return wheelInfo.TyreLoad.GetValueInUnits(ForceUnits);
        }

        protected override double GetYWheelValue(WheelInfo wheelInfo, TimedTelemetrySnapshot snapshot)
        {
            return wheelInfo.Camber.GetValueInUnits(AngleUnits);
        }
    }
}