namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct ParticipantData
    {

        public byte MAiControlled;           // Whether the vehicle is AI (1) or Human (0) controlled


        public byte MDriverId;       // Driver id - see appendix

        public byte MTeamId;                 // Team id - see appendix

        public byte MRaceNumber;             // Race number of the car

        public byte MNationality;            // Nationality of the driver

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] MName;               // Name of participant in UTF-8 format – null terminated Will be truncated with … (U+2026) if too long

        public byte MYourTelemetry;          // The player's UDP setting, 0 = restricted, 1 = public
    }
}