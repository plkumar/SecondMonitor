namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Filter
{
    using System;
    using DataModel.BasicProperties;
    using DataModel.Snapshot.Systems;
    using DataModel.Telemetry;

    public class CamberFilter : IWheelTelemetryFilter
    {
        private Func<Wheels, WheelInfo> _wheelPicker;

        public AngleUnits AngleUnits { get; set; }
        public double MinimumCamber { get; set; }
        public double MaximumCamber { get; set; }

        public bool Accepts(TimedTelemetrySnapshot dataSet)
        {
            double camber = _wheelPicker(dataSet.PlayerData.CarInfo.WheelsInfo).Camber.GetValueInUnits(AngleUnits);
            return MinimumCamber <= camber && camber <= MaximumCamber;
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