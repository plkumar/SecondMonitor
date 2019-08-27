namespace SecondMonitor.F12019Connector.Datamodel
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PacketMotionData
    {
        public PacketHeader MHeader;                  // Header

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20)]
        public CarMotionData[] MCarMotionData;      // Data for all cars on track

        // Extra player car ONLY data
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] MSuspensionPosition;       // Note: All wheel arrays have the following order:

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] MSuspensionVelocity;       // RL, RR, FL, FR

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] MSuspensionAcceleration;  // RL, RR, FL, FR

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] MWheelSpeed;              // Speed of each wheel

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] MWheelSlip;                // Slip ratio for each wheel
        public float MLocalVelocityX;             // Velocity in local space
        public float MLocalVelocityY;             // Velocity in local space
        public float MLocalVelocityZ;             // Velocity in local space
        public float MAngularVelocityX;       // Angular velocity x-component
        public float MAngularVelocityY;            // Angular velocity y-component
        public float MAngularVelocityZ;            // Angular velocity z-component
        public float MAngularAccelerationX;        // Angular velocity x-component
        public float MAngularAccelerationY;   // Angular velocity y-component
        public float MAngularAccelerationZ;        // Angular velocity z-component
        public float MFrontWheelsAngle;            // Current front wheels angle in radians
    };
}