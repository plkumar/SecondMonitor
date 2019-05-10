namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.GraphPanel
{
    using System.Collections.Generic;
    using DataExtractor;
    using DataModel.Telemetry;

    public class XGraphViewModel : AbstractSingleSeriesGraphViewModel
    {
        private string _title;
        private string _yUnits;
        private double _yTickInterval;
        private bool _canYZoom;

        public XGraphViewModel(IEnumerable<ISingleSeriesDataExtractor> dataExtractors) : base(dataExtractors)
        {
        }

        public override string Title => "X";

        protected override string YUnits => "m";

        protected override double YTickInterval => 250;

        protected override bool CanYZoom => true;

        protected override double GetYValue(TimedTelemetrySnapshot value)
        {
            return value.PlayerData.WorldPosition.X.InMeters;
        }
    }
}