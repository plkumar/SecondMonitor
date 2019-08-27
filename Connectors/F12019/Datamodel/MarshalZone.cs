namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct MarshalZone
    {
        public float MZoneStart;   // Fraction (0..1) of way through the lap the marshal zone starts
        public sbyte MZoneFlag;    // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red
    }
}