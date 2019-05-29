namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram
{
    using System.Collections.Generic;
    using Providers;

    public class Histogram
    {
        private readonly List<HistogramBand> _histogramItems;

        public Histogram()
        {
            _histogramItems = new List<HistogramBand>();
        }

        public string Title { get; set; }
        public int DataPointsCount { get; set; }
        public IReadOnlyCollection<HistogramBand> Items => _histogramItems.AsReadOnly();
        public double BandSize { get; set; }
        public double MajorTickSize { get; set; }
        public string Unit { get; set; }
        public double MinimumX { get; set; }
        public double MaximumX { get; set; }
        public bool UseCustomXRange { get; set; }

        public bool UseCustomYRange { get; set; }
        public double MinimumY { get; set; }
        public double MaximumY { get; set; }

        public void AddItem(HistogramBand histogramBand)
        {
            _histogramItems.Add(histogramBand);
        }

        public void AddItems(IEnumerable<HistogramBand> histogramItems)
        {
            _histogramItems.AddRange(histogramItems);
        }
    }
}