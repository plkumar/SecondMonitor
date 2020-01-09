namespace SecondMonitor.Timing.Presentation.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using WindowsControls.WPF;
    using DataModel.BasicProperties;
    using DataModel.DriversPresentation;
    using DataModel.Extensions;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using NLog;
    using SessionTiming.Drivers.Presentation.ViewModel;
    using SessionTiming.Drivers.ViewModel;
    using SessionTiming.ViewModel;
    using SimdataManagement.DriverPresentation;
    using ViewModels;
    using ViewModels.Colors;
    using ViewModels.Settings.Model;
    using ViewModels.Settings.ViewModel;

    public class TimingDataGridViewModel : AbstractViewModel, IPositionCircleInformationProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly DriverPresentationsManager _driverPresentationsManager;
        private readonly DisplaySettingsViewModel _displaySettingsViewModel;
        private readonly IClassColorProvider _classColorProvider;
        private readonly object _lockObject = new object();
        private readonly Dictionary<string, DriverTiming> _driverNameTimingMap;
        private int _loadIndex;
        private readonly Stopwatch _refreshGapWatch;
        private Task _pitBoardTask;
        private CancellationTokenSource _cancellationTokenSource;

        public TimingDataGridViewModel(DriverPresentationsManager driverPresentationsManager, DisplaySettingsViewModel displaySettingsViewModel, IClassColorProvider classColorProvider )
        {
            _refreshGapWatch = Stopwatch.StartNew();
            _loadIndex = 0;
            _driverNameTimingMap = new Dictionary<string, DriverTiming>();
            _driverPresentationsManager = driverPresentationsManager;
            _displaySettingsViewModel = displaySettingsViewModel;
            _classColorProvider = classColorProvider;
            DriversViewModels = new ObservableCollection<DriverTimingViewModel>();
            PitBoardViewModel = new PitBoardViewModel();
            PitBoardViewModel.PitBoard.Lap = "L0";

            _driverPresentationsManager.DriverCustomColorChanged += DriverPresentationsManagerOnDriverCustomColorEnabledChanged;
        }

        public ObservableCollection<DriverTimingViewModel> DriversViewModels { get; }

        public DisplayModeEnum DriversOrdering { get; set; }

        public DriverTimingViewModel PlayerViewModel { get; set; }

        public PitBoardViewModel PitBoardViewModel { get; private set; }

        public void UpdateProperties(SimulatorDataSet dataSet)
        {
            if (_loadIndex > 0)
            {
                return;
            }
            lock (_lockObject)
            {
                List<DriverTiming> orderedTimings = (DriversOrdering == DisplayModeEnum.Absolute ? _driverNameTimingMap.Values.OrderBy(x => x.Position) : _driverNameTimingMap.Values.OrderBy(x => x.DistanceToPlayer)).ToList();
                for (int i = 0; i < orderedTimings.Count; i++)
                {
                    RebindViewModel(DriversViewModels[i], orderedTimings[i]);
                    if (DriversViewModels[i].IsPlayer)
                    {
                        PlayerViewModel = DriversViewModels[i];
                    }
                }
                DriversViewModels.ForEach(x => x.RefreshProperties());

                if (_refreshGapWatch.ElapsedMilliseconds < 500)
                {
                    return;
                }

                UpdateGapsSize(dataSet);
                _refreshGapWatch.Restart();
            }
        }

        private void RebindViewModel(DriverTimingViewModel driverTimingViewModel, DriverTiming driverTiming)
        {
            if(driverTimingViewModel.DriverTiming == driverTiming)
            {
                return;
            }

            if (driverTimingViewModel.DriverTiming.CarClassId != driverTiming.CarClassId)
            {
                driverTimingViewModel.ClassIndicationBrush = _classColorProvider.GetColorForClass(driverTiming.CarClassId);
            }

            driverTimingViewModel.OutLineColor = GetDriverOutline(driverTiming.Name);
            driverTimingViewModel.DriverTiming = driverTiming;
        }

        private ColorDto GetDriverOutline(string driverName)
        {
            _driverPresentationsManager.TryGetDriverPresentation(driverName, out DriverPresentationDto driverPresentationDto);
            return driverPresentationDto?.CustomOutLineEnabled == true ? driverPresentationDto.OutLineColor  : null;
        }

        private void UpdateGapsSize(SimulatorDataSet dataSet)
        {
            if (dataSet?.SessionInfo == null || dataSet.SessionInfo.SessionType != SessionType.Race || DriversOrdering != DisplayModeEnum.Relative)
            {
                return;
            }

            bool isVisualizationEnabled = _displaySettingsViewModel.IsGapVisualizationEnabled;
            double minimalGap = _displaySettingsViewModel.MinimalGapForVisualization;
            double heightPerSecond = _displaySettingsViewModel.GapHeightForOneSecond;
            double maximumGap = _displaySettingsViewModel.MaximumGapHeight;

            DriverTimingViewModel previousViewModel = null;
            foreach (DriverTimingViewModel driverTimingViewModel in DriversViewModels)
            {
                if (previousViewModel == null)
                {
                    previousViewModel = driverTimingViewModel;
                    continue;
                }

                double gapInSeconds = Math.Abs(driverTimingViewModel.GapToPlayer.TotalSeconds - previousViewModel.GapToPlayer.TotalSeconds);
                double gapInSecondsWithMinimal = gapInSeconds - minimalGap;
                if (!isVisualizationEnabled || gapInSecondsWithMinimal < 0 )
                {
                    previousViewModel.GapHeight = 0;
                    previousViewModel = driverTimingViewModel;
                    continue;
                }

                previousViewModel.GapToNextDriver = gapInSeconds;
                previousViewModel.GapHeight = Math.Min(gapInSecondsWithMinimal * heightPerSecond, maximumGap);
                previousViewModel = driverTimingViewModel;

            }
        }

        public void RemoveDriver(DriverTiming driver)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => RemoveDriver(driver));
                return;
            }

            lock (_lockObject)
            {
                DriverTimingViewModel toRemove = DriversViewModels.FirstOrDefault(x => x.DriverTiming == driver);
                if (toRemove == null)
                {
                    return;
                }

                _driverNameTimingMap.Remove(driver.Name);
                DriversViewModels.Remove(toRemove);
            }

        }

        public async Task UpdateAndShowPitBoard()
        {
            if (!_displaySettingsViewModel.PitBoardSettingsViewModel.IsEnabled)
            {
                return;
            }

            PitBoardViewModel.UpdatePitBoard(DriversViewModels.ToArray());
            if (_pitBoardTask != null)
            {
                try
                {
                    _cancellationTokenSource.Cancel();
                    await _pitBoardTask;
                }
                catch (TaskCanceledException)
                {

                }
            }
            _cancellationTokenSource = new CancellationTokenSource();
            _pitBoardTask = PitBoardViewModel.ShowPitBoard(TimeSpan.FromSeconds(_displaySettingsViewModel.PitBoardSettingsViewModel.DisplaySeconds), _cancellationTokenSource.Token);
        }

        /*private void RemoveAllDrivers()
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(RemoveAllDrivers);
                return;
            }

            lock (_lockObject)
            {
                _driverNameTimingMap.Clear();
                DriversViewModels.Clear();
            };
        }*/

        public void AddDriver(DriverTiming driver)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => AddDriver(driver));
                return;
            }

            DriverTimingViewModel newViewModel = new DriverTimingViewModel(driver) {ClassIndicationBrush = GetClassColor(driver.DriverInfo)};
            lock (_lockObject)
            {
                //If possible, rebind - do not create new
                if (_driverNameTimingMap.ContainsKey(driver.Name))
                {
                    _driverNameTimingMap[driver.Name] = driver;
                    RebindViewModel(DriversViewModels.First(x => x.Name == driver.Name), driver);
                    return;
                }
                _driverNameTimingMap[driver.Name] = driver;
                newViewModel.OutLineColor = GetDriverOutline(driver.Name);
                DriversViewModels.Add(newViewModel);
            }
        }

        public void MatchDriversList(List<DriverTiming> drivers)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => MatchDriversList(drivers));
                return;
            }
            PitBoardViewModel.Reset();

            lock (_lockObject)
            {
                List<DriverTiming> driversToRemove = _driverNameTimingMap.Values.Where(x => drivers.FirstOrDefault(y => y.Name == x.Name) == null).ToList();
                List<DriverTiming> driversToAdd = drivers.Where(x => !_driverNameTimingMap.ContainsKey(x.Name)).ToList();
                List<DriverTiming> driversToRebind = drivers.Where(x => _driverNameTimingMap.ContainsKey(x.Name)).ToList();

                foreach (DriverTiming driverToRebind in driversToRebind)
                {
                    _driverNameTimingMap[driverToRebind.Name] = driverToRebind;
                    RebindViewModel(DriversViewModels.First(x => x.Name == driverToRebind.Name), driverToRebind);
                }

                int i = 0;
                for (i = 0; i < driversToAdd.Count && i < driversToRemove.Count; i++)
                {
                    _driverNameTimingMap.Remove(driversToRemove[i].Name);
                    _driverNameTimingMap.Add(driversToAdd[i].Name, driversToAdd[i]);
                    RebindViewModel(DriversViewModels.First(x => x.Name == driversToRemove[i].Name), driversToAdd[i]);
                }

                driversToRemove.Skip(i).ForEach(RemoveDriver);
                AddDrivers(driversToAdd.Skip(i));
            }
        }

        private void AddDrivers(IEnumerable<DriverTiming> drivers)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => AddDrivers(drivers));
                return;
            }

            try
            {
                _loadIndex++;
                Logger.Info("Add Drivers Called");
                List<DriverTimingViewModel> newViewModels = drivers.Select(x => new DriverTimingViewModel(x)
                {
                    ClassIndicationBrush = GetClassColor(x.DriverInfo),
                    OutLineColor = GetDriverOutline(x.Name)
                }).ToList();

                foreach (DriverTimingViewModel driverTimingViewModel in newViewModels)
                {
                    lock (_lockObject)
                    {
                        if (_driverNameTimingMap.ContainsKey(driverTimingViewModel.Name))
                        {
                            continue;
                        }
                        _driverNameTimingMap[driverTimingViewModel.Name] = driverTimingViewModel.DriverTiming;
                        DriversViewModels.Add(driverTimingViewModel);
                    }
                }

                _loadIndex--;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            Logger.Info("Add Drivers Completed");
        }

        public bool IsDriverOnValidLap(IDriverInfo driver)
        {
            if (driver?.DriverName == null)
            {
                return false;
            }

            lock (_lockObject)
            {
                if (_driverNameTimingMap.TryGetValue(driver.DriverName, out DriverTiming timing))
                {
                    return timing.CurrentLap?.Valid ?? false;
                }
            }

            return false;
        }

        public bool IsDriverLastSectorGreen(IDriverInfo driver, int sectorNumber)
        {
            if (driver?.DriverName == null)
            {
                return false;
            }

            DriverTiming timing;
            lock (_lockObject)
            {
                if (!_driverNameTimingMap.TryGetValue(driver.DriverName, out timing))
                {
                    return false;
                }
            }

            switch (sectorNumber)
            {
                case 1:
                    return timing.IsLastSector1PersonalBest;
                case 2:
                    return timing.IsLastSector2PersonalBest;
                case 3:
                    return timing.IsLastSector3PersonalBest;
                default:
                    return false;
            }
        }

        public bool IsDriverLastSectorPurple(IDriverInfo driver, int sectorNumber)
        {
            if (driver?.DriverName == null)
            {
                return false;
            }
            DriverTiming timing;
            lock (_lockObject)
            {
                if (!_driverNameTimingMap.TryGetValue(driver.DriverName, out timing))
                {
                    return false;
                }
            }

            switch (sectorNumber)
            {
                case 1:
                    return timing.IsLastSector1SessionBest;
                case 2:
                    return timing.IsLastSector2SessionBest;
                case 3:
                    return timing.IsLastSector3SessionBest;
                default:
                    return false;
            }
        }

        public bool TryGetCustomOutline(IDriverInfo driverInfo, out ColorDto outlineColor)
        {
            bool driverHasCustomOutline = _driverPresentationsManager.TryGetDriverPresentation(driverInfo.DriverName, out DriverPresentationDto driverPresentationDto);
            outlineColor = driverPresentationDto?.OutLineColor;
            return driverHasCustomOutline && driverPresentationDto?.CustomOutLineEnabled == true;


        }

        public ColorDto GetClassColor(IDriverInfo driverInfo)
        {
            return _classColorProvider.GetColorForClass(driverInfo.CarClassId);
        }

        private void DriverPresentationsManagerOnDriverCustomColorEnabledChanged(object sender, DriverCustomColorEnabledArgs e)
        {
            ColorDto colorToSet = e.IsCustomOutlineEnabled ? e.DriverColor : null;
            DriversViewModels.Where(x => x.Name == e.DriverName).ForEach(x => x.OutLineColor = colorToSet);
        }
    }
}