namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Filter
{
    using System;
    using DataModel.Telemetry;

    public class LoadedWheelFilter : IWheelTelemetryFilter
    {

        public bool IncludeLoaded { get; set; } = true;

        public bool IncludeUnloaded { get; set; } = false;

        public double MinimumG { get; set; }
        public double MaximumG { get; set; }

        protected bool IsLoadedOnPositiveG { get; set; }

        public bool Accepts(TimedTelemetrySnapshot dataSet)
        {
            double gRaw = dataSet.PlayerData.CarInfo.Acceleration.XinG;
            double gAbs = Math.Abs(gRaw);
            bool isInRange = gAbs >= MinimumG && gAbs < MaximumG;
            bool isWheelLoad = (IsLoadedOnPositiveG && gRaw >= 0) || (!IsLoadedOnPositiveG && gRaw <= 0);
            return isInRange && ((isWheelLoad && IncludeLoaded) || (!isWheelLoad && IncludeUnloaded));
        }

        public void FilterFrontLeft()
        {
            IsLoadedOnPositiveG = false;
        }

        public void FilterFrontRight()
        {
            IsLoadedOnPositiveG = true;
        }

        public void FilterRearLeft()
        {
            IsLoadedOnPositiveG = false;
        }

        public void FilterRearRight()
        {
            IsLoadedOnPositiveG = true;
        }
    }
}