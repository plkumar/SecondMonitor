namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct LapData
    {
        public float MLastLapTime;            // Last lap time in seconds

        public float MCurrentLapTime; // Current time around the lap in seconds

        public float MBestLapTime;        // Best lap time of the session in seconds

        public float MSector1Time;        // Sector 1 time in seconds

        public float MSector2Time;        // Sector 2 time in seconds

        public float MLapDistance;        // Distance vehicle is around current lap in metres – could

        // be negative if line hasn’t been crossed yet

        public float MTotalDistance;      // Total distance travelled in session in metres – could

        // be negative if line hasn’t been crossed yet
        public float MSafetyCarDelta;        // Delta in seconds for safety car

        public byte MCarPosition;    // Car race position

        public byte MCurrentLapNum;      // Current lap number

        public byte MPitStatus;              // 0 = none, 1 = pitting, 2 = in pit area

        public byte MSector;                 // 0 = sector1, 1 = sector2, 2 = sector3

        public byte MCurrentLapInvalid;      // Current lap invalid - 0 = valid, 1 = invalid

        public byte MPenalties;              // Accumulated time penalties in seconds to be added

        public byte MGridPosition;           // Grid position the vehicle started the race in

        public byte MDriverStatus;           // Status of driver - 0 = in garage, 1 = flying lap // 2 = in lap, 3 = out lap, 4 = on track

        public byte MResultStatus;          // Result status - 0 = invalid, 1 = inactive, 2 = active 3 = finished, 4 = disqualified, 5 = not classified  6 = retired
    }
}