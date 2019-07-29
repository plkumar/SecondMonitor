namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Extractors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.BasicProperties;
    using Filter;
    using SecondMonitor.ViewModels.Properties;
    using SecondMonitor.ViewModels.Settings;
    using TelemetryManagement.DTO;

    public class SpeedHistogramExtractor : AbstractHistogramDataExtractor
    {
        public SpeedHistogramExtractor(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        protected override bool ZeroBandInMiddle => false;
        public override string YUnit => Velocity.GetUnitSymbol(VelocityUnits);
        public override double DefaultBandSize => VelocityUnits == VelocityUnits.Mph ? 10 : Math.Round(Velocity.FromKph(10).GetValueInUnits(VelocityUnits), 0);

        public Histogram ExtractHistogram(IEnumerable<LapTelemetryDto> loadedLaps, [CanBeNull] IReadOnlyCollection<ITelemetryFilter> filters, double bandSize, string title)
        {
            var histogram = ExtractHistogram(loadedLaps, x => x.PlayerData.Speed.GetValueInUnits(VelocityUnits), filters, bandSize, title);
            histogram.UseCustomXRange = true;
            histogram.MinimumX = 0;
            histogram.MaximumX = histogram.Items.SelectMany(x => x.Items).Max(x => x.Category);
            return histogram;
        }


    }
}