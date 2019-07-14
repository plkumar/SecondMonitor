namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.AggregatedCharts.Histogram
{
    public class CamberWheelsChartViewModel : WheelsHistogramChartViewModel
    {
        private bool _isLoadedChecked;
        private bool _isUnloadedChecked;
        private double _fromG;
        private double _toG;
        private double _fromCamber;
        private double _toCamber;

        public bool IsLoadedChecked
        {
            get => _isLoadedChecked;
            set => SetProperty(ref _isLoadedChecked, value);
        }

        public bool IsUnloadedChecked
        {
            get => _isUnloadedChecked;
            set => SetProperty(ref _isUnloadedChecked, value);
        }

        public double FromG
        {
            get => _fromG;
            set => SetProperty(ref _fromG, value);
        }

        public double ToG
        {
            get => _toG;
            set => SetProperty(ref _toG, value);
        }

        public double FromCamber
        {
            get => _fromCamber;
            set => SetProperty(ref _fromCamber, value);
        }

        public double ToCamber
        {
            get => _toCamber;
            set => SetProperty(ref _toCamber, value);
        }
    }
}