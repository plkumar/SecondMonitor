namespace SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO.CarProperties
{
    using System.Xml.Serialization;
    using ChartProperties;

    public class CarPropertiesDto
    {
        public CarPropertiesDto()
        {
            FrontLeftTyre = new WheelPropertiesDto();
            FrontRightTyre = new WheelPropertiesDto();
            RearLeftTyre = new WheelPropertiesDto();
            RearRightTyre = new WheelPropertiesDto();
            ChartsProperties = new ChartsProperties();
        }

        [XmlAttribute]
        public string CarName { get; set; }

        [XmlAttribute]
        public string Simulator { get; set; }

        public ChartsProperties ChartsProperties { get; set; }

        public WheelPropertiesDto FrontLeftTyre { get; set; }

        public WheelPropertiesDto FrontRightTyre { get; set; }

        public WheelPropertiesDto RearLeftTyre { get; set; }

        public WheelPropertiesDto RearRightTyre { get; set; }
    }
}