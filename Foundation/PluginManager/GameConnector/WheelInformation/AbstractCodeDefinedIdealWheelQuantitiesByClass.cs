namespace SecondMonitor.PluginManager.GameConnector.WheelInformation
{
    using System.Collections.Generic;
    using DataModel.BasicProperties;
    using DataModel.Snapshot.Drivers;

    public abstract class AbstractCodeDefinedIdealWheelQuantitiesByClass : AbstractIdealWheelQuantitiesFiller
    {
        protected abstract Dictionary<string, TyresProperties> ClassTyrePropertiesMap { get; }

        protected override OptimalQuantity<Pressure> GetIdealTyrePressureFront(DriverInfo driver)
        {
            if (!ClassTyrePropertiesMap.TryGetValue(driver.CarClassName, out var tyreProperties))
            {
                return null;
            }
            return new OptimalQuantity<Pressure>()
            {
                IdealQuantity = Pressure.FromKiloPascals(tyreProperties.FrontTyres.OptimalPressure),
                IdealQuantityWindow = Pressure.FromKiloPascals(tyreProperties.FrontTyres.OptimalPressureRange)
            };
        }

        protected override OptimalQuantity<Temperature> GetIdealTyreTemperaturesFront(DriverInfo driver)
        {
            if (!ClassTyrePropertiesMap.TryGetValue(driver.CarClassName, out var tyreProperties))
            {
                return null;
            }
            return new OptimalQuantity<Temperature>()
            {
                IdealQuantity = Temperature.FromCelsius(tyreProperties.FrontTyres.OptimalTemperature),
                IdealQuantityWindow = Temperature.FromCelsius(tyreProperties.FrontTyres.OptimalTemperatureRange)
            };
        }

        protected override OptimalQuantity<Pressure> GetIdealTyrePressureRear(DriverInfo driver)
        {
            if (!ClassTyrePropertiesMap.TryGetValue(driver.CarClassName, out var tyreProperties))
            {
                return null;
            }
            return new OptimalQuantity<Pressure>()
            {
                IdealQuantity = Pressure.FromKiloPascals(tyreProperties.RearTyres.OptimalPressure),
                IdealQuantityWindow = Pressure.FromKiloPascals(tyreProperties.RearTyres.OptimalPressureRange)
            };
        }

        protected override OptimalQuantity<Temperature> GetIdealTyreTemperaturesRear(DriverInfo driver)
        {
            if (!ClassTyrePropertiesMap.TryGetValue(driver.CarClassName, out var tyreProperties))
            {
                return null;
            }
            return new OptimalQuantity<Temperature>()
            {
                IdealQuantity = Temperature.FromCelsius(tyreProperties.RearTyres.OptimalTemperature),
                IdealQuantityWindow = Temperature.FromCelsius(tyreProperties.RearTyres.OptimalTemperatureRange)
            };
        }

        protected class TyresProperties
        {
            public TyresProperties(TyreProperties frontTyres, TyreProperties rearTyres)
            {
                FrontTyres = frontTyres;
                RearTyres = rearTyres;
            }

            public TyreProperties FrontTyres { get; }
            public TyreProperties RearTyres { get; }
        }

        protected class TyreProperties
        {
            public TyreProperties(double optimalTemperature, double optimalTemperatureRange, double optimalPressure, double optimalPressureRange)
            {
                OptimalTemperature = optimalTemperature;
                OptimalTemperatureRange = optimalTemperatureRange;
                OptimalPressure = optimalPressure;
                OptimalPressureRange = optimalPressureRange;
            }

            public double OptimalTemperature { get; }
            public double OptimalTemperatureRange { get; }
            public double OptimalPressure { get; }
            public double OptimalPressureRange { get; }
        }
    }
}