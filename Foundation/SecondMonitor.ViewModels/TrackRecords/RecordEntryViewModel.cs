namespace SecondMonitor.ViewModels.TrackRecords
{
    using System;
    using DataModel.TrackRecords;

    public class RecordEntryViewModel : AbstractViewModel<RecordEntryDto>
    {
        public RecordEntryViewModel()
        {
            IsFilled = false;
        }

        public bool IsFilled { get; private set; }

        public string CarName { get; set; }

        private TimeSpan LapTime { get; set; }

        private DateTime CreationDate { get; set; }

        protected override void ApplyModel(RecordEntryDto model)
        {
            IsFilled = model != null;
            if (model == null)
            {
                return;
            }

            CarName = model.CarName;
            LapTime = model.LapTime;
            CreationDate = model.RecordDate;

        }

        public override RecordEntryDto SaveToNewModel()
        {
            throw new NotImplementedException();
        }
    }
}