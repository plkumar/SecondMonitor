namespace SecondMonitor.Rating.Application.Controller.Championship
{
    using System.Threading.Tasks;
    using SecondMonitor.ViewModels.Factory;
    using ViewModels.Championship.IconState;

    public class ChampionshipController : IChampionshipController
    {
        private readonly IViewModelFactory _viewModelFactory;

        public ChampionshipController(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            ChampionshipIconStateViewModel = _viewModelFactory.Create<ChampionshipIconStateViewModel>();
        }

        public ChampionshipIconStateViewModel ChampionshipIconStateViewModel { get; }

        public Task StartControllerAsync()
        {
            return Task.CompletedTask;
        }

        public Task StopControllerAsync()
        {
            return Task.CompletedTask;
        }

        public void OpenChampionshipWindow()
        {
        }
    }
}