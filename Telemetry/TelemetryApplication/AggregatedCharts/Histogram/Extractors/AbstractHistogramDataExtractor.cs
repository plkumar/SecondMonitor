﻿namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Extractors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WindowsControls.Properties;
    using DataModel.Telemetry;
    using Filter;
    using Providers;
    using SecondMonitor.ViewModels.Settings;
    using TelemetryManagement.DTO;
    using TelemetryManagement.StoryBoard;

    public abstract class AbstractHistogramDataExtractor : AbstractTelemetryDataExtractor
    {
        protected abstract bool ZeroBandInMiddle { get; }
        public abstract string YUnit { get; }
        public abstract double DefaultBandSize { get; }

        protected AbstractHistogramDataExtractor(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        protected Histogram ExtractHistogram(IEnumerable<LapTelemetryDto> loadedLaps, Func<TimedTelemetrySnapshot, double> extractFunc, double bandSize, string title)
        {
            return ExtractHistogram(loadedLaps, extractFunc, null, bandSize, title);
        }

        protected virtual Histogram ExtractHistogram(IEnumerable<LapTelemetryDto> loadedLaps, Func<TimedTelemetrySnapshot, double> extractFunc, [CanBeNull] IReadOnlyCollection<ITelemetryFilter> filters, double bandSize, string title)
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
            HistogramBand histogramBand = new HistogramBand();
            histogram.AddItem(histogramBand);

            for (double i = minBand; i <= maxBand; i += bandSize)
            {
                IGrouping<double, TimedValue> currentGrouping = groupedByBand.FirstOrDefault(x => Math.Abs(x.Key - i) < 0.0001);

                double bandTime = currentGrouping?.Sum(x => x.ValueTime.TotalSeconds) ?? 0;
                double percentage = bandTime / totalSeconds * 100;
                if (percentage < 0.25)
                {
                    continue;
                }
                HistogramBar currentBar = new HistogramBar(currentGrouping?.ToArray() ?? new TimedValue[0], i, percentage);
                histogramBand.AddItem(currentBar);
            }

            histogram.UseCustomYRange = true;
            histogram.MaximumY = histogram.Items.SelectMany(x => x.Items).Max(x => x.Percentage);

            histogram.UseCustomXRange = true;
            histogram.MaximumX = histogram.Items.SelectMany(x => x.Items).Max(x => x.Category);
            histogram.MinimumX = histogram.Items.SelectMany(x => x.Items).Min(x => x.Category);

            return histogram;
        }

        protected double GetBandMiddleValue(double value, double bandSize)
        {
            double coef = value < 0 ? -bandSize / 2 : bandSize / 2;
            //Computes the middle value of the band the value belongs to
            return ZeroBandInMiddle ? ((int) ((value + coef) / bandSize)) * bandSize : ((int)(value / bandSize)) * bandSize + coef;
        }
    }
}