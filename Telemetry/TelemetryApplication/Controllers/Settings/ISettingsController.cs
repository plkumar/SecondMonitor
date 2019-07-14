namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.Settings
{
    using System;
    using SecondMonitor.ViewModels.Controllers;
    using TelemetryApplication.Settings.DTO;
    using TelemetryApplication.Settings.DTO.CarProperties;

    public interface ISettingsController : IController
    {
        event EventHandler<SettingChangedArgs> SettingsChanged;

        TelemetrySettingsDto TelemetrySettings { get; }
        void SetTelemetrySettings(TelemetrySettingsDto telemetrySettings, RequestedAction action);

        CarPropertiesDto GetCarPropertiesForCurrentCar();

        CarPropertiesDto ResetAndGetCarPropertiesForCurrentCar();


        CarPropertiesDto GetCarProperties(string simulator, string carName);

        CarPropertiesDto ResetAndGetCarProperties(string simulator, string carName);

    }
}