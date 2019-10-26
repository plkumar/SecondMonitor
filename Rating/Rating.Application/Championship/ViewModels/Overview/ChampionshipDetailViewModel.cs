namespace SecondMonitor.Rating.Application.Championship.ViewModels.Overview
{
    using Common.DataModel.Championship;
    using Events;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class ChampionshipDetailViewModel : AbstractViewModel<ChampionshipDto>
    {
        public ChampionshipDetailViewModel(IViewModelFactory viewModelFactory)
        {
            StandingOverviewViewModel = viewModelFactory.Create<StandingOverviewViewModel>();
            CalendarResultsOverview = viewModelFactory.Create<CalendarResultsOverviewViewModel>();
            ChampionshipSessionsResults = viewModelFactory.Create<ChampionshipSessionsResultsViewModel>();
        }

        public string ChampionshipName { get; private set; }

        public StandingOverviewViewModel StandingOverviewViewModel { get; }
        public CalendarResultsOverviewViewModel CalendarResultsOverview { get; }
        public ChampionshipSessionsResultsViewModel ChampionshipSessionsResults { get; }

        protected override void ApplyModel(ChampionshipDto model)
        {
            if (model == null)
            {
                return;
            }
            ChampionshipName = model.ChampionshipName;
            StandingOverviewViewModel.FromModel(model.Drivers);
            CalendarResultsOverview.FromModel(model);
            ChampionshipSessionsResults.FromModel(model);
        }

        public override ChampionshipDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}