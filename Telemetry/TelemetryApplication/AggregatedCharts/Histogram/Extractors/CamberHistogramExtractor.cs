namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Extractors
{
    using System;
    using DataModel.BasicProperties;
    using DataModel.Snapshot.Systems;
    using SecondMonitor.ViewModels.Settings;

    public class CamberHistogramExtractor : AbstractWheelHistogramDataExtractor
    {
        public CamberHistogramExtractor(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        public double IdealCamber { get; set; } = 0;

        protected override bool ZeroBandInMiddle => false;
        public override string YUnit => Angle.GetUnitsSymbol(AngleUnits);
        public override double DefaultBandSize => Angle.GetFromDegrees(0.10).GetValueInUnits(AngleUnits);
        protected override Func<WheelInfo, double> WheelValueExtractor => GetCamber;

        private double GetCamber(WheelInfo wheelInfo)
        {
            return -IdealCamber + wheelInfo.Camber.GetValueInUnits(AngleUnits);
        }
    }
}