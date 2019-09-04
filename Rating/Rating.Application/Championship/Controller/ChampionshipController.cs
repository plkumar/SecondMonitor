namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System;
    using System.Threading.Tasks;
    using Common.DataModel.Championship;
    using Repository;
    using SecondMonitor.ViewModels.Factory;
    using ViewModels.IconState;

    public class ChampionshipController : IChampionshipController
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IChampionshipsRepository _championshipsRepository;
        private readonly Lazy<AllChampionshipsDto> _allChampionshipDtoLazy;

        public ChampionshipController(IViewModelFactory viewModelFactory, IChampionshipsRepository championshipsRepository)
        {
            _viewModelFactory = viewModelFactory;
            _championshipsRepository = championshipsRepository;
            ChampionshipIconStateViewModel = _viewModelFactory.Create<ChampionshipIconStateViewModel>();
            _allChampionshipDtoLazy = new Lazy<AllChampionshipsDto>(LoadAllChampionshipsDto);
        }

        public ChampionshipIconStateViewModel ChampionshipIconStateViewModel { get; }

        protected AllChampionshipsDto AllChampionshipsDto => _allChampionshipDtoLazy.Value;

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

        private AllChampionshipsDto LoadAllChampionshipsDto()
        {
            return _championshipsRepository.LoadRatingsOrCreateNew();
        }
    }
}