namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.TelemetryLoad.DataFillers
{
    using System;
    using DataModel.OperationalRange;
    using DataModel.Snapshot.Systems;
    using DataModel.Telemetry;
    using SimdataManagement.SimSettings;

    public class SlipInformationFiller : IMissingTelemetryFiller
    {
        private readonly ICarSpecificationProvider _carSpecificationProvider;
        private CarModelProperties _carModelProperties;
        private string _currentCarName;
        private string _simulator;
        private readonly object _lockObject = new object();

        public SlipInformationFiller(ICarSpecificationProvider carSpecificationProvider)
        {
            _carSpecificationProvider = carSpecificationProvider;
        }

        public void Visit(TimedTelemetrySnapshot timedTelemetrySnapshot)
        {
            if (timedTelemetrySnapshot.SimulatorSourceInfo.TelemetryInfo.ContainsSlipInformation)
            {
                return;
            }

            string carName = timedTelemetrySnapshot.PlayerData.CarName;
            lock (_lockObject)
            {
                if (string.IsNullOrWhiteSpace(_currentCarName) || _currentCarName != carName)
                {
                    _currentCarName = carName;
                    _carModelProperties = _carSpecificationProvider.GetCarModelProperties(_simulator, carName);
                }
            }

            if (_carModelProperties == null)
            {
                return;
            }

            ComputeSlip(timedTelemetrySnapshot, timedTelemetrySnapshot.PlayerData.CarInfo.WheelsInfo.FrontLeft, true);
            ComputeSlip(timedTelemetrySnapshot, timedTelemetrySnapshot.PlayerData.CarInfo.WheelsInfo.FrontRight, true);
            ComputeSlip(timedTelemetrySnapshot, timedTelemetrySnapshot.PlayerData.CarInfo.WheelsInfo.RearLeft, false);
            ComputeSlip(timedTelemetrySnapshot, timedTelemetrySnapshot.PlayerData.CarInfo.WheelsInfo.RearRight, false);
            timedTelemetrySnapshot.SimulatorSourceInfo.TelemetryInfo.ContainsSlipInformation = true;
        }

        public void SetSimulator(string simulatorName)
        {
            _simulator = simulatorName;
        }

        private void ComputeSlip(TimedTelemetrySnapshot snapshot, WheelInfo wheelInfo, bool isFrontWheel)
        {
            double wheelVelocity = ComputeWheelVelocity(wheelInfo, isFrontWheel);
            double slip = (wheelVelocity / snapshot.PlayerData.Speed.InMs) - 1;
            wheelInfo.Slip = Math.Min(slip, 1.0);
        }

        private double ComputeWheelVelocity(WheelInfo wheelInfo, bool isFrontWheel)
        {
            double wheelDiameter = isFrontWheel ? _carModelProperties.FrontWheelDiameter.InMeters : _carModelProperties.RearWheelDiameter.InMeters;
            return wheelInfo.Rps * wheelDiameter;
        }
    }
}