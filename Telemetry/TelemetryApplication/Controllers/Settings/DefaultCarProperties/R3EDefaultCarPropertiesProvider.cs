namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.Settings.DefaultCarProperties
{
    using System;
    using System.Linq;
    using DataModel.BasicProperties;
    using TelemetryApplication.Settings.DTO.CarProperties;
    using TelemetryApplication.Settings.DTO.DefaultCarProperties;

    public class R3EDefaultCarPropertiesProvider : IDefaultCarPropertiesProvider
    {
        private readonly DefaultR3ECarSettingsRepository _defaultR3ECarSettingsRepository;
        private readonly Lazy<DefaultR3ECarsProperties> _r3ECarPropertiesLazy;

        public R3EDefaultCarPropertiesProvider(DefaultR3ECarSettingsRepository defaultR3ECarSettingsRepository)
        {
            _defaultR3ECarSettingsRepository = defaultR3ECarSettingsRepository;
            _r3ECarPropertiesLazy = new Lazy<DefaultR3ECarsProperties>(LoadCarProperties);
        }

        protected DefaultR3ECarsProperties CarProperties => _r3ECarPropertiesLazy.Value;

        private DefaultR3ECarsProperties LoadCarProperties()
        {
            return _defaultR3ECarSettingsRepository.LoadRatingsOrCreateNew();
        }

        public bool TryGetDefaultSettings(string simName, string carName, out CarPropertiesDto carProperties)
        {
            if (simName != "R3E" )
            {
                carProperties = null;
                return false;
            }

            var r3EProperties = CarProperties.CarsProperties.FirstOrDefault(x => x.CarName == carName.Replace(" ", "").ToLower());
            if (r3EProperties == null)
            {
                carProperties = null;
                return false;
            }

            carProperties = new CarPropertiesDto()
            {
                CarName = carName,
                Simulator = simName,
            };

            carProperties.FrontLeftTyre.BumpTransition = Velocity.FromMs(r3EProperties.BumpTransitionFront);
            carProperties.FrontRightTyre.BumpTransition = Velocity.FromMs(r3EProperties.BumpTransitionFront);
            carProperties.RearLeftTyre.BumpTransition = Velocity.FromMs(r3EProperties.BumpTransitionRear);
            carProperties.RearRightTyre.BumpTransition = Velocity.FromMs(r3EProperties.BumpTransitionRear);

            carProperties.FrontLeftTyre.ReboundTransition = Velocity.FromMs(r3EProperties.ReboundTransitionFront);
            carProperties.FrontRightTyre.ReboundTransition = Velocity.FromMs(r3EProperties.ReboundTransitionFront);
            carProperties.RearLeftTyre.ReboundTransition = Velocity.FromMs(r3EProperties.ReboundTransitionRear);
            carProperties.RearRightTyre.ReboundTransition = Velocity.FromMs(r3EProperties.ReboundTransitionRear);

            return true;

        }
    }
}