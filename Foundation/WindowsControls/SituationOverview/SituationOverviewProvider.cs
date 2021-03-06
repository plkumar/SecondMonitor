﻿namespace SecondMonitor.WindowsControls.SituationOverview
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Contracts.Commands;
    using Contracts.TrackMap;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using DataModel.TrackMap;
    using NLog;
    using ViewModels;
    using ViewModels.Settings.ViewModel;
    using WPF;
    using WPF.DriverPosition;

    public class SituationOverviewProvider : DependencyObject, ISimulatorDataSetViewModel, INotifyPropertyChanged, IMapSidePanelViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ResourceDictionary _commonResources;
        private ISituationOverviewControl _situationOverviewControl;
        private IPositionCircleInformationProvider _positionCircleInformationProvider;
        private IMapManagementController _mapManagementController;
        private (string trackName, string layoutName, string simName) _currentTrackTuple;
        private DisplaySettingsViewModel _displaySettingsViewModel;
        private bool _alwaysUseCircle;


        public SituationOverviewProvider(IPositionCircleInformationProvider positionCircleInformation, DisplaySettingsViewModel displaySettingsViewModel)
        {
            _commonResources = new ResourceDictionary
                                   {
                                       Source = new Uri(
                                           @"pack://application:,,,/WindowsControls;component/WPF/CommonResources.xaml",
                                           UriKind.RelativeOrAbsolute)
                                   };
            _displaySettingsViewModel = DisplaySettingsViewModel;
            PositionCircleInformationProvider = positionCircleInformation;
        }

        public ICommand DeleteMapCommand  => new RelayCommand(RemoveCurrentMap);
        public ICommand RotateMapLeftCommand => new RelayCommand(RotateCurrentMapLeft);
        public ICommand RotateMapRightCommand  => new RelayCommand(RotateCurrentMapRight);

        public ISituationOverviewControl SituationOverviewControl
        {
            get => _situationOverviewControl;
            private set
            {
                _situationOverviewControl = value;
                NotifyPropertyChanged();
            }
        }

        public IPositionCircleInformationProvider PositionCircleInformationProvider
        {
            get => _positionCircleInformationProvider;

            set
            {
                _positionCircleInformationProvider = value;
                if(SituationOverviewControl != null)
                {
                    SituationOverviewControl.PositionCircleInformationProvider = value;
                }
            }
        }

        public IMapManagementController MapManagementController
        {
            get => _mapManagementController;
            set
            {
                UnsubscribeMapManager();
                _mapManagementController = value;
                SubscribeMapManager();
            }
        }

        public DisplaySettingsViewModel DisplaySettingsViewModel
        {
            get => _displaySettingsViewModel;
            set
            {
                UnSubscribeDisplaySettings();
                _displaySettingsViewModel = value;
                ApplyDisplaySettings();
                SubscribeDisplaySettings();
            }
        }

        private bool AlwaysUseCircle
        {
            get => _alwaysUseCircle;
            set
            {
                if (_alwaysUseCircle == value)
                {
                    return;
                }

                _alwaysUseCircle = value;
                LoadCurrentMap();
            }
        }

        private void ApplyDisplaySettings()
        {
            if (_displaySettingsViewModel == null )
            {
                return;
            }

            AlwaysUseCircle = _displaySettingsViewModel.MapDisplaySettingsViewModel.AlwaysUseCirce;

            if (MapManagementController != null)
            {
                MapManagementController.MapPointsInterval = TimeSpan.FromMilliseconds(DisplaySettingsViewModel.MapDisplaySettingsViewModel.MapPointsInterval);
            }

            if (SituationOverviewControl == null)
            {
                return;
            }

            SituationOverviewControl.AnimateDriversPos = DisplaySettingsViewModel.AnimateDriversPosition;

            if (SituationOverviewControl is FullMapControl fullMapControl)
            {
                fullMapControl.AutoScaleDriverControls = DisplaySettingsViewModel.MapDisplaySettingsViewModel.AutoScaleDrivers;
                fullMapControl.KeepMapRatio = DisplaySettingsViewModel.MapDisplaySettingsViewModel.KeepMapRatio;
            }


        }

        public void ApplyDateSet(SimulatorDataSet dataSet)
        {
            if (_currentTrackTuple.trackName != dataSet.SessionInfo.TrackInfo.TrackName || _currentTrackTuple.layoutName != dataSet.SessionInfo.TrackInfo.TrackLayoutName || _currentTrackTuple.simName != dataSet.Source)
            {
                _currentTrackTuple = (dataSet.SessionInfo.TrackInfo.TrackName, dataSet.SessionInfo.TrackInfo.TrackLayoutName, dataSet.Source);
                LoadCurrentMap();
            }

            SituationOverviewControl?.UpdateDrivers(dataSet, dataSet.DriversInfo);
        }

        public void Reset()
        {
            _situationOverviewControl?.RemoveAllDrivers();
        }

        public void RemoveDriver(DriverInfo driver)
        {
            _situationOverviewControl?.RemoveDrivers(driver);
        }

        public void AddDriver(DriverInfo driver)
        {
            _situationOverviewControl?.AddDrivers(driver);
        }

        private PositionCircleControl InitializePositionCircle()
        {
            Logger.Info("Initializing Position Circle");
            return new PositionCircleControl
            {
                PositionCircleInformationProvider = PositionCircleInformationProvider,

                DriverBackgroundBrush = (SolidColorBrush)_commonResources["DriverBackgroundColor"],
                DriverForegroundBrush = (SolidColorBrush)_commonResources["DriverForegroundColor"],

                DriverPitsBackgroundBrush = (SolidColorBrush)_commonResources["InPitsBrush"],
                DriverPitsMovingBackground = (SolidColorBrush)_commonResources["InPitsMovingBrush"],
                DriverPitsForegroundBrush = (SolidColorBrush)_commonResources["DriverPitsForegroundColor"],

                PlayerBackgroundBrush = (SolidColorBrush)_commonResources["PlayerBackgroundColor"],
                PlayerForegroundBrush = (SolidColorBrush)_commonResources["PlayerForegroundColor"],

                LappedDriverBackgroundBrush = (SolidColorBrush)_commonResources["TimingLappedBrush"],
                LappedDriverForegroundBrush = (SolidColorBrush)_commonResources["TimingLappedForegroundBrush"],

                LappingDriverBackgroundBrush = (SolidColorBrush)_commonResources["TimingLappingBrush"],
                LappingDriverForegroundBrush = (SolidColorBrush)_commonResources["TimingLappingForegroundBrush"],

                PlayerOutLineBrush = (SolidColorBrush) _commonResources["PlayerOutLineColor"],

                AnimateDriversPos = DisplaySettingsViewModel.AnimateDriversPosition
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SubscribeMapManager()
        {
            if (_mapManagementController == null)
            {
                return;
            }

            _mapManagementController.NewMapAvailable += OnNewMapAvailable;
            _mapManagementController.MapRemoved += OnMapRemoved;
        }

        private void UnsubscribeMapManager()
        {
            if (_mapManagementController == null)
            {
                return;
            }

            _mapManagementController.NewMapAvailable -= OnNewMapAvailable;
            _mapManagementController.MapRemoved -= OnMapRemoved;
        }

        private void SubscribeDisplaySettings()
        {
            if (_displaySettingsViewModel == null)
            {
                return;
            }

            _displaySettingsViewModel.PropertyChanged += OnDisplaySettingsChanged;
            _displaySettingsViewModel.MapDisplaySettingsViewModel.PropertyChanged += OnDisplaySettingsChanged;
        }

        private void UnSubscribeDisplaySettings()
        {
            if (_displaySettingsViewModel == null)
            {
                return;
            }

            _displaySettingsViewModel.PropertyChanged -= OnDisplaySettingsChanged;
            _displaySettingsViewModel.MapDisplaySettingsViewModel.PropertyChanged -= OnDisplaySettingsChanged;
        }

        private void OnDisplaySettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            ApplyDisplaySettings();
        }


        private void LoadCurrentMap()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(LoadCurrentMap);
            }
            if (string.IsNullOrEmpty(_currentTrackTuple.trackName) || _mapManagementController == null)
            {
                return;
            }
            Logger.Info("Loading New Map");
            if (AlwaysUseCircle || !_mapManagementController.TryGetMap(_currentTrackTuple.simName, _currentTrackTuple.trackName, _currentTrackTuple.layoutName, out TrackMapDto trackMapDto))
            {
                SituationOverviewControl = InitializePositionCircle();
                if (!AlwaysUseCircle)
                {
                    SituationOverviewControl.AdditionalInformation = $"No Valid Map for {_currentTrackTuple.trackName}\nComplete one valid lap for full map";
                }
            }
            else
            {
                SituationOverviewControl = InitializeFullMap(trackMapDto);
            }
        }

        private FullMapControl InitializeFullMap(ITrackMap trackMapDto)
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => InitializeFullMap(trackMapDto));
            }

            Logger.Info("Initializing Full Map Control");

            return new FullMapControl(trackMapDto)
            {
                PositionCircleInformationProvider = PositionCircleInformationProvider,

                DriverBackgroundBrush = (SolidColorBrush) _commonResources["DriverBackgroundColor"],
                DriverForegroundBrush = (SolidColorBrush) _commonResources["DriverForegroundColor"],

                DriverPitsBackgroundBrush = (SolidColorBrush) _commonResources["InPitsBrush"],
                DriverPitsMovingBackground = (SolidColorBrush)_commonResources["InPitsMovingBrush"],
                DriverPitsForegroundBrush = (SolidColorBrush) _commonResources["DriverPitsForegroundColor"],

                PlayerBackgroundBrush = (SolidColorBrush) _commonResources["PlayerBackgroundColor"],
                PlayerForegroundBrush = (SolidColorBrush) _commonResources["PlayerForegroundColor"],

                LappedDriverBackgroundBrush = (SolidColorBrush) _commonResources["TimingLappedBrush"],
                LappedDriverForegroundBrush = (SolidColorBrush) _commonResources["TimingLappedForegroundBrush"],

                LappingDriverBackgroundBrush = (SolidColorBrush) _commonResources["TimingLappingBrush"],
                LappingDriverForegroundBrush = (SolidColorBrush) _commonResources["TimingLappingForegroundBrush"],

                GreenSectorBrush = (SolidColorBrush)_commonResources["Green01Brush"],
                PurpleSectorBrush = (SolidColorBrush)_commonResources["PurpleTimingBrush"],
                YellowSectorBrush = (SolidColorBrush) _commonResources["YellowSectorBrush"],

                PlayerOutLineBrush = (SolidColorBrush) _commonResources["PlayerOutLineColor"],

                AnimateDriversPos = DisplaySettingsViewModel.AnimateDriversPosition,
                DataContext = this,
                AutoScaleDriverControls = DisplaySettingsViewModel.MapDisplaySettingsViewModel.AutoScaleDrivers,
                KeepMapRatio = DisplaySettingsViewModel.MapDisplaySettingsViewModel.KeepMapRatio,
            };
        }

        private void OnNewMapAvailable(object sender, MapEventArgs e)
        {
            if (_currentTrackTuple.simName == e.TrackMapDto.SimulatorSource && _currentTrackTuple.trackName == e.TrackMapDto.TrackName && _currentTrackTuple.layoutName == e.TrackMapDto.LayoutName)
            {
                Logger.Info($"New Map is Available: {_currentTrackTuple.simName}, {_currentTrackTuple.trackName}, {_currentTrackTuple.layoutName}");
                SituationOverviewControl = InitializeFullMap(e.TrackMapDto);
            }
        }

        private void OnMapRemoved(object sender, MapEventArgs e)
        {
            if (_currentTrackTuple.simName == e.TrackMapDto.SimulatorSource && _currentTrackTuple.trackName == e.TrackMapDto.TrackName && _currentTrackTuple.layoutName == e.TrackMapDto.LayoutName)
            {
                Logger.Info($" Map Removed: {_currentTrackTuple.simName}, {_currentTrackTuple.trackName}, {_currentTrackTuple.layoutName}");
                LoadCurrentMap();
            }
        }

        private void RemoveCurrentMap()
        {
            _mapManagementController.RemoveMap(_currentTrackTuple.simName, _currentTrackTuple.trackName, _currentTrackTuple.layoutName);
        }

        private void RotateCurrentMapLeft()
        {
            TrackMapDto trackMapDto = _mapManagementController.RotateMapLeft(_currentTrackTuple.simName, _currentTrackTuple.trackName, _currentTrackTuple.layoutName);
            InitializeFullMap(trackMapDto);
        }

        private void RotateCurrentMapRight()
        {
            TrackMapDto trackMapDto = _mapManagementController.RotateMapRight(_currentTrackTuple.simName, _currentTrackTuple.trackName, _currentTrackTuple.layoutName);
            InitializeFullMap(trackMapDto);
        }

    }
}