namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct CarSetupData
    {
        public byte MFrontWing;                // Front wing aero

        public byte MRearWing;                 // Rear wing aero

        public byte MOnThrottle;               // Differential adjustment on throttle (percentage)

        public byte MOffThrottle;              // Differential adjustment off throttle (percentage)

        public float MFrontCamber;              // Front camber angle (suspension geometry)

        public float MRearCamber;               // Rear camber angle (suspension geometry)

        public float MFrontToe;                 // Front toe angle (suspension geometry)

        public float MRearToe;                  // Rear toe angle (suspension geometry)

        public byte MFrontSuspension;          // Front suspension

        public byte MRearSuspension;           // Rear suspension

        public byte MFrontAntiRollBar;         // Front anti-roll bar

        public byte MRearAntiRollBar;          // Front anti-roll bar

        public byte MFrontSuspensionHeight;    // Front ride height

        public byte MRearSuspensionHeight;     // Rear ride height

        public byte MBrakePressure;            // Brake pressure (percentage)

        public byte MBrakeBias;                // Brake bias (percentage)

        public float MFrontTyrePressure;        // Front tyre pressure (PSI)

        public float MRearTyrePressure;         // Rear tyre pressure (PSI)

        public byte MBallast;                  // Ballast

        public float MFuelLoad;                 // Fuel load
    }
}