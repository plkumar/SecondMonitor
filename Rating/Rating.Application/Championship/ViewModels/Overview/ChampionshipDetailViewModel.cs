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
        }

        public string ChampionshipName { get; private set; }

        public StandingOverviewViewModel StandingOverviewViewModel { get; }


        protected override void ApplyModel(ChampionshipDto model)
        {
            if (model == null)
            {
                return;
            }
            ChampionshipName = model.ChampionshipName;
            StandingOverviewViewModel.FromModel(model.Drivers);
        }

        public override ChampionshipDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}