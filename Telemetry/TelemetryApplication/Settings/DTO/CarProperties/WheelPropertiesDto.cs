namespace SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO.CarProperties
{
    using DataModel.BasicProperties;

    public class WheelPropertiesDto
    {
        public WheelPropertiesDto()
        {
            BumpTransition = Velocity.FromMs(0.030);
            ReboundTransition = Velocity.FromMs(-0.030);
            IdealCamber = Angle.GetFromDegrees(0);
        }

        public Velocity BumpTransition { get; set; }

        public Velocity ReboundTransition { get; set; }

        public Angle IdealCamber { get; set; }
    }
}