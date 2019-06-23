namespace SecondMonitor.DataModel.OperationalRange
{
    using System;
    using System.Xml.Serialization;

    using BasicProperties;

    [Serializable]
    public sealed class TyreCompoundProperties
    {
        public TyreCompoundProperties()
        {
            NoWearLimit = 0.1;
            LowWearLimit = 0.25;
            HeavyWearLimit = 0.7;
        }

        [XmlAttribute]
        public string CompoundName { get; set; }

        [XmlAttribute]
        public double NoWearLimit { get; set; }

        [XmlAttribute]
        public double LowWearLimit { get; set; }

        [XmlAttribute]
        public double HeavyWearLimit { get; set; }

        [XmlElement(ElementName = "IdealPressure")]
        public Pressure FrontIdealPressure { get; set; }

        [XmlElement(ElementName = "IdealPressureWindow")]
        public Pressure FrontIdealPressureWindow { get; set; }

        public Pressure RearIdealPressure { get; set; }

        public Pressure RearIdealPressureWindow { get; set; }

        [XmlElement(ElementName = "IdealTemperature")]
        public Temperature FrontIdealTemperature { get; set; }

        [XmlElement(ElementName = "IdealTemperatureWindow")]
        public Temperature FrontIdealTemperatureWindow { get; set; }

        public Temperature RearIdealTemperature { get; set; }

        public Temperature RearIdealTemperatureWindow { get; set; }

    }
}