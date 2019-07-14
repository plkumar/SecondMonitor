namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Extractors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WindowsControls.Properties;
    using DataModel.BasicProperties;
    using DataModel.Snapshot.Systems;
    using DataModel.Telemetry;
    using Filter;
    using OxyPlot;
    using Providers;
    using SecondMonitor.ViewModels.Settings;
    using TelemetryManagement.DTO;
    using TelemetryManagement.StoryBoard;

    public class SuspensionVelocityHistogramDataExtractor : AbstractWheelHistogramDataExtractor
    {
        public SuspensionVelocityHistogramDataExtractor(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        protected override bool ZeroBandInMiddle => false;
        public override double DefaultBandSize => Math.Round(Velocity.FromMs(0.005).GetValueInUnits(VelocityUnitsSmall), 2);
        public override string YUnit => Velocity.GetUnitSymbol(VelocityUnitsSmall);

        public double BumpTransition { get; set; }

        public double ReboundTransition { get; set; }

        protected override Func<WheelInfo, double> WheelValueExtractor => (x) => x.SuspensionVelocity?.GetValueInUnits(VelocityUnitsSmall) ?? 0;

        protected override Histogram ExtractHistogram(IEnumerable<LapTelemetryDto> loadedLaps, Func<TimedTelemetrySnapshot, double> extractFunc, [CanBeNull] IReadOnlyCollection<ITelemetryFilter> filters, double bandSize, string title)
        {
            TimedValue[] data = ExtractTimedValuesOfLoadedLaps(loadedLaps, extractFunc, filters).Where(x => x.ValueTime.TotalSeconds < 2).OrderBy(x => x.Value).ToArray();
            if (data.Length == 0)
            {
                return new Histogram();
            }
            double minBand = GetBandMiddleValue(data[0].Value, bandSize);
            double maxBand = GetBandMiddleValue(data[data.Length - 1].Value, bandSize);
            double totalSeconds = data.Sum(x => x.ValueTime.TotalSeconds);
            List<IGrouping<double, TimedValue>> groupedByBand = data.GroupBy(x => GetBandMiddleValue(x.Value, bandSize)).ToList();

            Histogram histogram = new Histogram()
            {
                BandSize = bandSize,
                MajorTickSize = bandSize * 5,
                Title = title,
                Unit = YUnit,
                DataPointsCount = data.Length,
            };
            HistogramBand slowBand = new HistogramBand(OxyColors.Green) {Title = "Slow"};
            HistogramBand fastBand = new HistogramBand(OxyColors.DarkRed) { Title = "Fast" };
            histogram.AddItem(slowBand);
            histogram.AddItem(fastBand);

            for (double i = minBand; i <= maxBand; i += bandSize)
            {
                IGrouping<double, TimedValue> currentGrouping = groupedByBand.FirstOrDefault(x => Math.Abs(x.Key - i) < 0.0001);

                double bandTime = currentGrouping?.Sum(x => x.ValueTime.TotalSeconds) ?? 0;
                double percentage = bandTime / totalSeconds * 100;
                HistogramBar currentBar = new HistogramBar(currentGrouping?.ToArray() ?? new TimedValue[0], i, percentage);
                if (ReboundTransition < i && i < BumpTransition)
                {
                    slowBand.AddItem(currentBar);
                }
                else
                {
                    fastBand.AddItem(currentBar);
                }
            }

            histogram.UseCustomYRange = true;
            histogram.MaximumY = histogram.Items.SelectMany(x => x.Items).Max(x => x.Percentage);

            histogram.UseCustomXRange = true;
            histogram.MaximumX = histogram.Items.SelectMany(x => x.Items).Max(x => x.Category);
            histogram.MinimumX = histogram.Items.SelectMany(x => x.Items).Min(x => x.Category);

            return histogram;
        }
    }
}