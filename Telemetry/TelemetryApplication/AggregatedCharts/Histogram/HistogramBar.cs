namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram
{
    using TelemetryManagement.StoryBoard;

    public class HistogramBar
    {
        public HistogramBar(TimedValue[] sourceValues, double category, double percentage)
        {
            SourceValues = sourceValues;
            Category = category;
            Percentage = percentage;
        }

        public TimedValue[] SourceValues { get; set; }

        public double Category { get; set; }

        public double Percentage { get; set; }


    }
}