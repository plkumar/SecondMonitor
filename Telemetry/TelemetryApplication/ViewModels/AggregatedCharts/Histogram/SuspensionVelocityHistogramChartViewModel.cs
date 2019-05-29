namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.AggregatedCharts.Histogram
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Controllers.Synchronization;
    using TelemetryApplication.AggregatedCharts.Histogram;
    using TelemetryApplication.AggregatedCharts.Histogram.Providers;

    public class SuspensionVelocityHistogramChartViewModel : HistogramChartViewModel
    {
        public SuspensionVelocityHistogramChartViewModel(IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(dataPointSelectionSynchronization)
        {
            BandsStatistics = new ObservableCollection<SuspensionVelocityStatsViewModel>();
        }

        public ObservableCollection<SuspensionVelocityStatsViewModel> BandsStatistics { get; }

        protected override void ApplyModel(Histogram model)
        {
            BandsStatistics.Clear();
            List<HistogramBar> allBars = model.Items.SelectMany(x => x.Items).ToList();

            SuspensionVelocityStatsViewModel allSuspensionVelocityStatsViewModel = new SuspensionVelocityStatsViewModel() {Title = "All:"};
            allSuspensionVelocityStatsViewModel.FromModel(allBars);
            BandsStatistics.Add(allSuspensionVelocityStatsViewModel);

            foreach (HistogramBand histogramBand in model.Items)
            {
                var newStatsViewModel = new SuspensionVelocityStatsViewModel() {Title = histogramBand.Title + ":"};
                newStatsViewModel.FromModel(histogramBand.Items);
                BandsStatistics.Add(newStatsViewModel);
            }
            base.ApplyModel(model);
        }
    }
}