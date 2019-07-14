namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.AggregatedCharts.Histogram
{
    using System.Linq;
    using Controllers.Synchronization;
    using TelemetryApplication.AggregatedCharts.Histogram;

    public class CamberHistogramChartViewModel : HistogramChartViewModel
    {
        private double _idealCamber;
        private CamberStatsViewModel _camberStatsViewModel;

        public CamberHistogramChartViewModel(IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(dataPointSelectionSynchronization)
        {
        }

        public double IdealCamber
        {
            get => _idealCamber;
            set => SetProperty(ref _idealCamber, value);
        }

        public CamberStatsViewModel CamberStatsViewModel
        {
            get => _camberStatsViewModel;
            set => SetProperty(ref _camberStatsViewModel, value);
        }

        public string AngleUnits { get; set; }

        protected override void ApplyModel(Histogram model)
        {
            base.ApplyModel(model);
            CamberStatsViewModel = new CamberStatsViewModel()
            {
                Title = "Statistics:"
            };
            CamberStatsViewModel.FromModel(model.Items.SelectMany(x => x.Items));
        }
    }
}