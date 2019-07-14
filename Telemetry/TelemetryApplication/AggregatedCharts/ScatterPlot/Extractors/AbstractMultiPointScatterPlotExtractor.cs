namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Extractors
{
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.Extensions;
    using DataModel.Telemetry;
    using Filter;
    using OxyPlot;
    using SecondMonitor.ViewModels.Settings;
    using TelemetryManagement.DTO;

    public abstract class AbstractMultiPointScatterPlotExtractor : AbstractScatterPlotExtractor
    {
        protected AbstractMultiPointScatterPlotExtractor(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
        }

        public ScatterPlotSeries ExtractMultiPointSeries(IEnumerable<LapTelemetryDto> loadedLaps, IReadOnlyCollection<ITelemetryFilter> filters, string seriesTitle, OxyColor color)
        {
            TimedTelemetrySnapshot[] timedTelemetrySnapshots = loadedLaps.SelectMany(x => x.DataPoints).Where(x => filters.All(y => y.Accepts(x))).ToArray();

            if (timedTelemetrySnapshots.Length == 0)
            {
                return null;
            }

            ScatterPlotSeries newSeries = new ScatterPlotSeries(color, seriesTitle);
            timedTelemetrySnapshots.ForEach(x => newSeries.AddDataPoints(GetDataPoints(x)));
            return newSeries;
        }

        protected abstract IEnumerable<(double x, double y)> GetDataPoints(TimedTelemetrySnapshot snapshot);
    }
}