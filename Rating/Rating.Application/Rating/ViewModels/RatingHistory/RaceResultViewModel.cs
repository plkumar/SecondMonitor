namespace SecondMonitor.Rating.Application.Rating.ViewModels.RatingHistory
{
    using System;
    using Common.DataModel;
    using SecondMonitor.ViewModels;

    public class RaceResultViewModel : AbstractViewModel<RaceResult>, IRaceResultViewModel
    {
        public string ClassName { get; set; }

        public string CarName { get; set; }

        public string TrackName { get; set; }

        public int FinishingPosition { get; set; }

        public int SimRatingChange { get; set; }

        public int SimRatingAfterRace { get; set; }

        public int ClassRatingChange { get; set; }

        public int ClassRatingAfterRace { get; set; }

        public DateTime CreationTime { get; set; }

        public int Difficulty { get; set; }

        protected override void ApplyModel(RaceResult model)
        {
            ClassName = model.ClassName;
            CarName = model.CarName;
            TrackName = model.TrackName;
            FinishingPosition = model.FinishingPosition;
            Difficulty = model.Difficulty;
            SimRatingAfterRace = model.SimulatorRatingChange.RatingAfterChange;
            SimRatingChange = model.SimulatorRatingChange.Change;
            CreationTime = model.CreationTime;

            ClassRatingAfterRace = model.ClassRatingChange.RatingAfterChange;
            ClassRatingChange = model.ClassRatingChange.Change;
        }

        public override RaceResult SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}