namespace SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO.ChartProperties
{
    public class ChartsProperties
    {
        public ChartsProperties()
        {
            SuspensionVelocityHistogram = new SuspensionVelocityHistogram();
            CamberHistogram = new CamberHistogram();
        }
        public SuspensionVelocityHistogram SuspensionVelocityHistogram { get; set; }

        public CamberHistogram CamberHistogram { get; set; }
    }
}