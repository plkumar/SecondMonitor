namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Filter
{
    using DataModel.Telemetry;

    public class PositiveTorqueFilter : ITelemetryFilter
    {
        public bool Accepts(TimedTelemetrySnapshot dataSet)
        {
            return dataSet.PlayerData.CarInfo.EngineTorque.InNm > 0;
        }
    }
}