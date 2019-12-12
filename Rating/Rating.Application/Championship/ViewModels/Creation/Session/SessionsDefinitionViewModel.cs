namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Session
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Contracts.Commands;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class SessionsDefinitionViewModel : AbstractViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ISessionDefinitionViewModelFactory _sessionDefinitionViewModelFactory;
        private bool _isAddSessionCommandEnabled;
        private bool _isRemoveSessionCommandEnabled;

        public SessionsDefinitionViewModel(IViewModelFactory viewModelFactory, ISessionDefinitionViewModelFactory sessionDefinitionViewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            _sessionDefinitionViewModelFactory = sessionDefinitionViewModelFactory;
            SessionsDefinitions = new ObservableCollection<SessionDefinitionViewModel> {_sessionDefinitionViewModelFactory.CreateBase()};
            AddSessionCommand = new RelayCommand(AddNewSession);
            RemoveSessionCommand = new RelayCommand(RemoveLastSession);
            SetDefaultSessionNames();
            RefreshCommandAvailability();
        }

        public bool IsAddSessionCommandEnabled
        {
            get => _isAddSessionCommandEnabled;
            set => SetProperty(ref _isAddSessionCommandEnabled, value);
        }

        public bool IsRemoveSessionCommandEnabled
        {
            get => _isRemoveSessionCommandEnabled;
            set => SetProperty(ref _isRemoveSessionCommandEnabled, value);
        }

        public ObservableCollection<SessionDefinitionViewModel> SessionsDefinitions { get; }

        public ICommand AddSessionCommand { get; set; }

        public ICommand RemoveSessionCommand { get; set; }

        public void SetDefaultSessionNames()
        {
            for (int i = 0; i < SessionsDefinitions.Count; i++)
            {
                SessionsDefinitions[i].OriginalSessionName = "Race " + (i + 1);
            }
        }

        private void AddNewSession()
        {
            SessionsDefinitions.Add(_sessionDefinitionViewModelFactory.CreateBase());
            SetDefaultSessionNames();
            RefreshCommandAvailability();
        }

        private void RemoveLastSession()
        {
            SessionsDefinitions.RemoveAt(SessionsDefinitions.Count -1 );
            RefreshCommandAvailability();
        }

        private void RefreshCommandAvailability()
        {
            IsAddSessionCommandEnabled = SessionsDefinitions.Count < 4;
            IsRemoveSessionCommandEnabled = SessionsDefinitions.Count > 1;
        }
    }
}