namespace SecondMonitor.Rating.Application.Rating.ViewModels.RatingOverview
{
    using System;
    using System.Windows.Input;
    using Common.DataModel;
    using SecondMonitor.ViewModels;

    public interface IClassRatingViewModel : IViewModel<ClassRating>
    {
        string ClassName { get;}
        DateTime LastRace { get;  }

        int Rating { get; }

        int Difficulty { get; set; }

        int SimulatorDelta { get; set; }

        ICommand OpenClassHistoryCommand { get; set; }
    }
}