namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DefaultCarProperties;
    using Synchronization;
    using TelemetryApplication.Settings;
    using TelemetryApplication.Settings.DTO;
    using TelemetryApplication.Settings.DTO.CarProperties;
    using TelemetryApplication.Settings.DTO.ChartProperties;

    public class SettingsController : ISettingsController
    {
        private readonly ITelemetrySettingsRepository _telemetrySettingsRepository;
        private readonly ITelemetryViewsSynchronization _telemetryViewsSynchronization;
        private readonly List<IDefaultCarPropertiesProvider> _defaultCarPropertiesProviders;
        private string _currentSimulator;
        private string _currentCar;

        public SettingsController(ITelemetrySettingsRepository telemetrySettingsRepository, ITelemetryViewsSynchronization telemetryViewsSynchronization, IEnumerable<IDefaultCarPropertiesProvider> defaultCarPropertiesProviders)
        {
            _telemetrySettingsRepository = telemetrySettingsRepository;
            _telemetryViewsSynchronization = telemetryViewsSynchronization;
            _defaultCarPropertiesProviders = defaultCarPropertiesProviders.ToList();
        }

        public event EventHandler<SettingChangedArgs> SettingsChanged;

        public TelemetrySettingsDto TelemetrySettings { get; private set; }

        public Task StartControllerAsync()
        {
            _telemetryViewsSynchronization.NewSessionLoaded += TelemetryViewsSynchronizationOnNewSessionLoaded;
            TelemetrySettings = _telemetrySettingsRepository.LoadOrCreateNew();
            if (TelemetrySettings.CarsProperties == null)
            {
                TelemetrySettings.CarsProperties = new CarsProperties();
            }

            return Task.CompletedTask;
        }

        public Task StopControllerAsync()
        {
            _telemetryViewsSynchronization.NewSessionLoaded -= TelemetryViewsSynchronizationOnNewSessionLoaded;
            _telemetrySettingsRepository.SaveTelemetrySettings(TelemetrySettings);
            return Task.CompletedTask;
        }

        public void SetTelemetrySettings(TelemetrySettingsDto telemetrySettings, RequestedAction action)
        {
            TelemetrySettings = telemetrySettings;
            SettingsChanged?.Invoke(this, new SettingChangedArgs(action));
        }

        public CarPropertiesDto GetCarPropertiesForCurrentCar()
        {
            return GetCarProperties(_currentSimulator, _currentCar);
        }

        public CarPropertiesDto ResetAndGetCarPropertiesForCurrentCar()
        {
            return ResetAndGetCarProperties(_currentSimulator, _currentCar);
        }

        public CarPropertiesDto GetCarProperties(string simulator, string carName)
        {
            if (TelemetrySettings.CarsProperties.TryGetCarProperties(simulator, carName, out CarPropertiesDto carProperties))
            {
                if (carProperties.ChartsProperties.CamberHistogram == null)
                {
                    carProperties.ChartsProperties.CamberHistogram = new CamberHistogram();
                }
                return carProperties;
            }

            carProperties = CreateDefaultCarProperties(simulator, carName);
            TelemetrySettings.CarsProperties.SaveCarProperties(carProperties);
            return carProperties;
        }

        public CarPropertiesDto ResetAndGetCarProperties(string simulator, string carName)
        {
            var carProperties = CreateDefaultCarProperties(simulator, carName);
            TelemetrySettings.CarsProperties.SaveCarProperties(carProperties);
            return carProperties;
        }

        private CarPropertiesDto CreateDefaultCarProperties(string simulator, string carName)
        {
            CarPropertiesDto carPropertiesDto = null;
            return _defaultCarPropertiesProviders.Any(x => x.TryGetDefaultSettings(simulator, carName, out carPropertiesDto)) ? carPropertiesDto : new CarPropertiesDto()
            {
                CarName = carName,
                Simulator = simulator
            };
        }

        private void TelemetryViewsSynchronizationOnNewSessionLoaded(object sender, TelemetrySessionArgs e)
        {
            _currentSimulator = e.SessionInfoDto.Simulator;
            _currentCar = e.SessionInfoDto.CarName;
        }

    }
}