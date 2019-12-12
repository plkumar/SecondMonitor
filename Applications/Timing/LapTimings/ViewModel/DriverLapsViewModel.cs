namespace SecondMonitor.Timing.LapTimings.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using DataModel.BasicProperties;
    using SecondMonitor.Timing.SessionTiming.Drivers.ViewModel;
    using SessionTiming;
    using SessionTiming.Drivers.Presentation.ViewModel;
    using SimdataManagement.DriverPresentation;
    using ViewModels;
    using DriverLapsWindow = View.DriverLapsWindow;

    public class DriverLapsViewModel : AbstractViewModel
    {
        private readonly DriverTimingViewModel _driverTiming;

        private readonly DriverLapsWindow _gui;
        private ObservableCollection<LapViewModel> _laps;
        private readonly DriverPresentationsManager _driverPresentationsManager;
        private ColorDto _outLineColor;
        private bool _hasCustomOutline;
        private bool _isPlayer;
        private string _driverName;

        public DriverLapsViewModel()
        {
            DriverName = "Design Time";
            Laps = new ObservableCollection<LapViewModel>();
        }

        public DriverLapsViewModel(DriverTimingViewModel driverTiming, DriverLapsWindow gui, DriverPresentationsManager driverPresentationsManager)
        {
            _driverTiming = driverTiming;
            Laps = new ObservableCollection<LapViewModel>();
            BuildLapsViewModel();
            _driverTiming.DriverTiming.NewLapStarted += DriverTimingOnNewLapStarted;
            DriverName = _driverTiming.Name;
            IsPlayer = _driverTiming.IsPlayer;
            _gui = gui;
            _driverPresentationsManager = driverPresentationsManager;
            _gui.Closed += GuiOnClosed;
            _gui.MouseLeave += GuiOnMouseLeave;
            _gui.DataContext = this;

            HasCustomOutline = _driverPresentationsManager.IsCustomOutlineEnabled(DriverName);
            OutLineColor = _driverPresentationsManager.TryGetOutLineColor(DriverName, out ColorDto color) ? color : null;
        }

        public DriverTiming DriverTiming => _driverTiming.DriverTiming;

        public ObservableCollection<LapViewModel> Laps
        {
            get => _laps;
            private set => SetProperty(ref _laps, value);
        }

        public ColorDto OutLineColor
        {
            get => _outLineColor;
            set
            {
                _outLineColor = value;
                _driverPresentationsManager.SetOutLineColor(DriverTiming.Name, value);
                NotifyPropertyChanged();
            }
        }

        public bool IsPlayer
        {
            get => _isPlayer;
            set => SetProperty(ref _isPlayer, value);
        }

        public bool HasCustomOutline
        {
            get => _hasCustomOutline;
            set
            {
                _hasCustomOutline = value;
                _driverPresentationsManager.SetOutLineColorEnabled(DriverTiming.Name, value);
                NotifyPropertyChanged();
            }
        }

        public string DriverName
        {
            get => _driverName;
            private set => SetProperty(ref _driverName, value);
        }

        public void UnRegisterOnGui()
        {
            _driverTiming.DriverTiming.NewLapStarted -= DriverTimingOnNewLapStarted;
            _gui.Closed -= GuiOnClosed;
            _gui.MouseLeave -= GuiOnMouseLeave;
            foreach (var lap in Laps)
            {
                lap.StopRefresh();
            }
        }

        private void GuiOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            _gui.LapsGrid.SelectedItem = null;
        }

        private void GuiOnClosed(object sender, EventArgs eventArgs)
        {
            UnRegisterOnGui();
        }

        private void DriverTimingOnNewLapStarted(object sender, LapEventArgs e)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => DriverTimingOnNewLapStarted(sender, e));
                return;
            }
            var newLapModel = new LapViewModel(e.Lap);
            if (Laps.Any() && Laps.Last().LapNumber == newLapModel.LapNumber)
            {
                Laps.Remove(Laps.Last());
            }

            Laps.Add(newLapModel);
            _gui.LapsGrid.ScrollIntoView(newLapModel);
        }

        private void BuildLapsViewModel()
        {
            if (_driverTiming == null)
            {
                return;
            }
            foreach (var lap in _driverTiming.DriverTiming.Laps)
            {
                if (Laps.Any() && Laps.Last().LapNumber == lap.LapNumber)
                {
                    Laps.Remove(Laps.Last());
                }

                Laps.Add(new LapViewModel(lap));
            }
        }
    }
}