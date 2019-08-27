namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PacketSessionData
    {
        public PacketHeader MHeader; // Header

        public byte MWeather; // Weather - 0 = clear, 1 = light cloud, 2 = overcast

        // 3 = light rain, 4 = heavy rain, 5 = storm
        public sbyte MTrackTemperature; // Track temp. in degrees celsius

        public sbyte MAirTemperature; // Air temp. in degrees celsius

        public byte MTotalLaps; // Total number of laps in this race

        public ushort MTrackLength; // Track length in metres

        public byte MSessionType; // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P

        // 5 = Q1, 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ
        // 10 = R, 11 = R2, 12 = Time Trial
        public sbyte MTrackId; // -1 for unknown, 0-21 for tracks, see appendix

        public byte MFormula; // Formula, 0 = F1 Modern, 1 = F1 Classic, 2 = F2,

        // 3 = F1 Generic

        public ushort MSessionTimeLeft; // Time left in session in seconds

        public ushort MSessionDuration; // Session duration in seconds

        public byte MPitSpeedLimit; // Pit speed limit in kilometres per hour

        public byte MGamePaused; // Whether the game is paused

        public byte MIsSpectating; // Whether the player is spectating

        public byte MSpectatorCarIndex; // Index of the car being spectated

        public byte MSliProNativeSupport; // SLI Pro support, 0 = inactive, 1 = active

        public byte MNumMarshalZones; // Number of marshal zones to follow

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public MarshalZone[] MMarshalZones; // List of marshal zones – max 21

        public byte MSafetyCarStatus; // 0 = no safety car, 1 = full safety car

        // 2 = virtual safety car
        public byte MNetworkGame; // 0 = offline, 1 = online
    }
}