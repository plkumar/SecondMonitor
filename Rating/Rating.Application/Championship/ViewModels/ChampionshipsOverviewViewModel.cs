namespace SecondMonitor.Rating.Application.Championship.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class ChampionshipsOverviewViewModel : AbstractViewModel<IEnumerable<ChampionshipDto>>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private ICommand _createNewCommand;
        private ICommand _deleteSelectedCommand;
        private ICommand _openSelectedCommand;
        private ChampionshipOverviewViewModel _selectedChampionship;
        private IReadOnlyCollection<ChampionshipOverviewViewModel> _allChampionships;

        public ChampionshipsOverviewViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public ICommand CreateNewCommand
        {
            get => _createNewCommand;
            set => SetProperty(ref _createNewCommand, value);
        }

        public ICommand DeleteSelectedCommand
        {
            get => _deleteSelectedCommand;
            set => SetProperty(ref _deleteSelectedCommand, value);
        }

        public ICommand OpenSelectedCommand
        {
            get => _openSelectedCommand;
            set => SetProperty(ref _openSelectedCommand, value);
        }

        public ChampionshipOverviewViewModel SelectedChampionship
        {
            get => _selectedChampionship;
            set => SetProperty(ref _selectedChampionship, value);
        }

        public IReadOnlyCollection<ChampionshipOverviewViewModel> AllChampionships
        {
            get => _allChampionships;
            set => SetProperty(ref _allChampionships, value);
        }

        protected override void ApplyModel(IEnumerable<ChampionshipDto> model)
        {
            AllChampionships = model.OrderBy(x => x.ChampionshipState).Select(x =>
            {
                var newViewModel = _viewModelFactory.Create<ChampionshipOverviewViewModel>();
                newViewModel.FromModel(x);
                return newViewModel;
            }).ToList();
        }

        public override IEnumerable<ChampionshipDto> SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}