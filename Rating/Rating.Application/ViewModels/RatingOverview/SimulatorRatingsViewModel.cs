namespace SecondMonitor.Rating.Application.ViewModels.RatingOverview
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Configuration;
    using Common.DataModel;
    using Contracts.Commands;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class SimulatorRatingsViewModel : AbstractViewModel<SimulatorRating>, ISimulatorRatingsViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly SimulatorRatingConfigurationProvider _simulatorRatingConfigurationProvider;

        public SimulatorRatingsViewModel(IViewModelFactory viewModelFactory, SimulatorRatingConfigurationProvider simulatorRatingConfigurationProvider)
        {
            _viewModelFactory = viewModelFactory;
            _simulatorRatingConfigurationProvider = simulatorRatingConfigurationProvider;
        }

       public List<IClassRatingViewModel> ClassRatings { get; private set; }

       public DateTime LastRace { get; private set; }

       public int SimulatorRating { get; private set; }

       public string SimulatorName { get; private set; }

       public IRelayCommandWithParameter<IClassRatingViewModel> OpenClassHistoryCommand { get; set; }

        protected override void ApplyModel(SimulatorRating model)
        {
            var ratingConfiguration = _simulatorRatingConfigurationProvider.GetRatingConfiguration(model.SimulatorName);
            LastRace = model.PlayersRating.CreationTime;
            SimulatorRating = model.PlayersRating.Rating;
            SimulatorName = model.SimulatorName;

            ClassRatings = model.ClassRatings.OrderBy(x => x.ClassName).Select(x =>
            {
                var newViewModel = _viewModelFactory.Create<IClassRatingViewModel>();
                newViewModel.FromModel(x);
                newViewModel.SimulatorDelta = newViewModel.Rating - SimulatorRating;
                newViewModel.OpenClassHistoryCommand = new RelayCommandWithParameter<IClassRatingViewModel>(y => OpenClassHistory(newViewModel));
                newViewModel.Difficulty = ratingConfiguration.GetDifficultyForRating(x.DifficultyRating.Rating);
                return newViewModel;
            }).ToList();
        }

        private void OpenClassHistory(IClassRatingViewModel classRatingViewModel)
        {
            OpenClassHistoryCommand.Execute(classRatingViewModel);
        }

        public override SimulatorRating SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}