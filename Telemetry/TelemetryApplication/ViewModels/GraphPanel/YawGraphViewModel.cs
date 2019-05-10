namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.GraphPanel
{
    using System;
    using DataModel.BasicProperties;
    using DataModel.Calculators;
    using DataModel.Telemetry;
    using OxyPlot;

    public class YawGraphViewModel : AbstractAverageValueGraphViewModel
    {
        private readonly YawCalculator _yawCalculator;

        public YawGraphViewModel()
        {
            _yawCalculator = new YawCalculator();
        }

        public override string Title => "Yaw";

        protected override string YUnits => Angle.GetUnitsSymbol(AngleUnits);

        protected override double YTickInterval => 15;

        protected override bool CanYZoom => true;

        private double ValidityLimit => Angle.GetFromDegrees(90).GetValueInUnits(AngleUnits);

        protected override double GetYValue(TimedTelemetrySnapshot dp1, TimedTelemetrySnapshot dp2)
        {
            return _yawCalculator.CalculateYaw(dp1, dp2).GetValueInUnits(AngleUnits);
        }

        protected override bool IsValid(DataPoint oldPoint, DataPoint newPoint)
        {
            return Math.Abs(oldPoint.Y - newPoint.Y) < ValidityLimit;
        }
    }
}