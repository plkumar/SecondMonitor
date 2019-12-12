namespace SecondMonitor.Rating.Application.Rating.ViewModels.RatingOverview
{
    using System;
    using System.Windows.Input;
    using Common.DataModel;
    using SecondMonitor.ViewModels;

    public class ClassRatingViewModel : AbstractViewModel<ClassRating>, IClassRatingViewModel
    {
        public string ClassName { get; private set; }

        public DateTime LastRace { get; private set; }

        public int Rating { get; private set; }

        public int SimulatorDelta { get; set; }

        public int Difficulty { get; set; }

        public ICommand OpenClassHistoryCommand { get; set; }

        protected override void ApplyModel(ClassRating model)
        {
            ClassName = model.ClassName;
            LastRace = model.PlayersRating.CreationTime;
            Rating = model.PlayersRating.Rating;
        }

        public override ClassRating SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}