﻿namespace SecondMonitor.ViewModels.Settings.ViewModel
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Contracts.Commands;
    using DataModel.BasicProperties;
    using DataModel.BasicProperties.FuelConsumption;
    using DataModel.BasicProperties.Units;
    using Model;

    public class DisplaySettingsViewModel : AbstractViewModel<DisplaySettings>
    {
        private VelocityUnits _velocityUnits;
        private TemperatureUnits _temperatureUnits;
        private PressureUnits _pressureUnits;
        private VolumeUnits _volumeUnits;
        private FuelCalculationScope _fuelCalculationScope;
        private int _paceLaps;
        private int _refreshRate;
        private bool _scrollToPlayer;
        private SessionOptionsViewModel _practiceSessionDisplayOptionsView;
        private SessionOptionsViewModel _qualificationSessionDisplayOptionsView;
        private SessionOptionsViewModel _raceSessionDisplayOptionsView;
        private ReportingSettingsViewModel _reportingSettingsView;
        private bool _animateDriverPosition;
        private bool _animateDeltaTimes;

        private MapDisplaySettingsViewModel _mapDisplaySettingsViewModel;
        private TelemetrySettingsViewModel _telemetrySettingsViewModel;
        private MultiClassDisplayKind _multiClassDisplayKind;
        private ForceUnits _forceUnits;
        private AngleUnits _angleUnits;
        private PowerUnits _powerUnits;
        private TorqueUnits _torqueUnits;
        private bool _isGapVisualizationEnabled;
        private double _minimalGapForVisualization;
        private double _gapHeightForOneSecond;
        private double _maximumGapHeight;
        private RatingSettingsViewModel _ratingSettingsViewModel;
        private PitBoardSettingsViewModel _pitBoardSettingsViewModel;
        private TrackRecordsSettingsViewModel _trackRecordsSettingsViewModel;
        private bool _enablePedalInformation;
        private bool _enableTemperatureInformation;
        private bool _enableNonTemperatureInformation;

        public ICommand OpenLogDirectoryCommand => new RelayCommand(OpenLogDirectory);

        public TelemetrySettingsViewModel TelemetrySettingsViewModel
        {
            get => _telemetrySettingsViewModel;
            set
            {
                _telemetrySettingsViewModel = value;
                NotifyPropertyChanged();
            }
        }

        public TemperatureUnits TemperatureUnits
        {
            get => _temperatureUnits;
            set
            {
                _temperatureUnits = value;
                NotifyPropertyChanged();
            }
        }

        public MultiClassDisplayKind MultiClassDisplayKind
        {
            get => _multiClassDisplayKind;
            set
            {
                _multiClassDisplayKind = value;
                NotifyPropertyChanged();
            }
        }

        public PressureUnits PressureUnits
        {
            get => _pressureUnits;
            set
            {
                _pressureUnits = value;
                NotifyPropertyChanged();
            }
        }

        public VolumeUnits VolumeUnits
        {
            get => _volumeUnits;
            set
            {
                _volumeUnits = value;
                NotifyPropertyChanged();
            }
        }

        public VelocityUnits VelocityUnits
        {
            get => _velocityUnits;
            set
            {
                _velocityUnits = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(DistanceUnits));
                NotifyPropertyChanged(nameof(FuelPerDistanceUnits));
            }
        }

        public ForceUnits ForceUnits
        {
            get => _forceUnits;
            set => SetProperty(ref _forceUnits, value);
        }

        public AngleUnits AngleUnits
        {
            get => _angleUnits;
            set => SetProperty(ref _angleUnits, value);
        }

        public TorqueUnits TorqueUnits
        {
            get => _torqueUnits;
            set => SetProperty(ref _torqueUnits, value);
        }

        public PowerUnits PowerUnits
        {
            get => _powerUnits;
            set => SetProperty(ref _powerUnits, value);
        }

        public FuelCalculationScope FuelCalculationScope
        {
            get => _fuelCalculationScope;
            set
            {
                _fuelCalculationScope = value;
                NotifyPropertyChanged();
            }
        }

        public bool EnablePedalInformation
        {
            get => _enablePedalInformation;
            set => SetProperty(ref _enablePedalInformation, value);
        }

        public bool EnableTemperatureInformation
        {
            get => _enableTemperatureInformation;
            set => SetProperty(ref _enableTemperatureInformation, value);
        }

        public bool EnableNonTemperatureInformation
        {
            get => _enableNonTemperatureInformation;
            set => SetProperty(ref _enableNonTemperatureInformation, value);
        }

        public DistanceUnits DistanceUnits
        {
            get
            {
                switch (VelocityUnits)
                {
                    case VelocityUnits.Kph:
                        return DistanceUnits.Kilometers;
                    case VelocityUnits.Mph:
                        return DistanceUnits.Miles;
                    case VelocityUnits.Ms:
                        return DistanceUnits.Meters;
                    default:
                        return DistanceUnits.Kilometers;
                }
            }
        }

        public DistanceUnits DistanceUnitsSmall
        {
            get
            {
                switch (VelocityUnits)
                {
                    case VelocityUnits.Kph:
                        return DistanceUnits.Meters;
                    case VelocityUnits.Mph:
                        return DistanceUnits.Yards;
                    case VelocityUnits.Ms:
                        return DistanceUnits.Meters;
                    default:
                        return DistanceUnits.Meters;
                }
            }
        }

        public DistanceUnits DistanceUnitsVerySmall
        {
            get
            {
                switch (VelocityUnits)
                {
                    case VelocityUnits.Kph:
                        return DistanceUnits.Millimeter;
                    case VelocityUnits.Mph:
                        return DistanceUnits.Inches;
                    case VelocityUnits.Ms:
                        return DistanceUnits.Millimeter;
                    default:
                        return DistanceUnits.Millimeter;
                }
            }
        }

        public VelocityUnits VelocityUnitsVerySmall
        {
            get
            {
                switch (VelocityUnits)
                {
                    case VelocityUnits.Kph:
                        return VelocityUnits.MMPerSecond;
                    case VelocityUnits.Mph:
                        return VelocityUnits.InPerSecond;
                    case VelocityUnits.Ms:
                        return VelocityUnits.MMPerSecond;
                    case VelocityUnits.Fps:
                        return VelocityUnits.InPerSecond;
                    case VelocityUnits.CmPerSecond:
                        return VelocityUnits.MMPerSecond;
                    case VelocityUnits.InPerSecond:
                        return VelocityUnits.InPerSecond;
                    default:
                        return VelocityUnits.MMPerSecond;
                }
            }
        }

        public FuelPerDistanceUnits FuelPerDistanceUnits
        {
            get
            {
                switch (VelocityUnits)
                {
                    case VelocityUnits.Kph:
                        return FuelPerDistanceUnits.LitersPerHundredKm;
                    case VelocityUnits.Mph:
                        return FuelPerDistanceUnits.MilesPerGallon;
                    case VelocityUnits.Ms:
                        return FuelPerDistanceUnits.LitersPerHundredKm;
                    default:
                        return FuelPerDistanceUnits.LitersPerHundredKm;
                }
            }
        }

        public int PaceLaps
        {
            get => _paceLaps;
            set
            {
                _paceLaps = value;
                NotifyPropertyChanged();
            }
        }

        public int RefreshRate
        {
            get => _refreshRate;
            set
            {
                _refreshRate = value;
                NotifyPropertyChanged();
            }
        }

        public bool ScrollToPlayer
        {
            get => _scrollToPlayer;
            set
            {
                _scrollToPlayer = value;
                NotifyPropertyChanged();
            }
        }

        public bool AnimateDriversPosition
        {
            get => _animateDriverPosition;
            set
            {
                _animateDriverPosition = value;
                NotifyPropertyChanged();
            }
        }

        public bool AnimateDeltaTimes
        {
            get => _animateDeltaTimes;
            set
            {
                _animateDeltaTimes = value;
                NotifyPropertyChanged();
            }
        }

        public SessionOptionsViewModel PracticeSessionDisplayOptionsView
        {
            get => _practiceSessionDisplayOptionsView;
            set
            {
                _practiceSessionDisplayOptionsView = value;
                NotifyPropertyChanged();
            }
        }

        public SessionOptionsViewModel QualificationSessionDisplayOptionsView
        {
            get => _qualificationSessionDisplayOptionsView;
            set
            {
                _qualificationSessionDisplayOptionsView = value;
                NotifyPropertyChanged();
            }
        }

        public SessionOptionsViewModel RaceSessionDisplayOptionsView
        {
            get => _raceSessionDisplayOptionsView;
            set
            {
                _raceSessionDisplayOptionsView = value;
                NotifyPropertyChanged();
            }
        }

        public ReportingSettingsViewModel ReportingSettingsView
        {
            get => _reportingSettingsView;
            set
            {
                _reportingSettingsView = value;
                NotifyPropertyChanged();
            }
        }

        public MapDisplaySettingsViewModel MapDisplaySettingsViewModel
        {
            get => _mapDisplaySettingsViewModel;
            set
            {
                _mapDisplaySettingsViewModel = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsGapVisualizationEnabled
        {
            get => _isGapVisualizationEnabled;
            set => SetProperty(ref _isGapVisualizationEnabled, value);
        }

        public double MinimalGapForVisualization
        {
            get => _minimalGapForVisualization;
            set => SetProperty(ref _minimalGapForVisualization, value);
        }

        public double GapHeightForOneSecond
        {
            get => _gapHeightForOneSecond;
            set => SetProperty(ref _gapHeightForOneSecond, value);
        }

        public double MaximumGapHeight
        {
            get => _maximumGapHeight;
            set => SetProperty(ref _maximumGapHeight, value);
        }

        public RatingSettingsViewModel RatingSettingsViewModel
        {
            get => _ratingSettingsViewModel;
            set => SetProperty(ref _ratingSettingsViewModel, value);
        }

        public PitBoardSettingsViewModel PitBoardSettingsViewModel
        {
            get => _pitBoardSettingsViewModel;
            set => SetProperty(ref _pitBoardSettingsViewModel, value);
        }

        public TrackRecordsSettingsViewModel TrackRecordsSettingsViewModel
        {
            get => _trackRecordsSettingsViewModel;
            set => SetProperty(ref _trackRecordsSettingsViewModel, value);
        }

        public WindowLocationSetting WindowLocationSetting { get; set; }

        protected override void ApplyModel(DisplaySettings settings)
        {

            TemperatureUnits = settings.TemperatureUnits;
            PressureUnits = settings.PressureUnits;
            VolumeUnits = settings.VolumeUnits;
            VelocityUnits = settings.VelocityUnits;
            FuelCalculationScope = settings.FuelCalculationScope;
            PaceLaps = settings.PaceLaps;
            RefreshRate = settings.RefreshRate;
            ScrollToPlayer = settings.ScrollToPlayer;
            AnimateDeltaTimes = settings.AnimateDeltaTimes;
            AnimateDriversPosition = settings.AnimateDriversPosition;
            MultiClassDisplayKind = settings.MultiClassDisplayKind;
            ForceUnits = settings.ForceUnits;
            AngleUnits = settings.AngleUnits;

            IsGapVisualizationEnabled = settings.IsGapVisualizationEnabled;
            MinimalGapForVisualization = settings.MinimalGapForVisualization;
            MaximumGapHeight = settings.MaximumGapHeight;
            GapHeightForOneSecond = settings.GapHeightForOneSecond;

            MapDisplaySettingsViewModel = new MapDisplaySettingsViewModel();
            MapDisplaySettingsViewModel.FromModel(settings.MapDisplaySettings);

            PracticeSessionDisplayOptionsView = SessionOptionsViewModel.CreateFromModel(settings.PracticeOptions);
            QualificationSessionDisplayOptionsView = SessionOptionsViewModel.CreateFromModel(settings.QualificationOptions);
            RaceSessionDisplayOptionsView = SessionOptionsViewModel.CreateFromModel(settings.RaceOptions);

            ReportingSettingsView = new ReportingSettingsViewModel();
            ReportingSettingsView.FromModel(settings.ReportingSettings);

            TelemetrySettingsViewModel = new TelemetrySettingsViewModel();
            TelemetrySettingsViewModel.FromModel(settings.TelemetrySettings);
            WindowLocationSetting = settings.WindowLocationSetting;

            RatingSettingsViewModel = new RatingSettingsViewModel();
            RatingSettingsViewModel.FromModel(settings.RatingSettings);

            PitBoardSettingsViewModel = new PitBoardSettingsViewModel();
            PitBoardSettingsViewModel.FromModel(settings.PitBoardSettings);

            TrackRecordsSettingsViewModel = new TrackRecordsSettingsViewModel();
            TrackRecordsSettingsViewModel.FromModel(settings.TrackRecordsSettings);

            PowerUnits = settings.PowerUnits;
            TorqueUnits = settings.TorqueUnits;

            EnablePedalInformation = settings.EnablePedalInformation;
            EnableNonTemperatureInformation = settings.EnableNonTemperatureInformation;
            EnableTemperatureInformation = settings.EnableTemperatureInformation;
        }

        public override DisplaySettings SaveToNewModel()
        {
            return new DisplaySettings()
            {
                TemperatureUnits = TemperatureUnits,
                PressureUnits = PressureUnits,
                VolumeUnits = VolumeUnits,
                VelocityUnits =  VelocityUnits,
                FuelCalculationScope = FuelCalculationScope,
                PaceLaps = PaceLaps,
                RefreshRate = RefreshRate,
                ScrollToPlayer = ScrollToPlayer,
                PracticeOptions = PracticeSessionDisplayOptionsView.ToModel(),
                QualificationOptions = QualificationSessionDisplayOptionsView.ToModel(),
                RaceOptions = RaceSessionDisplayOptionsView.ToModel(),
                ReportingSettings = ReportingSettingsView.ToModel(),
                AnimateDriversPosition =  AnimateDriversPosition,
                AnimateDeltaTimes =  AnimateDeltaTimes,
                MapDisplaySettings = MapDisplaySettingsViewModel.SaveToNewModel(),
                TelemetrySettings = TelemetrySettingsViewModel.SaveToNewModel(),
                MultiClassDisplayKind = MultiClassDisplayKind,
                ForceUnits = ForceUnits,
                AngleUnits = AngleUnits,
                IsGapVisualizationEnabled = IsGapVisualizationEnabled,
                MinimalGapForVisualization = MinimalGapForVisualization,
                MaximumGapHeight = MaximumGapHeight,
                GapHeightForOneSecond = GapHeightForOneSecond,
                WindowLocationSetting = WindowLocationSetting,
                RatingSettings = RatingSettingsViewModel.SaveToNewModel(),
                PitBoardSettings = PitBoardSettingsViewModel.SaveToNewModel(),
                TrackRecordsSettings = TrackRecordsSettingsViewModel.SaveToNewModel(),
                PowerUnits = PowerUnits,
                TorqueUnits = TorqueUnits,
                EnablePedalInformation = EnablePedalInformation,
                EnableTemperatureInformation = EnableTemperatureInformation,
                EnableNonTemperatureInformation = EnableNonTemperatureInformation,
            };
        }

        private void OpenLogDirectory()
        {
            string reportDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SecondMonitor");
            Task.Run(
                () =>
                {
                    Process.Start(reportDirectory);
                });
        }
    }
}
