namespace SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO
{
    using System.Collections.Generic;
    using System.Linq;
    using CarProperties;

    public class CarsProperties
    {
        public CarsProperties()
        {
            Cars = new List<CarPropertiesDto>();
        }

        public List<CarPropertiesDto> Cars { get; set; }

        public bool TryGetCarProperties(string simulator, string carName, out CarPropertiesDto carProperties)
        {
            carProperties = Cars.FirstOrDefault(x => x.CarName == carName && x.Simulator == simulator);
            return carProperties != null;
        }

        public void SaveCarProperties(CarPropertiesDto carProperties)
        {
            Cars.RemoveAll(x => x.CarName == carProperties.CarName && x.Simulator == carProperties.Simulator);
            Cars.Add(carProperties);
        }
    }
}