namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Filter
{
    using DataModel.Telemetry;

    public class InGearFilter : ITelemetryFilter
    {
        public bool Accepts(TimedTelemetrySnapshot dataSet)
        {
            return dataSet.PlayerData.CarInfo.CurrentGear != "N";
        }
    }
}