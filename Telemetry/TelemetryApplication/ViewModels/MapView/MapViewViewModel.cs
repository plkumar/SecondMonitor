﻿namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.MapView
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using WindowsControls.WPF;
    using WindowsControls.WPF.DriverPosition;
    using Contracts.SelectionCounter;
    using Controllers.Synchronization;
    using DataModel.BasicProperties;
    using DataModel.Extensions;
    using DataModel.Snapshot.Drivers;
    using DataModel.Telemetry;
    using DataModel.TrackMap;
    using LoadedLapCache;
    using Ninject;
    using Ninject.Syntax;
    using NLog;
    using SecondMonitor.ViewModels;
    using SimdataManagement;
    using TelemetryManagement.DTO;

    public class MapViewViewModel : AbstractViewModel, IMapViewViewModel, IPositionCircleInformationProvider
    {

        private const int PedalStep = 25;
        private const double SelectionLength = 5;

        private readonly IResolutionRoot _resolutionRoot;
        private readonly ILoadedLapsCache _loadedLapsCache;
        private readonly IDataPointSelectionSynchronization _dataPointSelectionSynchronization;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ResourceDictionary _commonResources;
        private FullMapControl _situationOverviewControl;
        private ILapColorSynchronization _lapColorSynchronization;
        private readonly Dictionary<string, ILapCustomPathsCollection> _lapsPaths;
        private bool _showBrakeOverlay;
        private bool _showThrottleOverlay;
        private bool _showClutchOverlay;
        private bool _showShiftPoints;
        private bool _showColoredSectors;
        private readonly Dictionary<int, SelectionItemCounter<Path>> _selectionPaths;
        private ITrackMap _trackMapDto;

        public MapViewViewModel(IResolutionRoot resolutionRoot, ILoadedLapsCache loadedLapsCache, IDataPointSelectionSynchronization dataPointSelectionSynchronization, ILapColorSynchronization lapColorSynchronization)
        {
            _showColoredSectors = true;
            _resolutionRoot = resolutionRoot;
            _loadedLapsCache = loadedLapsCache;
            _dataPointSelectionSynchronization = dataPointSelectionSynchronization;
            _lapColorSynchronization = lapColorSynchronization;
            _lapsPaths = new Dictionary<string, ILapCustomPathsCollection>();
            _selectionPaths = new Dictionary<int, SelectionItemCounter<Path>>();
            _commonResources = new ResourceDictionary
            {
                Source = new Uri(
                    @"pack://application:,,,/WindowsControls;component/WPF/CommonResources.xaml",
                    UriKind.RelativeOrAbsolute)
            };
            Subscribe();
        }

        public FullMapControl SituationOverviewControl
        {
            get => _situationOverviewControl;
            set
            {
                _situationOverviewControl = value;
                NotifyPropertyChanged();
            }
        }

        public bool? ShowAllOverlays
        {
            get
            {
                if (ShowBrakeOverlay && ShowThrottleOverlay && ShowClutchOverlay && ShowShiftPoints)
                {
                    return true;
                }

                if (!ShowBrakeOverlay && !ShowThrottleOverlay && !ShowClutchOverlay && !ShowShiftPoints)
                {
                    return false;
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                _showBrakeOverlay = value.Value;
                _showThrottleOverlay = value.Value;
                _showShiftPoints = value.Value;
                _showClutchOverlay = value.Value;

                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ShowBrakeOverlay));
                NotifyPropertyChanged(nameof(ShowThrottleOverlay));
                NotifyPropertyChanged(nameof(ShowClutchOverlay));
                NotifyPropertyChanged(nameof(ShowShiftPoints));
                RefreshOverlays();
            }
        }

        public bool ShowColoredSectors
        {
            get => _showColoredSectors;
            set
            {
                SetProperty(ref _showColoredSectors, value);
                if (_situationOverviewControl != null)
                {
                    _situationOverviewControl.SectorsAlwaysVisible = value;
                }
            }
        }


        public bool ShowBrakeOverlay
        {
            get => _showBrakeOverlay;
            set
            {
                _showBrakeOverlay = value;
                RefreshOverlays();
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ShowAllOverlays));
            }
        }

        public bool ShowThrottleOverlay
        {
            get => _showThrottleOverlay;
            set
            {
                _showThrottleOverlay = value;
                RefreshOverlays();
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ShowAllOverlays));
            }
        }

        public bool ShowClutchOverlay
        {
            get => _showClutchOverlay;
            set
            {
                _showClutchOverlay = value;
                RefreshOverlays();
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ShowAllOverlays));
            }
        }

        public bool ShowShiftPoints
        {
            get => _showShiftPoints;
            set
            {
                _showShiftPoints = value;
                RefreshOverlays();
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ShowAllOverlays));
            }
        }


        public void RemoveDriver(IDriverInfo driverInfo)
        {
            _situationOverviewControl.RemoveDrivers(driverInfo);
        }

        public void UpdateDrivers(params IDriverInfo[] driversInfo)
        {
            _situationOverviewControl.UpdateDrivers(null, driversInfo);
        }

        public void LoadTrack(ITrackMap trackMapDto)
        {
            _trackMapDto = trackMapDto;
            SituationOverviewControl = InitializeFullMap(trackMapDto);
        }

        public async Task AddPathsForLap(LapTelemetryDto lapTelemetry)
        {

            ILapCustomPathsCollection geometryCollection = GetOrCreateLapPathCollection(lapTelemetry.LapSummary.Id);
            if (!geometryCollection.FullyInitialized)
            {
                await InitializeGeometryCollection(geometryCollection, lapTelemetry);
            }
            RefreshOverlays();
            geometryCollection.GetAllPaths().ForEach(SituationOverviewControl.AddCustomPath);
        }

        public void DeselectPoints(IEnumerable<TimedTelemetrySnapshot> points)
        {
            var toDeselect = points.Where(x => _selectionPaths.ContainsKey(GetSelectionKey(x))).Select(x => new KeyValuePair<int, SelectionItemCounter<Path>>(GetSelectionKey(x), _selectionPaths [GetSelectionKey(x)]));
            var toRemove = toDeselect.Where(x => x.Value.RemoveSelection()).ToList();
            toRemove.ForEach(x =>
            {
                _situationOverviewControl.RemoveCustomPath(x.Value.Item);
                _selectionPaths.Remove(x.Key);
            });


        }

        private static int GetSelectionKey(TimedTelemetrySnapshot timedTelemetrySnapshot)
        {
            return (int)(timedTelemetrySnapshot.PlayerData.LapDistance / SelectionLength);
        }

        public void SelectTelemetryPointsInArea(Point pt1, Point pt2)
        {
            _dataPointSelectionSynchronization.SelectPoints(GetTelemetryPointsInArea(pt1, pt2));
        }

        public void DeselectTelemetryPointsInArea(Point pt1, Point pt2)
        {
            _dataPointSelectionSynchronization.DeSelectPoints(GetTelemetryPointsInArea(pt1, pt2));
        }

        public void SelectPoints(IEnumerable<TimedTelemetrySnapshot> points)
        {
            foreach (TimedTelemetrySnapshot timedTelemetrySnapshot in points)
            {
                int key = GetSelectionKey(timedTelemetrySnapshot);
                if (_selectionPaths.TryGetValue(key, out SelectionItemCounter<Path> itemCounter))
                {
                    itemCounter.AddSelection();
                    continue;
                }
                var markerPoint = TrackMapFromTelemetryFactory.ExtractWorldPoint(timedTelemetrySnapshot, _trackMapDto.TrackGeometry.XCoef, _trackMapDto.TrackGeometry.YCoef, _trackMapDto.TrackGeometry.IsSwappedAxis);
                var newPath = new Path
                {
                    Fill = Brushes.GreenYellow,
                    Data = new EllipseGeometry(markerPoint, 5, 5)
                };
                itemCounter = new SelectionItemCounter<Path>(newPath);
                _selectionPaths.Add(key, itemCounter);
                _situationOverviewControl.AddCustomPath(newPath);
            }
        }

        private async Task InitializeGeometryCollection(ILapCustomPathsCollection geometryCollection, LapTelemetryDto lapTelemetry)
        {
            string baseGeometry = TrackMapFromTelemetryFactory.GetGeometry(lapTelemetry.GetReducedSet(TimeSpan.FromMilliseconds(300)), false, _trackMapDto.TrackGeometry.XCoef, _trackMapDto.TrackGeometry.YCoef, _trackMapDto.TrackGeometry.IsSwappedAxis);
            Path geometryPath = new Path { Data = Geometry.Parse(baseGeometry), StrokeThickness = 0.5, Stroke = new SolidColorBrush() };
            if (!_lapColorSynchronization.TryGetColorForLap(lapTelemetry.LapSummary.Id, out Color color))
            {
                color = Colors.DarkBlue;
            }
            geometryPath.Stroke = new SolidColorBrush(color);

            geometryCollection.BaseLapPath = geometryPath;

            string[] brakeGeometries = new string[0];
            string[] throttleGeometries = new string[0];
            string[] clutchGeometries = new string[0];
            string shiftPointsPath = string.Empty;

            Task brakeTask = Task.Run(() => brakeGeometries = GetGeometriesPathForBrakes(lapTelemetry));
            Task throttleTask = Task.Run(() => throttleGeometries = GetGeometriesPathPedalInput(lapTelemetry, x => x.ThrottlePedalPosition));
            Task clutchTask = Task.Run(() => clutchGeometries = GetGeometriesPathPedalInput(lapTelemetry, x => x.ClutchPedalPosition));
            Task shiftPointsTask = Task.Run(() => shiftPointsPath = GetShiftPointsGeometry(lapTelemetry));

            await Task.WhenAll(brakeTask, throttleTask, clutchTask, shiftPointsTask);
            //start from one, no reason to include "no brakes" geometry
            for (int i = 1; i < brakeGeometries.Length; i++)
            {
                string brakeGeometry = brakeGeometries[i];
                if (string.IsNullOrEmpty(brakeGeometry))
                {
                    continue;
                }
                Path brakePath = new Path { Data = Geometry.Parse(brakeGeometry), StrokeThickness = 5.0, Stroke = (i * PedalStep) > 99 ? Brushes.Red : Brushes.Orange, Opacity = (i * PedalStep) / 100.0 };
                geometryCollection.AddBrakingPath(brakePath, brakePath.Opacity);
            }

            for (int i = 1; i < throttleGeometries.Length; i++)
            {
                string throttleGeometry = throttleGeometries[i];
                if (string.IsNullOrEmpty(throttleGeometry))
                {
                    continue;
                }
                Path brakePath = new Path { Data = Geometry.Parse(throttleGeometry), StrokeThickness = 4.0, Stroke = (i * PedalStep) > 99 ? Brushes.LimeGreen : Brushes.Green, Opacity = (i * PedalStep) / 100.0 };
                geometryCollection.AddThrottlePath(brakePath, brakePath.Opacity);
            }

            for (int i = 1; i < clutchGeometries.Length; i++)
            {
                string clutchGeometry = clutchGeometries[i];
                if (string.IsNullOrEmpty(clutchGeometry))
                {
                    continue;
                }
                Path brakePath = new Path { Data = Geometry.Parse(clutchGeometry), StrokeThickness = 3.0, Stroke = Brushes.Yellow, Opacity = (i * PedalStep) / 100.0 };
                geometryCollection.AddClutchPath(brakePath, brakePath.Opacity);
            }

            Path shiftPointPath = new Path { Data = Geometry.Parse(shiftPointsPath), StrokeThickness = 5.0, Stroke = new SolidColorBrush(color)};
            geometryCollection.ShiftPointsPath = shiftPointPath;
            geometryCollection.FullyInitialized = true;
        }

        private string[] GetGeometriesPathForBrakes(LapTelemetryDto lapTelemetryDto)
        {
            return GetGeometriesPathPedalInput(lapTelemetryDto, x => x.BrakePedalPosition);
        }

        private string[] GetGeometriesPathPedalInput(LapTelemetryDto lapTelemetryDto, Func<InputInfo, double> pedalValueFunc)
        {
            string[] geometries = new string[100 / PedalStep + 1];
            for (int i = 0; i < geometries.Length; i++)
            {
                geometries[i] = string.Empty;
            }

            int previousBrakeBand = -1;
            List<TimedTelemetrySnapshot> currentSnapShot = new List<TimedTelemetrySnapshot>();
            foreach (TimedTelemetrySnapshot timedTelemetrySnapshot in lapTelemetryDto.GetReducedSet(TimeSpan.FromMilliseconds(200)))
            {
                int currentBrakeBand = ((int)(pedalValueFunc(timedTelemetrySnapshot.InputInfo) * 100)) / PedalStep;
                if (previousBrakeBand >= 0 && previousBrakeBand != currentBrakeBand)
                {
                    currentSnapShot.Add(timedTelemetrySnapshot);
                    geometries[previousBrakeBand] += $" {TrackMapFromTelemetryFactory.GetGeometry(currentSnapShot, false, _trackMapDto.TrackGeometry.XCoef, _trackMapDto.TrackGeometry.YCoef, _trackMapDto.TrackGeometry.IsSwappedAxis)}";
                    currentSnapShot.Clear();
                }
                currentSnapShot.Add(timedTelemetrySnapshot);
                previousBrakeBand = currentBrakeBand;
            }

            geometries[previousBrakeBand] += $" {TrackMapFromTelemetryFactory.GetGeometry(currentSnapShot, false, _trackMapDto.TrackGeometry.XCoef, _trackMapDto.TrackGeometry.YCoef, _trackMapDto.TrackGeometry.IsSwappedAxis)}";
            currentSnapShot.Clear();

            return geometries;
        }

        private string GetShiftPointsGeometry(LapTelemetryDto lapTelemetryDto)
        {
            StringBuilder sb = new StringBuilder();
            List<TimedTelemetrySnapshot> shiftPoints = new List<TimedTelemetrySnapshot>();
            string currentGear = string.Empty;
            foreach (TimedTelemetrySnapshot timedTelemetrySnapshot in lapTelemetryDto.DataPoints)
            {
                if (!string.IsNullOrEmpty(currentGear) && currentGear != "N" && currentGear != timedTelemetrySnapshot.PlayerData.CarInfo.CurrentGear)
                {
                    shiftPoints.Add(timedTelemetrySnapshot);
                }

                currentGear = timedTelemetrySnapshot.PlayerData.CarInfo.CurrentGear;
            }

            TrackMapFromTelemetryFactory.ExtractWorldPoints(shiftPoints, _trackMapDto.TrackGeometry.XCoef, _trackMapDto.TrackGeometry.YCoef, _trackMapDto.TrackGeometry.IsSwappedAxis).ForEach(x => sb.Append($"M{x.X-1},{x.Y-1} a2,2 0 1,0 0,-1 Z "));
            return sb.ToString();
        }

        private void RefreshOverlays()
        {
            Visibility clutchPointVisibility = _showClutchOverlay ? Visibility.Visible : Visibility.Collapsed;
            Visibility throttlePointVisibility = _showThrottleOverlay ? Visibility.Visible : Visibility.Collapsed;
            Visibility brakePointVisibility = _showBrakeOverlay ? Visibility.Visible : Visibility.Collapsed;
            Visibility shiftPointVisibility = _showShiftPoints ? Visibility.Visible : Visibility.Collapsed;
            foreach (ILapCustomPathsCollection lapCustomPathsCollection in _lapsPaths.Values)
            {
                if (!lapCustomPathsCollection.FullyInitialized)
                {
                    continue;
                }
                lapCustomPathsCollection.ShiftPointsPath.Visibility = shiftPointVisibility;
                lapCustomPathsCollection.GetAllBrakingPaths().ForEach(x => x.Visibility = brakePointVisibility);
                lapCustomPathsCollection.GetAllClutchPaths().ForEach(x => x.Visibility = clutchPointVisibility);
                lapCustomPathsCollection.GetAllThrottlePaths().ForEach(x => x.Visibility = throttlePointVisibility);
            }
        }

        public void RemovePathsForLap(LapSummaryDto lapSummaryDto)
        {
            ILapCustomPathsCollection geometryCollection = GetOrCreateLapPathCollection(lapSummaryDto.Id);
            geometryCollection.GetAllPaths().ForEach(SituationOverviewControl.RemoveCustomPath);
        }

        public void Dispose()
        {
            UnsubscribeLapColorSync();
        }

        protected ILapCustomPathsCollection GetOrCreateLapPathCollection(string id)
        {
            if (_lapsPaths.TryGetValue(id, out ILapCustomPathsCollection lapCustomPathsCollection))
            {
                return lapCustomPathsCollection;
            }

            ILapCustomPathsCollection newLapCustomPathsCollection = _resolutionRoot.Get<ILapCustomPathsCollection>();
            _lapsPaths[id] = newLapCustomPathsCollection;
            return newLapCustomPathsCollection;

        }

        private void UnsubscribeLapColorSync()
        {
            _lapColorSynchronization.LapColorChanged -= LapColorSynchronizationOnLapColorChanged;
            _dataPointSelectionSynchronization.OnPointsSelected -= DataPointSelectionSynchronizationOnOnPointsSelected;
            _dataPointSelectionSynchronization.OnPointsDeselected -= DataPointSelectionSynchronizationOnOnPointsDeselected;
        }

        private void Subscribe()
        {
            _lapColorSynchronization.LapColorChanged += LapColorSynchronizationOnLapColorChanged;
            _dataPointSelectionSynchronization.OnPointsSelected += DataPointSelectionSynchronizationOnOnPointsSelected;
            _dataPointSelectionSynchronization.OnPointsDeselected += DataPointSelectionSynchronizationOnOnPointsDeselected;
        }

        private void DataPointSelectionSynchronizationOnOnPointsDeselected(object sender, TimedTelemetryArgs e)
        {
            DeselectPoints(e.TelemetrySnapshots);
        }

        private void DataPointSelectionSynchronizationOnOnPointsSelected(object sender, TimedTelemetryArgs e)
        {
            SelectPoints(e.TelemetrySnapshots);
        }

        private void LapColorSynchronizationOnLapColorChanged(object sender, LapColorArgs e)
        {
            if (SituationOverviewControl==null)
            {
                return;
            }
            ILapCustomPathsCollection lapPaths = GetOrCreateLapPathCollection(e.LapId);
            if (lapPaths.BaseLapPath != null)
            {
                lapPaths.BaseLapPath.Stroke = new SolidColorBrush(e.Color);
            }

            if (lapPaths.ShiftPointsPath != null)
            {
                lapPaths.ShiftPointsPath.Stroke = new SolidColorBrush(e.Color);
            }
        }

        private IEnumerable<TimedTelemetrySnapshot> GetTelemetryPointsInArea(Point pt1, Point pt2)
        {
            Point realWorldPoint1 = TrackMapFromTelemetryFactory.InverseExtractWorldPoint(pt1, _trackMapDto.TrackGeometry.XCoef, _trackMapDto.TrackGeometry.YCoef, _trackMapDto.TrackGeometry.IsSwappedAxis);
            Point realWorldPoint2 = TrackMapFromTelemetryFactory.InverseExtractWorldPoint(pt2, _trackMapDto.TrackGeometry.XCoef, _trackMapDto.TrackGeometry.YCoef, _trackMapDto.TrackGeometry.IsSwappedAxis);

            double minX = Math.Min(realWorldPoint1.X, realWorldPoint2.X);
            double maxX = Math.Max(realWorldPoint1.X, realWorldPoint2.X);
            double minY = Math.Min(realWorldPoint1.Y, realWorldPoint2.Y);
            double maxY = Math.Max(realWorldPoint1.Y, realWorldPoint2.Y);

            var pointsToSelect = _loadedLapsCache.LoadedLaps.SelectMany(x => x.DataPoints).Where(x =>
            {
                var worldPos = x.PlayerData.WorldPosition;
                double xInMeters = worldPos.X.InMeters;
                double yInMeters = worldPos.Z.InMeters;
                return minX <= xInMeters && xInMeters <= maxX && minY <= yInMeters && yInMeters <= maxY;
            });
            return pointsToSelect;
        }

        private FullMapControl InitializeFullMap(ITrackMap trackMapDto)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                return Application.Current.Dispatcher.Invoke(() => InitializeFullMap(trackMapDto));
            }

            Logger.Info("Initializing Full Map Control");
           _lapsPaths.Clear();

            return new FullMapControl(trackMapDto)
            {
                PositionCircleInformationProvider = this,

                DriverBackgroundBrush = (SolidColorBrush)_commonResources["DriverBackgroundColor"],
                DriverForegroundBrush = (SolidColorBrush)_commonResources["DriverForegroundColor"],

                DriverPitsBackgroundBrush = (SolidColorBrush)_commonResources["DriverPitsBackgroundColor"],
                DriverPitsForegroundBrush = (SolidColorBrush)_commonResources["DriverPitsForegroundColor"],

                PlayerBackgroundBrush = (SolidColorBrush)_commonResources["PlayerBackgroundColor"],
                PlayerForegroundBrush = (SolidColorBrush)_commonResources["PlayerForegroundColor"],

                LappedDriverBackgroundBrush = (SolidColorBrush)_commonResources["TimingLappedBrush"],
                LappedDriverForegroundBrush = (SolidColorBrush)_commonResources["TimingLappedForegroundBrush"],

                LappingDriverBackgroundBrush = (SolidColorBrush)_commonResources["TimingLappingBrush"],
                LappingDriverForegroundBrush = (SolidColorBrush)_commonResources["TimingLappingForegroundBrush"],

                GreenSectorBrush = (SolidColorBrush)_commonResources["Green01Brush"],
                PurpleSectorBrush = (SolidColorBrush)_commonResources["PurpleTimingBrush"],
                YellowSectorBrush = (SolidColorBrush)_commonResources["YellowSectorBrush"],

                Sector1Brush = (SolidColorBrush)_commonResources["Sector1Brush"],
                Sector2Brush = (SolidColorBrush)_commonResources["Sector2Brush"],
                Sector3Brush = (SolidColorBrush)_commonResources["Sector3Brush"],

                PlayerOutLineBrush = (SolidColorBrush)_commonResources["PlayerOutLineColor"],

                AnimateDriversPos = false,
                DataContext = this,
                AutoScaleDriverControls = false,
                KeepMapRatio = true,
                EnableSidePanel = false,
                SectorsAlwaysVisible = ShowColoredSectors
            };
        }

        public bool IsDriverOnValidLap(IDriverInfo driver) => true;

        public bool IsDriverLastSectorGreen(IDriverInfo driver, int sectorNumber) => false;

        public bool IsDriverLastSectorPurple(IDriverInfo driver, int sectorNumber) => false;

        public bool TryGetCustomOutline(IDriverInfo driverInfo, out ColorDto outlineBrush)
        {
            if (_lapColorSynchronization == null)
            {
                outlineBrush = null;
                return false;
            }

            if (_lapColorSynchronization.TryGetColorForLap(driverInfo.DriverName, out Color lapColor))
            {
                outlineBrush = new ColorDto()
                {
                    Alpha = lapColor.A,
                    Blue = lapColor.B,
                    Green = lapColor.G,
                    Red =  lapColor.R,
                };
                return true;
            }

            outlineBrush = null;
            return false;
        }

        public ColorDto GetClassColor(IDriverInfo driverInfo)
        {
            return ColorDto.FromColor(Colors.Transparent);
        }
    }
}