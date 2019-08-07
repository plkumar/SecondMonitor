namespace SecondMonitor.SimdataManagement.SimSettings
{
    using System.Collections.Concurrent;
    using DataModel.Extensions;
    using DataModel.OperationalRange;
    using ViewModels.Settings;

    public class CarSpecificationProvider : ICarSpecificationProvider
    {
        private readonly SimSettingsLoader _simSettingsLoader;
        private readonly ConcurrentDictionary<string, DataSourceProperties> _cachedProperties;

        public CarSpecificationProvider(ISettingsProvider settingsProvider)
        {
            _cachedProperties = new ConcurrentDictionary<string, DataSourceProperties>();
            _simSettingsLoader = new SimSettingsLoader(settingsProvider.CarSpecificationPath);
        }

        public void SaveAllProperties()
        {
            _cachedProperties.Values.ForEach(_simSettingsLoader.SaveDataSourceProperties);
        }

        public void SaveDataSourceProperties(DataSourceProperties dataSourceProperties)
        {
            _cachedProperties.AddOrUpdate(dataSourceProperties.SourceName, dataSourceProperties, (x, y ) => dataSourceProperties);
            _simSettingsLoader.SaveDataSourceProperties(dataSourceProperties);
        }

        public DataSourceProperties GetSimulatorProperties(string simulatorName)
        {
            return _cachedProperties.GetOrAdd(simulatorName, LoadSimulatorProperties);
        }

        public CarModelProperties GetCarModelProperties(string simulatorName, string carName)
        {
            return GetSimulatorProperties(simulatorName).GetCarModel(carName);
        }

        private DataSourceProperties LoadSimulatorProperties(string simulatorName)
        {
            return _simSettingsLoader.GetDataSourceProperties(simulatorName);
        }

    }
}