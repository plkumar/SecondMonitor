namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot
{
    public class AxisDefinition
    {
        public AxisDefinition(double majorTick, double minorTick, string unit, string title)
        {
            MajorTick = majorTick;
            MinorTick = minorTick;
            Unit = unit;
            Title = title;
        }

        public AxisDefinition(double majorTick, double minorTick, string unit) : this(majorTick, minorTick, unit, string.Empty)
        {

        }


        public double MajorTick { get; }
        public double MinorTick { get; }
        public string Unit { get; }
        public string Title { get; set; }
        public bool UseCustomRange { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }

        public void SetCustomRange(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
            UseCustomRange = true;
        }

    }
}