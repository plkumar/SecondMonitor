namespace SecondMonitor.PluginManager.GameConnector.WheelInformation
{
    using Contracts.WheelInformation;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;

    public abstract class AbstractIdealWheelQuantitiesFiller : IIdealWheelQuantitiesFiller
    {
        private string _lastCarName;
        private string _lastTyreCompound;
        private OptimalQuantity<Pressure> _optimalPressure;
        private OptimalQuantity<Temperature> _optimalTemperature;

        public void FillWheelIdealQuantities(SimulatorDataSet dataSet)
        {
            DriverInfo driver = dataSet.PlayerInfo;
            if (driver == null || string.IsNullOrEmpty(driver.CarName))
            {
                return;
            }

            CheckAndRetrieveIdealQuantities(driver);

            if (_optimalPressure == null && _optimalTemperature == null)
            {
                return;
            }

            dataSet.SimulatorSourceInfo.TelemetryInfo.ContainsOptimalTemperatures = true;

            foreach (var wheel in driver.CarInfo.WheelsInfo.AllWheels)
            {
                if (_optimalPressure != null)
                {
                    wheel.TyrePressure.IdealQuantity.InKpa = _optimalPressure.IdealQuantity.InKpa;
                    wheel.TyrePressure.IdealQuantityWindow.InKpa = _optimalPressure.IdealQuantityWindow.InKpa;
                }

                if (_optimalTemperature != null)
                {
                    wheel.TyreCoreTemperature.IdealQuantity.InCelsius = _optimalTemperature.IdealQuantity.InCelsius;
                    wheel.TyreCoreTemperature.IdealQuantityWindow.InCelsius = _optimalTemperature.IdealQuantityWindow.InCelsius;

                    wheel.LeftTyreTemp.IdealQuantity.InCelsius = _optimalTemperature.IdealQuantity.InCelsius;
                    wheel.LeftTyreTemp.IdealQuantityWindow.InCelsius = _optimalTemperature.IdealQuantityWindow.InCelsius;

                    wheel.CenterTyreTemp.IdealQuantity.InCelsius = _optimalTemperature.IdealQuantity.InCelsius;
                    wheel.CenterTyreTemp.IdealQuantityWindow.InCelsius = _optimalTemperature.IdealQuantityWindow.InCelsius;

                    wheel.RightTyreTemp.IdealQuantity.InCelsius = _optimalTemperature.IdealQuantity.InCelsius;
                    wheel.RightTyreTemp.IdealQuantityWindow.InCelsius = _optimalTemperature.IdealQuantityWindow.InCelsius;
                }
            }
        }

        private void CheckAndRetrieveIdealQuantities(DriverInfo driver)
        {
            if (!string.IsNullOrEmpty(_lastCarName) && _lastCarName == driver.CarName && _lastTyreCompound == driver.CarInfo.WheelsInfo.AllWheels[0].TyreType)
            {
                return;
            }

            _optimalPressure = GetIdealTyrePressure(driver);
            _optimalTemperature = GetIdealTyreTemperatures(driver);

            _lastCarName = driver.CarName;
            _lastTyreCompound = driver.CarInfo.WheelsInfo.AllWheels[0].TyreType;
        }

        protected abstract OptimalQuantity<Pressure> GetIdealTyrePressure(DriverInfo driver);
        protected abstract OptimalQuantity<Temperature> GetIdealTyreTemperatures(DriverInfo driver);
    }
}