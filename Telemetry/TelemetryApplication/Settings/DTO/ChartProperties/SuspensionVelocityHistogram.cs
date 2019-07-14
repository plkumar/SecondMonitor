namespace SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO.ChartProperties
{
    using DataModel.BasicProperties;

    public class SuspensionVelocityHistogram
    {
        public SuspensionVelocityHistogram()
        {
            Minimum = Velocity.FromMs(-0.2);
            Maximum = Velocity.FromMs(0.2);
            BandSize = Velocity.FromMs(0.005);
        }

        public Velocity Minimum { get; set; }

        public Velocity Maximum { get; set; }

        public Velocity BandSize { get; set; }
    }
}