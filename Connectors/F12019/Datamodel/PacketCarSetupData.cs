namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PacketCarSetupData
    {
        public PacketHeader MHeader;            // Header

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20)]
        public CarSetupData[] MCarSetups;
    }
}