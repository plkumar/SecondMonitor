namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram
{
    using System.Linq;
    using TelemetryManagement.StoryBoard;

    public class HistogramBar
    {
        public HistogramBar(TimedValue[] sourceValues, double category, double percentage)
        {
            SourceValues = sourceValues;
            Category = category;
            Percentage = percentage;
            TotalDistinctPointsCount = sourceValues.SelectMany(x => x.BothPoints).Distinct().Count();
        }

        public TimedValue[] SourceValues { get; set; }

        public int TotalDistinctPointsCount { get; }

        public double Category { get; set; }

        public double Percentage { get; set; }


    }
}