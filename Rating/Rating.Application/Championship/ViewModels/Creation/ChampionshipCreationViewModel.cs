﻿namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation
{
    using System.Collections.Specialized;
    using System.Windows.Input;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;
    using Session;

    public class ChampionshipCreationViewModel : AbstractViewModel
    {
        private bool _isSimulatorSelectionEnabled;
        private string _championshipTitle;
        private bool _aiNamesCanChange;

        private bool _isOkButtonEnabled;
        private ICommand _okCommand;
        private ICommand _cancelCommand;

        public ChampionshipCreationViewModel(IViewModelFactory viewModelFactory)
        {
            CalendarDefinitionViewModel = viewModelFactory.Create<CalendarDefinitionViewModel>();
            SessionsDefinitionViewModel = viewModelFactory.Create<SessionsDefinitionViewModel>();
            AiNamesCanChange = true;
            ChampionshipTitle = "Custom Championship";
            CalendarDefinitionViewModel.CalendarViewModel.CalendarEntries.CollectionChanged += CalendarEntriesOnCollectionChanged;

        }

        public bool IsSimulatorSelectionEnabled
        {
            get => _isSimulatorSelectionEnabled;
            set => SetProperty(ref _isSimulatorSelectionEnabled, value);
        }

        public CalendarDefinitionViewModel CalendarDefinitionViewModel { get;}

        public SessionsDefinitionViewModel SessionsDefinitionViewModel { get; }

        public string[] AvailableSimulators { get; set; }

        public string SelectedSimulator { get; set; }

        public string ChampionshipTitle
        {
            get => _championshipTitle;
            set => SetProperty(ref _championshipTitle, value);
        }

        public ICommand ConfirmSimulatorCommand { get; set; }

        public bool AiNamesCanChange
        {
            get => _aiNamesCanChange;
            set => SetProperty(ref _aiNamesCanChange, value);
        }

        public bool IsOkButtonEnabled
        {
            get => _isOkButtonEnabled;
            set => SetProperty(ref _isOkButtonEnabled, value);
        }

        public ICommand OkCommand
        {
            get => _okCommand;
            set => SetProperty(ref _okCommand, value);
        }

        public ICommand CancelCommand
        {
            get => _cancelCommand;
            set => SetProperty(ref _cancelCommand, value);
        }

        private void CalendarEntriesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsOkButtonEnabled = CalendarDefinitionViewModel.CalendarViewModel.CalendarEntries.Count > 0;
        }
    }
}