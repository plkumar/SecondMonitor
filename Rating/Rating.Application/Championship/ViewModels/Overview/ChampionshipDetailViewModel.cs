namespace SecondMonitor.Rating.Application.Championship.ViewModels.Overview
{
    using Common.DataModel.Championship;
    using Events;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class ChampionshipDetailViewModel : AbstractViewModel<ChampionshipDto>
    {
        private readonly IViewModelFactory _viewModelFactory;

        public ChampionshipDetailViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            StandingOverviewViewModel = _viewModelFactory.Create<StandingOverviewViewModel>();
            ChampionshipResultsOverview = _viewModelFactory.Create<ChampionshipResultsOverviewViewModel>();
        }

        public string ChampionshipName { get; private set; }

        public StandingOverviewViewModel StandingOverviewViewModel { get; }
        public ChampionshipResultsOverviewViewModel ChampionshipResultsOverview { get; }


        protected override void ApplyModel(ChampionshipDto model)
        {
            if (model == null)
            {
                return;
            }
            ChampionshipName = model.ChampionshipName;
            StandingOverviewViewModel.FromModel(model.Drivers);
            ChampionshipResultsOverview.FromModel(model);
        }

        public override ChampionshipDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}