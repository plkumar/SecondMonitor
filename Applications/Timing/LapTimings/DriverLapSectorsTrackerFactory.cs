namespace SecondMonitor.Timing.LapTimings
{
    using SessionTiming.Drivers.ViewModel;
    using ViewModels.SessionEvents;

    public class DriverLapSectorsTrackerFactory
    {
        private readonly ISessionEventProvider _sessionEventProvider;

        public DriverLapSectorsTrackerFactory(ISessionEventProvider sessionEventProvider)
        {
            _sessionEventProvider = sessionEventProvider;
        }

        public IDriverLapSectorsTracker Build(DriverTiming driverTiming)
        {
            return new DriverLapSectorsTracker(driverTiming, _sessionEventProvider);
        }
    }
}