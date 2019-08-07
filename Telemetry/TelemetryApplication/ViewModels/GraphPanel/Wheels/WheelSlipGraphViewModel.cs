namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.GraphPanel.Wheels
{
    using System;
    using System.Collections.Generic;
    using DataModel.Snapshot.Systems;
    using DataModel.Telemetry;
    using OxyPlot;
    using OxyPlot.Series;
    using TelemetryManagement.DTO;

    public class WheelSlipGraphViewModel : AbstractWheelsGraphViewModel
    {
        public override string Title => "Wheel Slip";
        protected override string YUnits => "";
        protected override double YTickInterval => 0.5;
        protected override bool CanYZoom => true;
        protected override Func<WheelInfo, double> ExtractorFunction => (x) => x.Slip;

        protected override List<LineSeries> GetLineSeries(LapSummaryDto lapSummary, List<TimedTelemetrySnapshot> dataPoints, OxyColor color)
        {
            var lineSeries = base.GetLineSeries(lapSummary, dataPoints, color);
            YMaximum = 0.5;
            YMinimum = -0.5;
            return lineSeries;
        }
    }
}