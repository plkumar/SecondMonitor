namespace SecondMonitor.Rating.Application.Rating.ViewModels.RatingOverview
{
    using System;
    using System.Collections.Generic;
    using Common.DataModel;
    using Contracts.Commands;
    using SecondMonitor.ViewModels;

    public interface ISimulatorRatingsViewModel : IViewModel<SimulatorRating>
    {
        List<IClassRatingViewModel> ClassRatings { get;}

        DateTime LastRace { get; }

        int SimulatorRating { get; }

        string SimulatorName { get;}

        IRelayCommandWithParameter<IClassRatingViewModel> OpenClassHistoryCommand { get; set; }
    }
}