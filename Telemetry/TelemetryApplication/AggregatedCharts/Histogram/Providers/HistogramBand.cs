namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using OxyPlot;

    public class HistogramBand
    {
        private readonly List<HistogramBar> _histogramBars;

        public HistogramBand() : this(Enumerable.Empty<HistogramBar>())
        {

        }

        public HistogramBand(OxyColor color) : this(color, Enumerable.Empty<HistogramBar>())
        {
        }

        public HistogramBand(IEnumerable<HistogramBar> bars) : this(OxyColors.Green, bars)
        {
        }

        public HistogramBand(OxyColor color, IEnumerable<HistogramBar> bars)
        {
            Color = color;
            _histogramBars = bars.ToList();
        }

        public IReadOnlyCollection<HistogramBar> Items => _histogramBars.AsReadOnly();

        public OxyColor Color { get; }

        public string Title { get; set; }

        public void AddItem(HistogramBar histogramBar)
        {
            _histogramBars.Add(histogramBar);
        }
    }
}