namespace SecondMonitor.Rating.Application.Championship.ViewModels.Selection
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class ChampionshipsSelectionViewModel : AbstractViewModel<IEnumerable<ChampionshipDto>>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private ChampionshipOverviewViewModel _selectedChampionship;

        public ChampionshipsSelectionViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public IReadOnlyCollection<ChampionshipOverviewViewModel> Championships
        {
            get;
            private set;
        }

        public ChampionshipOverviewViewModel SelectedChampionship
        {
            get => _selectedChampionship;
            set => SetProperty(ref _selectedChampionship, value);
        }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        protected override void ApplyModel(IEnumerable<ChampionshipDto> model)
        {
            Championships = model.Select(x =>
            {
                var newViewModel = _viewModelFactory.Create<ChampionshipOverviewViewModel>();
                newViewModel.FromModel(x);
                return newViewModel;
            }).ToList();

            SelectedChampionship = Championships.FirstOrDefault();
        }

        public override IEnumerable<ChampionshipDto> SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}