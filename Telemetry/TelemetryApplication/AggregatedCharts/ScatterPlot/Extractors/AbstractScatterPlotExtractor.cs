﻿namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Extractors
{
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.Extensions;
    using DataModel.Telemetry;
    using Filter;
    using OxyPlot;
    using SecondMonitor.ViewModels.Settings;
    using TelemetryManagement.DTO;

    public abstract class AbstractScatterPlotExtractor : AbstractTelemetryDataExtractor
    {
        protected AbstractScatterPlotExtractor(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        public abstract string YUnit { get; }
        public abstract string XUnit { get; }
        public abstract double XMajorTickSize { get; }
        public abstract double YMajorTickSize { get; }

        public ScatterPlotSeries ExtractSeries(IEnumerable<LapTelemetryDto> loadedLaps, string seriesTitle, OxyColor color)
        {
            return ExtractSeries(loadedLaps, Enumerable.Empty<ITelemetryFilter>(), seriesTitle, color);
        }

        public ScatterPlotSeries ExtractSeries(IEnumerable<LapTelemetryDto> loadedLaps, IEnumerable<ITelemetryFilter> filters, string seriesTitle, OxyColor color)
        {
            TimedTelemetrySnapshot[] timedTelemetrySnapshots = loadedLaps.SelectMany(x => x.DataPoints).Where(x => filters.All(y => y.Accepts(x))).ToArray();

            if (timedTelemetrySnapshots.Length == 0)
            {
                return null;
            }

            ScatterPlotSeries newSeries = new ScatterPlotSeries(color, seriesTitle);
            timedTelemetrySnapshots.ForEach(x => newSeries.AddDataPoint(GetXValue(x), GetYValue(x), x));
            return newSeries;
        }

        protected abstract double GetXValue(TimedTelemetrySnapshot snapshot);
        protected abstract double GetYValue(TimedTelemetrySnapshot snapshot);

    }
}