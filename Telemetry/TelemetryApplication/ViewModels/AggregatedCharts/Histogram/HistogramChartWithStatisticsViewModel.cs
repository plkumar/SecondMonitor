namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.AggregatedCharts.Histogram
{
    using System.Linq;
    using Controllers.Synchronization;
    using TelemetryApplication.AggregatedCharts.Histogram;

    public class HistogramChartWithStatisticsViewModel : HistogramChartViewModel
    {
        private HistogramStatisticsViewModel _histogramStatisticsViewModel;

        public HistogramChartWithStatisticsViewModel(IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(dataPointSelectionSynchronization)
        {
        }

        public HistogramStatisticsViewModel HistogramStatisticsViewModel
        {
            get => _histogramStatisticsViewModel;
            set => SetProperty(ref _histogramStatisticsViewModel, value);
        }

        protected override void ApplyModel(Histogram model)
        {
            HistogramStatisticsViewModel = new HistogramStatisticsViewModel();
            HistogramStatisticsViewModel.FromModel(model.Items.SelectMany(x => x.Items));
            base.ApplyModel(model);
        }
    }
}