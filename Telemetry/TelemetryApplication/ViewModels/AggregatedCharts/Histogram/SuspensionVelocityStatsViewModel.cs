namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.AggregatedCharts.Histogram
{
    using System.Collections.Generic;
    using System.Linq;
    using SecondMonitor.ViewModels;
    using TelemetryApplication.AggregatedCharts.Histogram;
    using TelemetryManagement.StoryBoard;

    public class SuspensionVelocityStatsViewModel : AbstractViewModel<IEnumerable<HistogramBar>>
    {
        private double _bumpAverageSpeed;
        private double _reboundAverageSpeed;
        private double _bumpPercentage;
        private double _reboundPercentage;
        private string _title;

        public double BumpAverageSpeed
        {
            get => _bumpAverageSpeed;
            set => SetProperty(ref _bumpAverageSpeed, value);
        }

        public double ReboundAverageSpeed
        {
            get => _reboundAverageSpeed;
            set => SetProperty(ref _reboundAverageSpeed, value);
        }

        public double BumpPercentage
        {
            get => _bumpPercentage;
            set => SetProperty(ref _bumpPercentage, value);
        }

        public double ReboundPercentage
        {
            get => _reboundPercentage;
            set => SetProperty(ref _reboundPercentage, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected override void ApplyModel(IEnumerable<HistogramBar> model)
        {
            List<HistogramBar> bumps = model.Where(x => x.Category > 0).ToList();
            List<HistogramBar> rebound = model.Where(x => x.Category < 0).ToList();
            BumpPercentage = bumps.Sum(x => x.Percentage);
            ReboundPercentage = rebound.Sum(x => x.Percentage);

            List<TimedValue> bumpValues = bumps.SelectMany(x => x.SourceValues).ToList();
            BumpAverageSpeed = bumpValues.Sum(x => x.Value * x.ValueTime.TotalSeconds) / bumpValues.Sum(x => x.ValueTime.TotalSeconds);

            List<TimedValue> reboundValues = rebound.SelectMany(x => x.SourceValues).ToList();
            ReboundAverageSpeed = reboundValues.Sum(x => x.Value * x.ValueTime.TotalSeconds) / reboundValues.Sum(x => x.ValueTime.TotalSeconds);
        }

        public override IEnumerable<HistogramBar> SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}