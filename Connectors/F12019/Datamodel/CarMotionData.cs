namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CarMotionData
    {
        public float MWorldPositionX;           // World space X position
        public float MWorldPositionY;           // World space Y position
        public float MWorldPositionZ;           // World space Z position
        public float MWorldVelocityX;           // Velocity in world space X
        public float MWorldVelocityY;           // Velocity in world space Y
        public float MWorldVelocityZ;           // Velocity in world space Z
        public short MWorldForwardDirX;         // World space forward X direction (normalised)
        public short MWorldForwardDirY;         // World space forward Y direction (normalised)
        public short MWorldForwardDirZ;         // World space forward Z direction (normalised)
        public short MWorldRightDirX;           // World space right X direction (normalised)
        public short MWorldRightDirY;           // World space right Y direction (normalised)
        public short MWorldRightDirZ;           // World space right Z direction (normalised)
        public float MGForceLateral;            // Lateral G-Force component
        public float MGForceLongitudinal;       // Longitudinal G-Force component
        public float MGForceVertical;           // Vertical G-Force component
        public float MYaw;                      // Yaw angle in radians
        public float MPitch;                    // Pitch angle in radians
        public float MRoll;                     // Roll angle in radians
    }
}