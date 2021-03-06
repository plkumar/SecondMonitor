﻿namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Extractors
{
    using System;
    using System.Collections.Generic;
    using DataModel.Snapshot.Systems;
    using DataModel.Telemetry;
    using Filter;
    using SecondMonitor.ViewModels.Settings;
    using TelemetryManagement.DTO;

    public abstract class AbstractWheelHistogramDataExtractor : AbstractHistogramDataExtractor
    {
        protected AbstractWheelHistogramDataExtractor(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        protected abstract Func<WheelInfo, double> WheelValueExtractor { get; }

        public Histogram ExtractHistogramFrontLeft(IEnumerable<LapTelemetryDto> loadedLaps, double bandSize, IReadOnlyCollection<ITelemetryFilter> filters)
        {
            double ExtractFunc(TimedTelemetrySnapshot x) => WheelValueExtractor(x.PlayerData.CarInfo.WheelsInfo.FrontLeft);
            return ExtractHistogram(loadedLaps, ExtractFunc, filters, bandSize, "Front Left");
        }

        public Histogram ExtractHistogramFrontRight(IEnumerable<LapTelemetryDto> loadedLaps, double bandSize, IReadOnlyCollection<ITelemetryFilter> filters)
        {
            double ExtractFunc(TimedTelemetrySnapshot x) => WheelValueExtractor(x.PlayerData.CarInfo.WheelsInfo.FrontRight);
            return ExtractHistogram(loadedLaps, ExtractFunc, filters, bandSize, "Front Right");
        }

        public Histogram ExtractHistogramRearLeft(IEnumerable<LapTelemetryDto> loadedLaps, double bandSize, IReadOnlyCollection<ITelemetryFilter> filters)
        {
            double ExtractFunc(TimedTelemetrySnapshot x) => WheelValueExtractor(x.PlayerData.CarInfo.WheelsInfo.RearLeft);
            return ExtractHistogram(loadedLaps, ExtractFunc, filters, bandSize, "Rear Left");
        }

        public Histogram ExtractHistogramRearRight(IEnumerable<LapTelemetryDto> loadedLaps, double bandSize, IReadOnlyCollection<ITelemetryFilter> filters)
        {
            double ExtractFunc(TimedTelemetrySnapshot x) => WheelValueExtractor(x.PlayerData.CarInfo.WheelsInfo.RearRight);
            return ExtractHistogram(loadedLaps, ExtractFunc, filters, bandSize, "Rear Right");
        }
    }
}