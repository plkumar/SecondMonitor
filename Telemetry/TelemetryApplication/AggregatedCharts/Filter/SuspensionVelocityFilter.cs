namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Filter
{
    using System;
    using DataModel.BasicProperties;
    using DataModel.Snapshot.Systems;
    using DataModel.Telemetry;

    public class SuspensionVelocityFilter : IWheelTelemetryFilter
    {
        private Func<Wheels, WheelInfo> _wheelPicker;

        public VelocityUnits VelocityUnits { get; set; }
        public double LimitVelocity { get; set; }

        public bool Accepts(TimedTelemetrySnapshot dataSet)
        {
            return Math.Abs(_wheelPicker(dataSet.PlayerData.CarInfo.WheelsInfo).SuspensionVelocity.GetValueInUnits(VelocityUnits)) < LimitVelocity;
        }


        public void FilterFrontLeft()
        {
            _wheelPicker = x => x.FrontLeft;
        }

        public void FilterFrontRight()
        {
            _wheelPicker = x => x.FrontRight;
        }

        public void FilterRearLeft()
        {
            _wheelPicker = x => x.RearLeft;
        }

        public void FilterRearRight()
        {
            _wheelPicker = x => x.RearRight;
        }

    }
}