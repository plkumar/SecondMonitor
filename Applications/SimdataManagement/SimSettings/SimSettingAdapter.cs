namespace SecondMonitor.SimdataManagement.SimSettings
{
    using System.Collections.Generic;

    using DataModel.OperationalRange;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Systems;

    public class SimSettingAdapter : ISimulatorDataSetVisitor
    {
        private readonly ICarSpecificationProvider _carSpecificationProvider;

        private DataSourceProperties _dataSourceProperties;

        private KeyValuePair<string, CarModelProperties> _lastCarProperties;
        private KeyValuePair<string, TyreCompoundProperties> _lastCompound;

        public SimSettingAdapter(ICarSpecificationProvider carSpecificationProvider)
        {
            _carSpecificationProvider = carSpecificationProvider;
        }

        public CarModelProperties LastUsedCarProperties => _lastCarProperties.Value;

        public TyreCompoundProperties LastUsedCompound => _lastCompound.Value;

        public List<TyreCompoundProperties> GlobalTyreCompoundsProperties
        {
            get => _dataSourceProperties.TyreCompoundsProperties;
            set
            {
                _dataSourceProperties.TyreCompoundsProperties = value;
                _lastCarProperties = new KeyValuePair<string, CarModelProperties>(string.Empty, _lastCarProperties.Value);
                _lastCompound = new KeyValuePair<string, TyreCompoundProperties>(string.Empty, _lastCompound.Value);
                _carSpecificationProvider.SaveDataSourceProperties(_dataSourceProperties);
            }
        }

        public void Visit(SimulatorDataSet simulatorDataSet)
        {
            if (simulatorDataSet?.PlayerInfo?.CarName == null)
            {
                return;
            }

            if (_dataSourceProperties?.SourceName != simulatorDataSet.Source)
            {
                ReloadDataSourceProperties(simulatorDataSet.Source);
                if (!simulatorDataSet.SimulatorSourceInfo.GlobalTyreCompounds && _dataSourceProperties.TyreCompoundsProperties.Count > 0)
                {
                    _dataSourceProperties.TyreCompoundsProperties.Clear();
                }
            }

            CarModelProperties carModel = GetCarModelProperties(simulatorDataSet);
            ApplyCarMode(simulatorDataSet, carModel);
        }

        public void Reset()
        {

        }

        public void ReplaceCarModelProperties(CarModelProperties carModelProperties)
        {
            _dataSourceProperties.ReplaceCarModel(carModelProperties);
            _lastCarProperties = new KeyValuePair<string, CarModelProperties>(carModelProperties.Name, carModelProperties);
            _carSpecificationProvider.SaveDataSourceProperties(_dataSourceProperties);
        }

        private void ApplyCarMode(SimulatorDataSet simulatorDataSet, CarModelProperties carModel)
        {
            Wheels wheels = simulatorDataSet.PlayerInfo.CarInfo.WheelsInfo;
            if (!simulatorDataSet.InputInfo.WheelAngleFilled)
            {
                simulatorDataSet.InputInfo.WheelAngleFilled = true;
                simulatorDataSet.InputInfo.WheelAngle = ((carModel.WheelRotation) / 2.0) * simulatorDataSet.InputInfo.SteeringInput;
            }

            TyreCompoundProperties tyreCompound = GetTyreCompound(simulatorDataSet, wheels.FrontLeft, wheels.RearLeft, carModel);
            ApplyWheelProperty(wheels.FrontLeft, true, carModel, tyreCompound);
            ApplyWheelProperty(wheels.FrontRight, true, carModel, tyreCompound);
            ApplyWheelProperty(wheels.RearLeft, false, carModel, tyreCompound);
            ApplyWheelProperty(wheels.RearRight, false, carModel, tyreCompound);
        }

        private static void ApplyWheelProperty(WheelInfo wheelInfo, bool isFront, CarModelProperties carModel, TyreCompoundProperties tyreCompound)
        {
            wheelInfo.BrakeTemperature.IdealQuantity.InCelsius = carModel.OptimalBrakeTemperature.InCelsius;
            wheelInfo.BrakeTemperature.IdealQuantityWindow.InCelsius = carModel.OptimalBrakeTemperatureWindow.InCelsius;

            if (string.IsNullOrWhiteSpace(wheelInfo.TyreType) || wheelInfo.TyreType == "\u0001")
            {
                return;
            }

            wheelInfo.TyrePressure.IdealQuantity.InKpa = isFront ? tyreCompound.FrontIdealPressure.InKpa : tyreCompound.RearIdealPressure.InKpa;
            wheelInfo.TyrePressure.IdealQuantityWindow.InKpa = isFront ? tyreCompound.FrontIdealPressureWindow.InKpa : tyreCompound.RearIdealPressureWindow.InKpa;

            wheelInfo.LeftTyreTemp.IdealQuantity.InCelsius = isFront ? tyreCompound.FrontIdealTemperature.InCelsius : tyreCompound.RearIdealTemperature.InCelsius;
            wheelInfo.LeftTyreTemp.IdealQuantityWindow.InCelsius = isFront ? tyreCompound.FrontIdealTemperatureWindow.InCelsius : tyreCompound.RearIdealTemperatureWindow.InCelsius;

            wheelInfo.RightTyreTemp.IdealQuantity.InCelsius = isFront ? tyreCompound.FrontIdealTemperature.InCelsius : tyreCompound.RearIdealTemperature.InCelsius;
            wheelInfo.RightTyreTemp.IdealQuantityWindow.InCelsius = isFront ? tyreCompound.FrontIdealTemperatureWindow.InCelsius : tyreCompound.RearIdealTemperatureWindow.InCelsius;

            wheelInfo.CenterTyreTemp.IdealQuantity.InCelsius = isFront ? tyreCompound.FrontIdealTemperature.InCelsius : tyreCompound.RearIdealTemperature.InCelsius;
            wheelInfo.CenterTyreTemp.IdealQuantityWindow.InCelsius = isFront ? tyreCompound.FrontIdealTemperatureWindow.InCelsius : tyreCompound.RearIdealTemperatureWindow.InCelsius;

            wheelInfo.TyreCoreTemperature.IdealQuantity.InCelsius = isFront ? tyreCompound.FrontIdealTemperature.InCelsius : tyreCompound.RearIdealTemperature.InCelsius;
            wheelInfo.TyreCoreTemperature.IdealQuantityWindow.InCelsius = isFront ? tyreCompound.FrontIdealTemperatureWindow.InCelsius : tyreCompound.RearIdealTemperatureWindow.InCelsius;

            wheelInfo.TyreWear.NoWearWearLimit = tyreCompound.NoWearLimit;
            wheelInfo.TyreWear.LightWearLimit = tyreCompound.LowWearLimit;
            wheelInfo.TyreWear.HeavyWearLimit = tyreCompound.HeavyWearLimit;
        }

        private TyreCompoundProperties GetTyreCompound(SimulatorDataSet simulatorDataSet, WheelInfo frontWheel, WheelInfo rearWheel, CarModelProperties carModel)
        {
            string compoundName = frontWheel.TyreType;
            if (_lastCompound.Key == compoundName)
            {
                return _lastCompound.Value;
            }

            TyreCompoundProperties tyreCompound = null;

            tyreCompound = carModel.GetTyreCompound(compoundName);
            if (tyreCompound != null)
            {
                _lastCompound = new KeyValuePair<string, TyreCompoundProperties>(tyreCompound.CompoundName, tyreCompound);
                return tyreCompound;
            }


            tyreCompound = _dataSourceProperties.GetTyreCompound(compoundName);
            if (tyreCompound == null)
            {
                tyreCompound = CreateTyreCompound(frontWheel, rearWheel);
                if (simulatorDataSet.SimulatorSourceInfo.GlobalTyreCompounds)
                {
                    _dataSourceProperties.AddTyreCompound(tyreCompound);
                }
                else
                {
                    carModel.AddTyreCompound(tyreCompound);
                }
                _carSpecificationProvider.SaveDataSourceProperties(_dataSourceProperties);
            }

            _lastCompound = new KeyValuePair<string, TyreCompoundProperties>(tyreCompound.CompoundName, tyreCompound);
            return tyreCompound;

        }

        private CarModelProperties GetCarModelProperties(SimulatorDataSet simulatorDataSet)
        {
            string carName = simulatorDataSet.PlayerInfo.CarName;

            if (carName == _lastCarProperties.Key)
            {
                return _lastCarProperties.Value;
            }

            CarModelProperties carModelProperties = _dataSourceProperties.GetCarModel(carName);

            if (carModelProperties == null || carModelProperties.OriginalContainsOptimalTemperature != simulatorDataSet.SimulatorSourceInfo.TelemetryInfo.ContainsOptimalTemperatures)
            {
                _dataSourceProperties.CarModelsProperties.RemoveAll(x => x.Name == carName);
                carModelProperties = CreateNewCarModelProperties(carName, simulatorDataSet);
            }

            _lastCarProperties = new KeyValuePair<string, CarModelProperties>(carModelProperties.Name, carModelProperties);
            return carModelProperties;
        }

        private CarModelProperties CreateNewCarModelProperties(string carName, SimulatorDataSet simulatorDataSet)
        {
            CarModelProperties newCarModelProperties = new CarModelProperties {Name = carName, OriginalContainsOptimalTemperature = simulatorDataSet.SimulatorSourceInfo.TelemetryInfo.ContainsOptimalTemperatures, OptimalBrakeTemperature = simulatorDataSet.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.BrakeTemperature.IdealQuantity, OptimalBrakeTemperatureWindow = simulatorDataSet.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.BrakeTemperature.IdealQuantityWindow};
            _dataSourceProperties.AddCarModel(newCarModelProperties);
            _carSpecificationProvider.SaveDataSourceProperties(_dataSourceProperties);
            return newCarModelProperties;
        }

        private TyreCompoundProperties CreateTyreCompound(WheelInfo frontWheel, WheelInfo rearWheel)
        {
            TyreCompoundProperties tyreCompound = new TyreCompoundProperties() { CompoundName = frontWheel.TyreType };
            tyreCompound.FrontIdealPressure = frontWheel.TyrePressure.IdealQuantity;
            tyreCompound.FrontIdealPressureWindow = frontWheel.TyrePressure.IdealQuantityWindow;

            tyreCompound.RearIdealPressure = rearWheel.TyrePressure.IdealQuantity;
            tyreCompound.RearIdealPressureWindow = rearWheel.TyrePressure.IdealQuantityWindow;

            tyreCompound.RearIdealTemperature = rearWheel.CenterTyreTemp.IdealQuantity;
            tyreCompound.RearIdealTemperatureWindow = rearWheel.CenterTyreTemp.IdealQuantityWindow;

            tyreCompound.FrontIdealTemperature = frontWheel.CenterTyreTemp.IdealQuantity;
            tyreCompound.FrontIdealTemperatureWindow = frontWheel.CenterTyreTemp.IdealQuantityWindow;

            tyreCompound.NoWearLimit = frontWheel.TyreWear.NoWearWearLimit;
            tyreCompound.LowWearLimit = frontWheel.TyreWear.LightWearLimit;
            tyreCompound.HeavyWearLimit = frontWheel.TyreWear.HeavyWearLimit;

            return tyreCompound;
        }

        private void ReloadDataSourceProperties(string sourceName)
        {
            _dataSourceProperties = _carSpecificationProvider.GetSimulatorProperties(sourceName);
        }
    }
}