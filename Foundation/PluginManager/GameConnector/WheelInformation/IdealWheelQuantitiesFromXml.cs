namespace SecondMonitor.PluginManager.GameConnector.WheelInformation
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using DataModel.BasicProperties;
    using DataModel.OperationalRange;
    using DataModel.Snapshot.Drivers;

    public class IdealWheelQuantitiesFromXml : AbstractIdealWheelQuantitiesFiller
    {
        private readonly string _fileName;
        private readonly Lazy<DataSourceProperties> _dataSourceProperties;

        public IdealWheelQuantitiesFromXml(string fileName)
        {
            _fileName = fileName;
            _dataSourceProperties = new Lazy<DataSourceProperties>(LoaDataSourceProperties);
        }

        private DataSourceProperties DataSourceProperties => _dataSourceProperties.Value;

        protected override OptimalQuantity<Pressure> GetIdealTyrePressureFront(DriverInfo driver)
        {
            var tyreProperties = GetTyreProperties(driver);
            return tyreProperties != null ? new OptimalQuantity<Pressure>() {IdealQuantity = tyreProperties.FrontIdealPressure, IdealQuantityWindow = tyreProperties.FrontIdealPressureWindow} : null;
        }

        protected override OptimalQuantity<Temperature> GetIdealTyreTemperaturesFront(DriverInfo driver)
        {
            var tyreProperties = GetTyreProperties(driver);
            return tyreProperties != null ? new OptimalQuantity<Temperature>() { IdealQuantity = tyreProperties.FrontIdealTemperature, IdealQuantityWindow = tyreProperties.FrontIdealTemperatureWindow } : null;
        }

        protected override OptimalQuantity<Pressure> GetIdealTyrePressureRear(DriverInfo driver)
        {
            var tyreProperties = GetTyreProperties(driver);
            return tyreProperties != null ? new OptimalQuantity<Pressure>() { IdealQuantity = tyreProperties.RearIdealPressure, IdealQuantityWindow = tyreProperties.RearIdealPressureWindow } : null;
        }

        protected override OptimalQuantity<Temperature> GetIdealTyreTemperaturesRear(DriverInfo driver)
        {
            var tyreProperties = GetTyreProperties(driver);
            return tyreProperties != null ? new OptimalQuantity<Temperature>() { IdealQuantity = tyreProperties.RearIdealTemperature, IdealQuantityWindow = tyreProperties.RearIdealTemperatureWindow } : null;
        }

        private TyreCompoundProperties GetTyreProperties(DriverInfo driver)
        {
            var carProperties = DataSourceProperties.CarModelsProperties.FirstOrDefault(x => x.Name == driver.CarName);

            return carProperties?.TyreCompoundsProperties.FirstOrDefault(x => x.CompoundName == driver.CarInfo.WheelsInfo.AllWheels[0].TyreType);
        }

        private DataSourceProperties LoaDataSourceProperties()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSourceProperties));

            if (!File.Exists(_fileName))
            {
                return new DataSourceProperties();
            }

            using (FileStream file = File.Open(_fileName, FileMode.Open, FileAccess.Read))
            {
                var properties = (DataSourceProperties)xmlSerializer.Deserialize(file);
                properties.MigrateUp();
                return properties;
            }
        }
    }
}