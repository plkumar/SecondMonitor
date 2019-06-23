namespace SecondMonitor.PluginManager.GameConnector.WheelInformation
{
    using Contracts.WheelInformation;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using DataModel.Snapshot.Systems;

    public abstract class AbstractIdealWheelQuantitiesFiller : IIdealWheelQuantitiesFiller
    {
        private string _lastCarName;
        private string _lastTyreCompound;
        private OptimalQuantity<Pressure> _optimalPressureFront;
        private OptimalQuantity<Temperature> _optimalTemperatureFront;

        private OptimalQuantity<Pressure> _optimalPressureRear;
        private OptimalQuantity<Temperature> _optimalTemperatureRear;

        public void FillWheelIdealQuantities(SimulatorDataSet dataSet)
        {
            DriverInfo driver = dataSet.PlayerInfo;
            if (driver == null || string.IsNullOrEmpty(driver.CarName))
            {
                return;
            }

            CheckAndRetrieveIdealQuantities(driver);

            if (_optimalPressureFront == null && _optimalTemperatureFront == null)
            {
                return;
            }

            dataSet.SimulatorSourceInfo.TelemetryInfo.ContainsOptimalTemperatures = true;
            Wheels wheels = driver.CarInfo.WheelsInfo;
            FillWheelIdealQuantities(wheels.FrontLeft, _optimalPressureFront, _optimalTemperatureFront);
            FillWheelIdealQuantities(wheels.FrontRight, _optimalPressureFront, _optimalTemperatureFront);

            FillWheelIdealQuantities(wheels.RearLeft, _optimalPressureRear, _optimalTemperatureRear);
            FillWheelIdealQuantities(wheels.RearRight, _optimalPressureRear, _optimalTemperatureRear);
        }

        private void FillWheelIdealQuantities(WheelInfo wheel, OptimalQuantity<Pressure> optimalPressure, OptimalQuantity<Temperature> optimalTemperature)
        {
            if (optimalPressure != null)
            {
                wheel.TyrePressure.IdealQuantity.InKpa = optimalPressure.IdealQuantity.InKpa;
                wheel.TyrePressure.IdealQuantityWindow.InKpa = optimalPressure.IdealQuantityWindow.InKpa;
            }

            if (optimalTemperature != null)
            {
                wheel.TyreCoreTemperature.IdealQuantity.InCelsius = optimalTemperature.IdealQuantity.InCelsius;
                wheel.TyreCoreTemperature.IdealQuantityWindow.InCelsius = optimalTemperature.IdealQuantityWindow.InCelsius;

                wheel.LeftTyreTemp.IdealQuantity.InCelsius = optimalTemperature.IdealQuantity.InCelsius;
                wheel.LeftTyreTemp.IdealQuantityWindow.InCelsius = optimalTemperature.IdealQuantityWindow.InCelsius;

                wheel.CenterTyreTemp.IdealQuantity.InCelsius = optimalTemperature.IdealQuantity.InCelsius;
                wheel.CenterTyreTemp.IdealQuantityWindow.InCelsius = optimalTemperature.IdealQuantityWindow.InCelsius;

                wheel.RightTyreTemp.IdealQuantity.InCelsius = optimalTemperature.IdealQuantity.InCelsius;
                wheel.RightTyreTemp.IdealQuantityWindow.InCelsius = optimalTemperature.IdealQuantityWindow.InCelsius;
            }
        }

        private void CheckAndRetrieveIdealQuantities(DriverInfo driver)
        {
            if (!string.IsNullOrEmpty(_lastCarName) && _lastCarName == driver.CarName && _lastTyreCompound == driver.CarInfo.WheelsInfo.AllWheels[0].TyreType)
            {
                return;
            }

            _optimalPressureFront = GetIdealTyrePressureFront(driver);
            _optimalTemperatureFront = GetIdealTyreTemperaturesFront(driver);

            _optimalPressureRear = GetIdealTyrePressureRear(driver);
            _optimalTemperatureRear = GetIdealTyreTemperaturesRear(driver);

            _lastCarName = driver.CarName;
            _lastTyreCompound = driver.CarInfo.WheelsInfo.AllWheels[0].TyreType;
        }

        protected abstract OptimalQuantity<Pressure> GetIdealTyrePressureFront(DriverInfo driver);
        protected abstract OptimalQuantity<Temperature> GetIdealTyreTemperaturesFront(DriverInfo driver);

        protected abstract OptimalQuantity<Pressure> GetIdealTyrePressureRear(DriverInfo driver);
        protected abstract OptimalQuantity<Temperature> GetIdealTyreTemperaturesRear(DriverInfo driver);
    }
}