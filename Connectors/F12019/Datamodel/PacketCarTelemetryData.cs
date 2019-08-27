namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PacketCarTelemetryData
    {
        public  PacketHeader MHeader;        // Header

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20)]
        public  CarTelemetryData[] MCarTelemetryData;

        public uint MButtonStatus;        // Bit flags specifying which buttons are being pressed currently - see appendices
    }
}