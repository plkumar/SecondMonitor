namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PacketParticipantsData
    {
        public PacketHeader MHeader;            // Header

        public byte MNumActiveCars;  // Number of active cars in the data – should match number of cars on HUD

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20)]
        public ParticipantData[] MParticipants;
    }
}