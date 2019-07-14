namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.Settings.DefaultCarProperties
{
    using TelemetryApplication.Settings.DTO.CarProperties;

    public interface IDefaultCarPropertiesProvider
    {
        bool TryGetDefaultSettings(string simName, string carName, out CarPropertiesDto carProperties);
    }
}