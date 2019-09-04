namespace SecondMonitor.Rating.Application.Championship.ViewModels.IconState
{
    using SecondMonitor.ViewModels;

    public class ChampionshipIconStateViewModel : AbstractViewModel
    {
        private ChampionshipIconState _championshipIconState;

        public ChampionshipIconState ChampionshipIconState
        {
            get => _championshipIconState;
            set => SetProperty(ref _championshipIconState, value);
        }
    }
}