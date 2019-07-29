namespace SecondMonitor.ViewModels.TrackRecords
{
    using System;
    using DataModel.TrackRecords;

    public class RecordViewModel : AbstractViewModel<RecordEntryDto>, IRecordViewModel
    {
        private TimeSpan _lapTime;
        private bool _isHighlighted;
        private bool _isVisible;
        private DateTime _recordDatetime;
        private string _sessionType;
        private string _carName;

        public TimeSpan LapTime
        {
            get => _lapTime;
            set => SetProperty(ref _lapTime, value);
        }

        public bool IsHighlighted
        {
            get => _isHighlighted;
            set => SetProperty(ref _isHighlighted, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public DateTime RecordDatetime
        {
            get => _recordDatetime;
            private set => SetProperty(ref _recordDatetime, value);
        }

        public string SessionType
        {
            get => _sessionType;
            private set => SetProperty(ref _sessionType, value);
        }

        public string CarName
        {
            get => _carName;
            private set => SetProperty(ref _carName, value);
        }

        protected override void ApplyModel(RecordEntryDto model)
        {

            if (model == null)
            {
                IsVisible = false;
                return;
            }
            LapTime = model.LapTime;
            IsVisible = LapTime != TimeSpan.Zero && LapTime != TimeSpan.MinValue;
            CarName = model.CarName;
            RecordDatetime = model.RecordDate;
            SessionType = model.SessionType.ToString();
        }

        public override RecordEntryDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}