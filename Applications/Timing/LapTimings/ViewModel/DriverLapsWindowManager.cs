namespace SecondMonitor.Timing.LapTimings.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using DataModel.Extensions;
    using View;
    using SecondMonitor.Timing.Presentation.ViewModel.Commands;
    using SessionTiming.Drivers.Presentation.ViewModel;
    using SimdataManagement.DriverPresentation;

    public class DriverLapsWindowManager
    {
        private readonly DriverPresentationsManager _driverPresentationsManager;
        private readonly List<DriverLapsWindow> _openedWindows = new List<DriverLapsWindow>();

        public DriverLapsWindowManager(Func<Window> getWindowOwnerFunc, Func<DriverTimingViewModel> getDriverTiming, DriverPresentationsManager driverPresentationsManager)
        {
            _driverPresentationsManager = driverPresentationsManager;
            GetDriverTiming = getDriverTiming;
            GetWindowOwnerFunc = getWindowOwnerFunc;
        }

        public Func<Window> GetWindowOwnerFunc
        {
            get;
            set;
        }

        public Func<DriverTimingViewModel> GetDriverTiming
        {
            get;
            set;
        }

        public ICommand OpenWindowCommand => new NoArgumentCommand(OpenWindowDefault);

        public void OpenWindowDefault()
        {
            OpenWindow(GetDriverTiming(), GetWindowOwnerFunc());
        }

        private void OpenWindow(DriverTimingViewModel driverTiming, Window ownerWindow )
        {
            if (driverTiming == null)
            {
                return;
            }
            DriverLapsWindow lapsWindow = new DriverLapsWindow()
                                              {
                                                  Owner = ownerWindow,
                                                  WindowStartupLocation = WindowStartupLocation.CenterOwner,
                                              };
            new DriverLapsViewModel(driverTiming, lapsWindow, _driverPresentationsManager);
            _openedWindows.Add(lapsWindow);
            lapsWindow.Closed += LapsWindow_Closed;
            lapsWindow.Show();
        }

        private void LapsWindow_Closed(object sender, EventArgs e)
        {
            if (sender is DriverLapsWindow window)
            {
                _openedWindows.Remove(window);
            }
        }

        public void Rebind(DriverTimingViewModel driverTiming)
        {
            _openedWindows.FindAll(p => ((DriverLapsViewModel)p.DataContext).DriverTiming.Name == driverTiming.Name).ForEach(p => Rebind(p, driverTiming));
        }

        public void RebindAll(IEnumerable<DriverTimingViewModel> driverTimings)
        {
            driverTimings.ForEach(Rebind);
        }

        private void Rebind(DriverLapsWindow window, DriverTimingViewModel newViewModel)
        {
            if (!(window.DataContext is DriverLapsViewModel oldDriverLapsViewModel))
            {
                return;
            }
            oldDriverLapsViewModel.UnRegisterOnGui();
            window.DataContext = new DriverLapsViewModel(newViewModel, window, _driverPresentationsManager);
        }
    }
}