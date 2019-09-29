namespace SecondMonitor.Rating.Application.Championship.ViewModels.IconState
{
    using SecondMonitor.ViewModels;

    public class ChampionshipIconStateViewModel : AbstractViewModel
    {
        private ChampionshipIconState _championshipIconState;
        private string _tooltipText;

        public ChampionshipIconState ChampionshipIconState
        {
            get => _championshipIconState;
            set => SetProperty(ref _championshipIconState, value);
        }

        public string TooltipText
        {
            get => _tooltipText;
            set => SetProperty(ref _tooltipText, value);
        }
    }
}