namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PacketEventData
    {
        public PacketHeader MHeader; // Header

        /**
         *“SSTA”  Sent when the session starts
          “SEND” Sent when the session ends
          “FTLP” When a driver achieves the fastest lap
          “RTMT” When a driver retires
          “DRSE” Race control have enabled DRS
          “DRSD” Race control have disabled DRS
          “TMPT” Your team mate has entered the pits
          “CHQF” The chequered flag has been waved
          “RCWN” The race winner is announced
         */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] MEventStringCode; // Event string code, see below

        public EventDataDetails MEventDetails; // Event details - should be interpreted differently for each type
    }
}