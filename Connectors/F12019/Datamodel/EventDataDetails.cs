namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct EventDataDetails
    {
        [FieldOffset(0)]
        public Retirement retirement;
        [FieldOffset(0)]
        public TeamMateInPits teamMateInPits;
        [FieldOffset(0)]
        public RaceWinner raceWinner;
        [FieldOffset(0)]
        public FastestLap fastestLap;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct FastestLap
    {
        public byte vehicleIdx;
        public float lapTime;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct RaceWinner
    {
        public byte vehicleIdx;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TeamMateInPits
    {
        public byte vehicleIdx;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct Retirement
    {
        public byte vehicleIdx;
    }
}