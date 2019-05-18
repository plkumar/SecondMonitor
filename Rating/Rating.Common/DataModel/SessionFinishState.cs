namespace SecondMonitor.Rating.Common.DataModel
{
    using System.Collections.Generic;
    using System.Linq;

    public class SessionFinishState
    {
        public SessionFinishState(string trackName, IEnumerable<DriverFinishState> driversFinishStates)
        {
            TrackName = trackName;
            DriverFinishStates = driversFinishStates.ToArray();
        }

        public string TrackName { get; }

        public DriverFinishState[] DriverFinishStates { get; }
    }
}