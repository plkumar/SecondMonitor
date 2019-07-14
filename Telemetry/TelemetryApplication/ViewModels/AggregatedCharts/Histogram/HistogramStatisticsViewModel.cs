namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.AggregatedCharts.Histogram
{
    using System.Collections.Generic;
    using System.Linq;
    using SecondMonitor.ViewModels;
    using TelemetryApplication.AggregatedCharts.Histogram;
    using TelemetryManagement.StoryBoard;

    public class HistogramStatisticsViewModel : AbstractViewModel<IEnumerable<HistogramBar>>
    {
        private double _meanValue;
        private double _variance;
        private double _medianValue;

        public double MeanValue
        {
            get => _meanValue;
            set => SetProperty(ref _meanValue, value);
        }

        public double Variance
        {
            get => _variance;
            set => SetProperty(ref _variance, value);
        }

        public double MedianValue
        {
            get => _medianValue;
            set => SetProperty(ref _medianValue, value);
        }

        protected override void ApplyModel(IEnumerable<HistogramBar> model)
        {
            List<TimedValue> values = model.SelectMany(x => x.SourceValues).OrderBy(x => x.Value).ToList();
            MeanValue = values.Sum(x => x.Value * x.ValueTime.TotalSeconds) / values.Sum(x => x.ValueTime.TotalSeconds);
            MedianValue = values[values.Count / 2].Value;
            Variance = values[values.Count - 1].Value - values[0].Value;
        }

        public override IEnumerable<HistogramBar> SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}