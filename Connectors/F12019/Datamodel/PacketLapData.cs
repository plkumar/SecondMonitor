namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PacketLapData
    {
        public PacketHeader MHeader;              // Header

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public LapData[] MLapData;         // Lap data for all cars on track
    }
}