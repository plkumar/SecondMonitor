namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct CarTelemetryData
    {
        public ushort MSpeed; // Speed of car in kilometres per hour

        public float MThrottle; // Amount of throttle applied (0.0 to 1.0)

        public float MSteer; // Steering (-1.0 (full lock left) to 1.0 (full lock right))

        public float MBrake; // Amount of brake applied (0.0 to 1.0)

        public byte MClutch; // Amount of clutch applied (0 to 100)

        public sbyte MGear; // Gear selected (1-8, N=0, R=-1)

        public ushort MEngineRpm; // Engine RPM

        public byte MDrs; // 0 = off, 1 = on

        public byte MRevLightsPercent; // Rev lights indicator (percentage)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public ushort[] MBrakesTemperature; // Brakes temperature (celsius)

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public ushort[] MTyresSurfaceTemperature; // Tyres surface temperature (celsius)

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public ushort[] MTyresInnerTemperature; // Tyres inner temperature (celsius)

        public ushort MEngineTemperature; // Engine temperature (celsius)

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] MTyresPressure; // Tyres pressure (PSI)

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] MSurfaceType; // Driving surface, see appendices
    }
}