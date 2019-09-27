namespace SecondMonitor.Rating.Application.Championship.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Common.DataModel.Championship;
    using Contracts.Commands;
    using DataModel.Extensions;
    using IconState;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class ChampionshipsOverviewViewModel : AbstractViewModel<IEnumerable<ChampionshipDto>>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private ICommand _createNewCommand;
        private ICommand _deleteSelectedCommand;
        private ICommand _openSelectedCommand;
        private ChampionshipOverviewViewModel _selectedChampionship;
        private ObservableCollection<ChampionshipOverviewViewModel> _allChampionships;

        public ChampionshipsOverviewViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            AllChampionships = new ObservableCollection<ChampionshipOverviewViewModel>();
            NextRaceOverviewViewModel = viewModelFactory.Create<NextRaceOverviewViewModel>();

        }

        public NextRaceOverviewViewModel NextRaceOverviewViewModel { get; }

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
            set
            {
                SetProperty(ref _selectedChampionship, value);
                NextRaceOverviewViewModel.FromModel(value?.OriginalModel);
            }
        }

        public ObservableCollection<ChampionshipOverviewViewModel> AllChampionships
        {
            get => _allChampionships;
            set => SetProperty(ref _allChampionships, value);
        }

        public ICommand RemoveSelectedCommand { get; set; }

        protected override void ApplyModel(IEnumerable<ChampionshipDto> model)
        {
            AllChampionships.Clear();
            model.OrderBy(x => x.ChampionshipState).ForEach(AddChampionship);
            SelectedChampionship = AllChampionships.FirstOrDefault();

        }

        public override IEnumerable<ChampionshipDto> SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }

        public void InsertChampionshipFirst(ChampionshipDto championshipDto)
        {
            var newViewModel = _viewModelFactory.Create<ChampionshipOverviewViewModel>();
            newViewModel.FromModel(championshipDto);
            AllChampionships.Insert(0, newViewModel);
        }

        private void AddChampionship(ChampionshipDto championshipDto)
        {
            var newViewModel = _viewModelFactory.Create<ChampionshipOverviewViewModel>();
            newViewModel.FromModel(championshipDto);
            AllChampionships.Add(newViewModel);
        }

        public void RemoveChampionship(ChampionshipDto championshipDto)
        {
            List<ChampionshipOverviewViewModel> toRemove = AllChampionships.Where(x => x.OriginalModel.ChampionshipGlobalId == championshipDto.ChampionshipGlobalId).ToList();
            toRemove.ForEach(x => AllChampionships.Remove(x));
        }
    }
}