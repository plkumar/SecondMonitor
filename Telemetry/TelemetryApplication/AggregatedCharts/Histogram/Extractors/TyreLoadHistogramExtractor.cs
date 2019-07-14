namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Extractors
{
    using System;
    using DataModel.BasicProperties;
    using DataModel.Snapshot.Systems;
    using SecondMonitor.ViewModels.Settings;

    public class TyreLoadHistogramExtractor : AbstractWheelHistogramDataExtractor
    {
        public TyreLoadHistogramExtractor(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        protected override bool ZeroBandInMiddle => true;
        public override string YUnit => Force.GetUnitSymbol(ForceUnits);
        public override double DefaultBandSize => Math.Round(Force.GetFromNewtons(100).GetValueInUnits(ForceUnits));
        protected override Func<WheelInfo, double> WheelValueExtractor => GetTyreLoad;

        private double GetTyreLoad(WheelInfo wheelInfo)
        {
            return wheelInfo.TyreLoad.GetValueInUnits(ForceUnits);
        }
    }
}