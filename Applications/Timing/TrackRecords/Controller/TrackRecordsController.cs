namespace SecondMonitor.Timing.TrackRecords.Controller
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Contracts.Commands;
    using Contracts.TrackRecords;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.TrackRecords;
    using SessionTiming.Drivers.ViewModel;
    using ViewModels;
    using ViewModels.Factory;
    using ViewModels.Settings;
    using ViewModels.TrackRecords;

    public class TrackRecordsController : ITrackRecordsController, ITrackRecordsProvider
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IWindowService _windowService;
        private readonly TrackRecordsRepository _trackRecordsRepository;
        private SimulatorsRecords _simulatorsRecords;
        private SimulatorRecords _currentSimulatorRecords;
        private TrackRecord _currentTrackRecords;
        private NamedRecordSet _currentVehicleRecordSet;
        private NamedRecordSet _currentClassRecordSet;
        private bool _initialized;
        private bool _isEnabled;
        private bool _showRecordsForSession;
        private SessionType _currentSessionType;

        public TrackRecordsController(ISettingsProvider settingsProvider, ITrackRecordsRepositoryFactory trackRecordsRepositoryFactory, IViewModelFactory viewModelFactory, IWindowService windowService)
        {
            TrackRecordsViewModel = viewModelFactory.Create<ITrackRecordsViewModel>();
            _settingsProvider = settingsProvider;
            _viewModelFactory = viewModelFactory;
            _windowService = windowService;
            _trackRecordsRepository = trackRecordsRepositoryFactory.Create();
            BindCommands();
        }

        public Task StartControllerAsync()
        {
            _simulatorsRecords = _trackRecordsRepository.LoadRatingsOrCreateNew();
            _settingsProvider.DisplaySettingsViewModel.TrackRecordsSettingsViewModel.PropertyChanged += TrackRecordsSettingsViewModelOnPropertyChanged;
            InitializeFromSettings();
            return Task.CompletedTask;
        }

        private void InitializeFromSettings()
        {
            _showRecordsForSession = _settingsProvider.DisplaySettingsViewModel.TrackRecordsSettingsViewModel.DisplayRecordForCurrentSessionType;
            _isEnabled = _settingsProvider.DisplaySettingsViewModel.TrackRecordsSettingsViewModel.IsEnabled;;
            TrackRecordsViewModel.IsVisible = _isEnabled;
            RefreshViewModel();
        }

        private void TrackRecordsSettingsViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InitializeFromSettings();
        }

        public Task StopControllerAsync()
        {
            _settingsProvider.DisplaySettingsViewModel.TrackRecordsSettingsViewModel.PropertyChanged -= TrackRecordsSettingsViewModelOnPropertyChanged;
            _trackRecordsRepository.Save(_simulatorsRecords);
            return Task.CompletedTask;
        }

        public ITrackRecordsViewModel TrackRecordsViewModel { get; }

        public void OnSessionStarted(SimulatorDataSet dataSet)
        {
            InitializeCurrentSets(dataSet);
        }

        public void OnDataLoaded(SimulatorDataSet dataSet)
        {
            if (_initialized && dataSet.SessionInfo.SessionType == _currentSessionType)
            {
                return;
            }

            InitializeCurrentSets(dataSet);
        }

        public bool EvaluateFastestLapCandidate(ILapInfo lapInfo)
        {
            if (!lapInfo.Valid || !lapInfo.Driver.IsPlayer || !_isEnabled)
            {
                return false;
            }

            bool isTrackRecord = EvaluateAsTrackRecord(lapInfo);
            bool isClassRecord = EvaluateAsClassRecord(lapInfo);
            bool isVehicleRecord = EvaluateAsVehicleRecord(lapInfo);

            TrackRecordsViewModel.TrackRecord.IsHighlighted = isTrackRecord;
            TrackRecordsViewModel.ClassRecord.IsHighlighted = isClassRecord;
            TrackRecordsViewModel.VehicleRecord.IsHighlighted = isVehicleRecord;

            if (isVehicleRecord || isClassRecord || isTrackRecord)
            {
                RefreshViewModel();
            }

            return isVehicleRecord || isClassRecord || isTrackRecord;
        }

        public bool TryGetOverallBestRecord(string simulatorName, string trackFullName, SessionType sessionType, out RecordEntryDto recordEntry)
        {
            var simulatorSet = _simulatorsRecords.GetOrCreateSimulatorRecords(simulatorName);
            var trackRecordSet = simulatorSet.TrackRecords.FirstOrDefault(x => x.TrackName == trackFullName);
            if (trackRecordSet == null)
            {
                recordEntry = null;
                return false;
            }

            recordEntry = trackRecordSet.OverallRecord.GetProperEntry(sessionType);
            return recordEntry != null;
        }

        public bool TryGetCarBestRecord(string simulatorName, string trackFullName, string carName, SessionType sessionType, out RecordEntryDto recordEntry)
        {
            var simulatorSet = _simulatorsRecords.GetOrCreateSimulatorRecords(simulatorName);
            var trackRecordSet = simulatorSet.TrackRecords.FirstOrDefault(x => x.TrackName == trackFullName);
            if (trackRecordSet == null)
            {
                recordEntry = null;
                return false;
            }

            var carRecord = trackRecordSet.GetOrCreateVehicleRecord(carName);
            recordEntry = carRecord.GetProperEntry(sessionType);
            return recordEntry != null;
        }

        public bool TryGetClassBestRecord(string simulatorName, string trackFullName, string className, SessionType sessionType, out RecordEntryDto recordEntry)
        {
            var simulatorSet = _simulatorsRecords.GetOrCreateSimulatorRecords(simulatorName);
            var trackRecordSet = simulatorSet.TrackRecords.FirstOrDefault(x => x.TrackName == trackFullName);
            if (trackRecordSet == null)
            {
                recordEntry = null;
                return false;
            }

            var carRecord = trackRecordSet.GetOrCreateClassRecord(className);
            recordEntry = carRecord.GetProperEntry(sessionType);
            return recordEntry != null;
        }

        private bool EvaluateAsTrackRecord(ILapInfo lapInfo)
        {
            var sessionType = lapInfo.Driver.Session.SessionType;
            if (Evaluate(lapInfo, _currentTrackRecords.OverallRecord.GetProperEntry(sessionType)))
            {
                var newRecordEntry = FromLap(lapInfo);
                _currentTrackRecords.OverallRecord.SetProperEntry(sessionType, newRecordEntry);
                return GetCurrentTrackRecord() == newRecordEntry;
            }

            return false;
        }

        private bool EvaluateAsVehicleRecord(ILapInfo lapInfo)
        {
            var sessionType = lapInfo.Driver.Session.SessionType;
            if (Evaluate(lapInfo, _currentVehicleRecordSet.GetProperEntry(sessionType)))
            {
                var newRecordEntry = FromLap(lapInfo);
                _currentVehicleRecordSet.SetProperEntry(sessionType, newRecordEntry);
                return GetCurrentVehicleRecord() == newRecordEntry;
            }

            return false;
        }

        private bool EvaluateAsClassRecord(ILapInfo lapInfo)
        {
            var sessionType = lapInfo.Driver.Session.SessionType;
            if (Evaluate(lapInfo, _currentClassRecordSet.GetProperEntry(sessionType)))
            {
                var newRecordEntry = FromLap(lapInfo);
                _currentClassRecordSet.SetProperEntry(sessionType, newRecordEntry);
                return GetCurrentClassRecord() == newRecordEntry;
            }

            return false;
        }

        private bool Evaluate(ILapInfo lapInfo, RecordEntryDto recordEntryDto)
        {
            if (recordEntryDto == null || recordEntryDto.LapTime > lapInfo.LapTime)
            {
                return true;
            }

            return false;
        }

        private void InitializeCurrentSets(SimulatorDataSet dataSet)
        {
            _currentSessionType = dataSet.SessionInfo.SessionType;

            if(dataSet.SessionInfo.SessionType == SessionType.Na || string.IsNullOrEmpty(dataSet.Source) || dataSet.PlayerInfo == null)
            {
                _initialized = false;
                return;
            }

            _currentSimulatorRecords = _simulatorsRecords.GetOrCreateSimulatorRecords(dataSet.Source);
            _currentTrackRecords = _currentSimulatorRecords.GetOrCreateTrackRecord(dataSet.SessionInfo.TrackInfo.TrackFullName);
            _currentVehicleRecordSet = _currentTrackRecords.GetOrCreateVehicleRecord(dataSet.PlayerInfo.CarName);
            _currentClassRecordSet = _currentTrackRecords.GetOrCreateClassRecord(dataSet.PlayerInfo.CarClassName);

            RefreshViewModel();
            TrackRecordsViewModel.TrackRecord.IsHighlighted = false;
            TrackRecordsViewModel.ClassRecord.IsHighlighted = false;
            TrackRecordsViewModel.VehicleRecord.IsHighlighted = false;

        }

        private void RefreshViewModel()
        {
            if (_currentTrackRecords == null || _currentClassRecordSet == null || _currentVehicleRecordSet == null)
            {
                return;
            }
            TrackRecordsViewModel.TrackRecord.FromModel(GetCurrentTrackRecord());
            TrackRecordsViewModel.ClassRecord.FromModel(GetCurrentClassRecord());
            TrackRecordsViewModel.VehicleRecord.FromModel(GetCurrentVehicleRecord());
        }

        private RecordEntryDto GetCurrentVehicleRecord()
        {
            return GetCorrectRecordEntry(_currentVehicleRecordSet);
        }

        private RecordEntryDto GetCurrentClassRecord()
        {
            return GetCorrectRecordEntry(_currentClassRecordSet);
        }

        private RecordEntryDto GetCurrentTrackRecord()
        {
            return GetCorrectRecordEntry(_currentTrackRecords.OverallRecord);
        }

        private RecordEntryDto GetCorrectRecordEntry(RecordSet recordSet)
        {
            return _showRecordsForSession ?  recordSet.GetProperEntry(_currentSessionType) : recordSet.GetOverAllBest();
        }

        private RecordEntryDto FromLap(ILapInfo lapInfo)
        {
            return new RecordEntryDto()
            {
                CarClass = lapInfo.Driver.CarClassName,
                CarName = lapInfo.Driver.CarName,
                IsPlayer = true,
                LapTime = lapInfo.LapTime,
                PlayerName = lapInfo.Driver.Name,
                RecordDate = DateTime.Now,
                SessionType = lapInfo.Driver.Session.SessionType
            };
        }

        private void BindCommands()
        {
            TrackRecordsViewModel.OpenTracksRecordsCommand = new RelayCommand(OpenTracksRecords);
            TrackRecordsViewModel.OpenClassRecordsCommand = new RelayCommand(OpenClassRecords);
            TrackRecordsViewModel.OpenVehiclesRecordsCommands = new RelayCommand(OpenVehicleRecords);
        }

        private void OpenTracksRecords()
        {
            if (_currentSimulatorRecords == null)
            {
                return;
            }

            var trackRecordsOrdered = _currentSimulatorRecords.TrackRecords.OrderBy(x => x.TrackName);
            var simulatorRecordsViewModel = _viewModelFactory.Create<SimulatorRecordsViewModel>();
            simulatorRecordsViewModel.Title = "Track Records for " + _currentSimulatorRecords.SimulatorName + " (" + (_showRecordsForSession ? _currentSessionType.ToString() : "All Sessions" ) + ")";
            List<TrackRecordViewModel> trackRecords =  new List<TrackRecordViewModel>(_currentSimulatorRecords.TrackRecords.Count);
            foreach (TrackRecord trackRecord in trackRecordsOrdered)
            {
                RecordEntryDto record = GetCorrectRecordEntry(trackRecord.OverallRecord);
                if (record == null)
                {
                    continue;
                }
                TrackRecordViewModel newViewModel = _viewModelFactory.Create<TrackRecordViewModel>();
                newViewModel.CarName = record.CarName;
                newViewModel.ClassName = record.CarClass;
                newViewModel.BestTime = record.LapTime;
                newViewModel.RecordSetDate = record.RecordDate;
                newViewModel.TrackName = trackRecord.TrackName;
                newViewModel.SessionType = record.SessionType.ToString();
                trackRecords.Add(newViewModel);
            }

            simulatorRecordsViewModel.ChildViewModels = trackRecords;
            _windowService.OpenWindow(simulatorRecordsViewModel, simulatorRecordsViewModel.Title, WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner);
        }

        private void OpenVehicleRecords()
        {
            if (_currentTrackRecords == null)
            {
                return;
            }

            var records = _currentTrackRecords.VehicleRecords.Where(x => GetCorrectRecordEntry(x) != null).OrderBy(x => GetCorrectRecordEntry(x).LapTime);
            var carRecordsViewModel = _viewModelFactory.Create<CarRecordsCollectionViewModel>();
            carRecordsViewModel.Title = "Vehicle Records for " + _currentTrackRecords.TrackName + " (" + (_showRecordsForSession ? _currentSessionType.ToString() : "All Sessions") + ")";

            List<CarRecordViewModel> trackRecords = new List<CarRecordViewModel>(_currentTrackRecords.VehicleRecords.Count);
            foreach (NamedRecordSet classRecord in records)
            {
                RecordEntryDto record = GetCorrectRecordEntry(classRecord);
                CarRecordViewModel newViewModel = _viewModelFactory.Create<CarRecordViewModel>();
                newViewModel.CarName = record.CarName;
                newViewModel.ClassName = record.CarClass;
                newViewModel.BestTime = record.LapTime;
                newViewModel.RecordSetDate = record.RecordDate;
                newViewModel.SessionType = record.SessionType.ToString();
                trackRecords.Add(newViewModel);
            }

            carRecordsViewModel.ChildViewModels = trackRecords;
            _windowService.OpenWindow(carRecordsViewModel, carRecordsViewModel.Title, WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner);
        }

        private void OpenClassRecords()
        {
            if (_currentTrackRecords == null)
            {
                return;
            }

            var records = _currentTrackRecords.ClassRecords.Where(x => GetCorrectRecordEntry(x) != null).OrderBy(x => GetCorrectRecordEntry(x).LapTime);
            var carRecordsViewModel = _viewModelFactory.Create<CarRecordsCollectionViewModel>();
            carRecordsViewModel.Title = "Class Records for " + _currentTrackRecords.TrackName + " (" + (_showRecordsForSession ? _currentSessionType.ToString() : "All Sessions") + ")";

            List<CarRecordViewModel> trackRecords = new List<CarRecordViewModel>(_currentTrackRecords.ClassRecords.Count);
            foreach (NamedRecordSet classRecord in records)
            {
                RecordEntryDto record = GetCorrectRecordEntry(classRecord);
                CarRecordViewModel newViewModel = _viewModelFactory.Create<CarRecordViewModel>();
                newViewModel.CarName = record.CarName;
                newViewModel.ClassName = record.CarClass;
                newViewModel.BestTime = record.LapTime;
                newViewModel.RecordSetDate = record.RecordDate;
                newViewModel.SessionType = record.SessionType.ToString();
                trackRecords.Add(newViewModel);
            }

            carRecordsViewModel.ChildViewModels = trackRecords;
            _windowService.OpenWindow(carRecordsViewModel, carRecordsViewModel.Title, WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner);
        }
    }
}