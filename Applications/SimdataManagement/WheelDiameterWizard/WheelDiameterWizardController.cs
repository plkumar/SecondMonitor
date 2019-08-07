namespace SecondMonitor.SimdataManagement.WheelDiameterWizard
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Contracts.Commands;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using ViewModel;
    using ViewModels;
    using ViewModels.Factory;
    using ViewModels.Settings;
    using ViewModels.WheelDiameterWizard;

    public class WheelDiameterWizardController : IWheelDiameterWizardController
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IWindowService _windowService;
        private readonly ISettingsProvider _settingsProvider;
        public event EventHandler<EventArgs> WizardCompleted;
        private readonly WelcomeStageViewModel _welcomeStageViewModel;
        private readonly AccelerationStageViewModel _accelerationStageViewModel;
        private readonly PreparationStageViewModel _preparationStageViewModel;
        private readonly MeasurementPhaseViewModel _measurementPhaseViewModel;
        private readonly ResultsStageViewModel _resultsStageViewModel;
        private readonly DistanceUnits _distanceUnits;
        private readonly VelocityUnits _velocityUnits;
        private Window _wizardWindow;
        private readonly List<DriverInfo> _capturedData;
        private readonly Stopwatch _measurementStopWatch;
        private CarModelPropertiesViewModel _carModelPropertiesViewModel;

        /*Stages:
         0 - Welcome to wizard screen
         1 - Accelerate to 70Kph
         2 - Depress Clutch or Higher Gear
         3 - Wait 3 seconds
         4 - Done
         */
        private int _currentStage = 0;
        private double _frontWheelDiameter;
        private double _rearWheelDiameter;


        public WheelDiameterWizardController(IViewModelFactory viewModelFactory, IWindowService windowService, ISettingsProvider settingsProvider)
        {
            _velocityUnits = settingsProvider.DisplaySettingsViewModel.VelocityUnits;
            _distanceUnits = settingsProvider.DisplaySettingsViewModel.DistanceUnitsVerySmall;
            _viewModelFactory = viewModelFactory;
            _windowService = windowService;
            _settingsProvider = settingsProvider;
            _welcomeStageViewModel = viewModelFactory.Create<WelcomeStageViewModel>();
            _accelerationStageViewModel = viewModelFactory.Create<AccelerationStageViewModel>();
            _preparationStageViewModel = viewModelFactory.Create<PreparationStageViewModel>();
            _measurementPhaseViewModel = viewModelFactory.Create<MeasurementPhaseViewModel>();
            _resultsStageViewModel = viewModelFactory.Create<ResultsStageViewModel>();

            _accelerationStageViewModel.VelocityUnits = Velocity.GetUnitSymbol(_velocityUnits);
            _accelerationStageViewModel.TargetSpeed = _velocityUnits == VelocityUnits.Mph ? 75 : Math.Round(Velocity.FromKph(120).GetValueInUnits(_velocityUnits), 0);

            _resultsStageViewModel.DistanceUnits = Distance.GetUnitsSymbol(_distanceUnits);
            _resultsStageViewModel.OkCommand = new RelayCommand(TakeResultsFromWizard);
            _resultsStageViewModel.CancelCommand = new RelayCommand(CancelWizard);
            _resultsStageViewModel.RestartCommand = new RelayCommand(RestartWizard);

            _measurementStopWatch = new Stopwatch();
            _capturedData = new List<DriverInfo>();
        }

        public Task StartControllerAsync()
        {
            return Task.CompletedTask;
        }

        public Task StopControllerAsync()
        {
            return Task.CompletedTask;
        }

        public void OpenWizard(CarModelPropertiesViewModel propertiesToDetermine)
        {
            _carModelPropertiesViewModel = propertiesToDetermine;
            _currentStage = 0;
            _wizardWindow = _windowService.OpenWindow(_welcomeStageViewModel, "Tyre Diameter Wizard", WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner, OnWizardWindowClosed);
        }

        private void OnWizardWindowClosed()
        {
            WizardCompleted?.Invoke(this, new EventArgs());
        }

        public void ProcessDataSet(SimulatorDataSet dataSet)
        {
            if (_wizardWindow == null)
            {
                return;
            }

            switch (_currentStage)
            {
                case 0:
                    ProcessWelcomePhase(dataSet);
                    break;
                case 1:
                    ProcessAccelerationPhase(dataSet);
                    break;
                case 2:
                    ProcessPreparationPhase(dataSet);
                    break;
                case 3:
                    ProcessMeasurementPhase(dataSet);
                    break;
            }
        }

        private void ProcessMeasurementPhase(SimulatorDataSet dataSet)
        {
            if (_measurementStopWatch.ElapsedMilliseconds > 1000)
            {
                _capturedData.Add(dataSet.PlayerInfo);
            }

            _measurementPhaseViewModel.TimeLeftFormatted = (5.0 - _measurementStopWatch.ElapsedMilliseconds / 1000.0).ToString("F3");

            bool isBrakeReleased = dataSet.InputInfo.BrakePedalPosition < 0.01;
            bool isThrottleReleased = dataSet.InputInfo.ThrottlePedalPosition < 0.01;
            bool isClutchDepressed = dataSet.InputInfo.ClutchPedalPosition > 0.95;
            bool isInHighGear = int.TryParse(dataSet.PlayerInfo.CarInfo.CurrentGear, out int gear) && gear >= 4;

            if (!isBrakeReleased || !isThrottleReleased || (!isClutchDepressed && !isInHighGear))
            {
                _measurementStopWatch.Stop();
                ChangeState(2, _preparationStageViewModel);
                return;
            }

            if (_measurementStopWatch.ElapsedMilliseconds > 5000)
            {
                _measurementStopWatch.Stop();
                ComputeResultsAndSwitch();
            }
        }

        private void ProcessWelcomePhase(SimulatorDataSet dataSet)
        {
            if (dataSet.PlayerInfo.Speed.InKph > 5)
            {
                ChangeState(1, _accelerationStageViewModel);
            }
        }

        private void ProcessAccelerationPhase(SimulatorDataSet dataSet)
        {
            _accelerationStageViewModel.CurrentSpeed = Math.Round(dataSet.PlayerInfo.Speed.GetValueInUnits(_velocityUnits),0);
            if (_accelerationStageViewModel.CurrentSpeed > _accelerationStageViewModel.TargetSpeed)
            {
                ChangeState(2, _preparationStageViewModel);
            }
        }

        private void SwitchToMeasurementPhase()
        {
            _capturedData.Clear();
            _measurementStopWatch.Restart();
            ChangeState(3, _measurementPhaseViewModel);
        }

        private void ComputeResultsAndSwitch()
        {
            ChangeState(5, _resultsStageViewModel);
            _frontWheelDiameter = _capturedData.Select(x => x.Speed.InMs / x.CarInfo.WheelsInfo.FrontLeft.Rps).Average();
            _rearWheelDiameter = _capturedData.Select(x => x.Speed.InMs / x.CarInfo.WheelsInfo.RearLeft.Rps).Average();

            _resultsStageViewModel.FrontWheelDiameter = Distance.FromMeters(_frontWheelDiameter).GetByUnit(_distanceUnits);
            _resultsStageViewModel.RearWheelDiameter = Distance.FromMeters(_rearWheelDiameter).GetByUnit(_distanceUnits);
        }

        private void ProcessPreparationPhase(SimulatorDataSet dataSet)
        {
            _preparationStageViewModel.IsBrakeReleased = dataSet.InputInfo.BrakePedalPosition < 0.01;
            _preparationStageViewModel.IsThrottleReleased = dataSet.InputInfo.ThrottlePedalPosition < 0.01;
            _preparationStageViewModel.IsClutchDepressed = dataSet.InputInfo.ClutchPedalPosition > 0.95;
            _preparationStageViewModel.IsInHighGear = int.TryParse(dataSet.PlayerInfo.CarInfo.CurrentGear, out int gear) && gear >= 4;

            if(dataSet.PlayerInfo.Speed.InKph < 75)
            {
                ChangeState(1, _accelerationStageViewModel);
            }

            if (_preparationStageViewModel.IsBrakeReleased && _preparationStageViewModel.IsThrottleReleased && (_preparationStageViewModel.IsClutchDepressed || _preparationStageViewModel.IsInHighGear))
            {
                SwitchToMeasurementPhase();
            }
        }

        private void ChangeState(int newState, IViewModel newViewModel)
        {
            if (Application.Current.Dispatcher != null && !Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => ChangeState(newState, newViewModel));
                return;
            }

            _wizardWindow.Content = newViewModel;
            _currentStage = newState;
        }

        private void RestartWizard()
        {
            ChangeState(0, _welcomeStageViewModel);
        }

        private void CancelWizard()
        {
            _wizardWindow.Close();
        }

        private void TakeResultsFromWizard()
        {
            _carModelPropertiesViewModel.FrontWheelDiameter = Distance.FromMeters(_frontWheelDiameter);
            _carModelPropertiesViewModel.RearWheelDiameter = Distance.FromMeters(_rearWheelDiameter);
            _wizardWindow.Close();
        }
    }
}