namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;


    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct CarStatusData
    {
        public byte MTractionControl; // 0 (off) - 2 (high)

        public byte MAntiLockBrakes; // 0 (off) - 1 (on)

        public byte MFuelMix; // Fuel mix - 0 = lean, 1 = standard, 2 = rich, 3 = max

        public byte MFrontBrakeBias; // Front brake bias (percentage)

        public byte MPitLimiterStatus; // Pit limiter status - 0 = off, 1 = on

        public float MFuelInTank; // Current fuel mass

        public float MFuelCapacity; // Fuel capacity

        public float MFuelRemainingLaps; // Fuel remaining in terms of laps (value on MFD)

        public ushort MMaxRpm; // Cars max RPM, point of rev limiter

        public ushort MIdleRpm; // Cars idle RPM

        public byte MMaxGears; // Maximum number of gears

        public byte MDrsAllowed; // 0 = not allowed, 1 = allowed, -1 = unknown

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] MTyresWear; // Tyre wear percentage

        public byte MActualTyreCompound; // F1 Modern - 16 = C5, 17 = C4, 18 = C3, 19 = C2, 20 = C1

        // 7 = inter, 8 = wet
        // F1 Classic - 9 = dry, 10 = wet
        // F2 – 11 = super soft, 12 = soft, 13 = medium, 14 = hard
        // 15 = wet
        public byte MTyreVisualCompound; // F1 visual (can be different from actual compound)  16 = soft, 17 = medium, 18 = hard, 7 = inter, 8 = wet  F1 Classic – same as above// F2 – same as above

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] MTyresDamage; // Tyre damage (percentage)

        public byte MFrontLeftWingDamage; // Front left wing damage (percentage)

        public byte MFrontRightWingDamage; // Front right wing damage (percentage)

        public byte MRearWingDamage; // Rear wing damage (percentage)

        public byte MEngineDamage; // Engine damage (percentage)

        public byte MGearBoxDamage; // Gear box damage (percentage)

        public sbyte MVehicleFiaFlags; // -1 = invalid/unknown, 0 = none, 1 = green,  2 = blue, 3 = yellow, 4 = red

        public float MErsStoreEnergy; // ERS energy store in Joules

        public byte MErsDeployMode; // ERS deployment mode, 0 = none, 1 = low, 2 = medium, 3 = high, 4 = overtake, 5 = hotlap

        public float MErsHarvestedThisLapMguk; // ERS energy harvested this lap by MGU-K

        public float MErsHarvestedThisLapMguh; // ERS energy harvested this lap by MGU-H

        public float MErsDeployedThisLap; // ERS energy deployed this lap
    }

}