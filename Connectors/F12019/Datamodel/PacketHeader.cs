namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PacketHeader
    {
        public ushort MPacketFormat;         // 2019
        public byte MGameMajorVersion;     // Game major version - "X.00"
        public byte MGameMinorVersion;     // Game minor version - "1.XX"
        public byte MPacketVersion;        // Version of this packet type, all start from 1
        public byte MPacketId;             // Identifier for the packet type, see below
        public ulong MSessionUid;           // Unique identifier for the session
        public float MSessionTime;          // Session timestamp
        public uint MFrameIdentifier;      // Identifier for the frame the data was retrieved on
        public byte MPlayerCarIndex;       // Index of player's car in the array
    };
}