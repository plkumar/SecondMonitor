namespace SecondMonitor.SimdataManagement.SimSettings
{
    using DataModel.OperationalRange;

    public interface ICarSpecificationProvider
    {
        void SaveAllProperties();

        void SaveDataSourceProperties(DataSourceProperties dataSourceProperties);

        DataSourceProperties GetSimulatorProperties(string simulatorName);

        CarModelProperties GetCarModelProperties(string simulatorName, string carProperties);
    }
}