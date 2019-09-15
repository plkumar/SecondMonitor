namespace SecondMonitor.ViewModels.CarStatus.FuelStatus
{
    using System.Windows;
    using Contracts.FuelInformation;
    using DataModel.BasicProperties;

    public class SessionFuelConsumptionViewModel : AbstractViewModel<SessionFuelConsumptionInfo>, ISessionFuelConsumptionViewModel
    {
        private string _trackName;
        private Distance _lapDistance;
        private string _sessionType;
        private IFuelConsumptionInfo _fuelConsumptionInfo;
        private Volume _avgPerMinute;
        private Volume _avgPerLap;

        public string TrackName
        {
            get => _trackName;
            set => SetProperty(ref _trackName, value);
        }

        public Distance LapDistance
        {
            get => _lapDistance;
            set
            {
                SetProperty(ref _lapDistance, value);
                OnFuelConsumptionChanged();
            }
        }

        public string SessionType
        {
            get => _sessionType;
            set => SetProperty(ref _sessionType, value);
        }

        public IFuelConsumptionInfo FuelConsumption
        {
            get => _fuelConsumptionInfo;
            set
            {
                SetProperty(ref _fuelConsumptionInfo, value);
                OnFuelConsumptionChanged();
            }
        }

        public Volume AvgPerMinute
        {
            get => _avgPerMinute;
            set => SetProperty(ref _avgPerMinute, value);
        }

        public Volume AvgPerLap
        {
            get => _avgPerLap;
            set => SetProperty(ref _avgPerLap, value);
        }

        protected override void ApplyModel(SessionFuelConsumptionInfo model)
        {
            TrackName = model.TrackName;
            LapDistance = model.LapDistance;
            FuelConsumption = model.FuelConsumptionInfo;
            SessionType = model.SessionType.ToString();
        }

        public override SessionFuelConsumptionInfo SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }

        private void OnFuelConsumptionChanged()
        {
            if (FuelConsumption == null || LapDistance == null)
            {
                return;
            }

            AvgPerMinute = FuelConsumption.GetAveragePerMinute();
            AvgPerLap = FuelConsumption.GetAveragePerDistance(LapDistance);
        }
    }
}