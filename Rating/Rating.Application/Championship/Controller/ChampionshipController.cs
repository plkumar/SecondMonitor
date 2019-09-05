namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Threading.Tasks;
    using SecondMonitor.ViewModels.Controllers;
    using SecondMonitor.ViewModels.Factory;
    using ViewModels.IconState;

    public class ChampionshipController : IChampionshipController
    {
        private readonly IChampionshipOverviewController _championshipOverviewController;

        public ChampionshipController(IViewModelFactory viewModelFactory, IChildControllerFactory childControllerFactory)
        {
            ChampionshipIconStateViewModel =  viewModelFactory.Create<ChampionshipIconStateViewModel>();
            _championshipOverviewController = childControllerFactory.Create<IChampionshipOverviewController>();
        }

        public ChampionshipIconStateViewModel ChampionshipIconStateViewModel { get; }


        public async Task StartControllerAsync()
        {
            await StartChildControllersAsync();
        }

        public async Task StopControllerAsync()
        {
            await StopChildControllersAsync();
        }

        public void OpenChampionshipWindow()
        {
            _championshipOverviewController.OpenChampionshipOverviewWindow();
        }

        protected async Task StartChildControllersAsync()
        {
            await _championshipOverviewController.StartControllerAsync();
        }

        protected async Task StopChildControllersAsync()
        {
            await _championshipOverviewController.StopControllerAsync();
        }
    }
}