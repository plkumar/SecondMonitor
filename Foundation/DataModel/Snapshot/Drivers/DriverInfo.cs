using System.Diagnostics;

namespace SecondMonitor.DataModel.Snapshot.Drivers
{
    using System;

    using BasicProperties;

    using Systems;
    using ProtoBuf;

    [ProtoContract]
    [Serializable]
    [DebuggerDisplay("Driver Name: {DriverName}")]
    public sealed class DriverInfo : IDriverInfo
    {
        public DriverInfo()
        {
            WorldPosition = new Point3D();
            Speed = Velocity.FromMs(0);
        }

        [ProtoMember(1)]
        public string DriverName { get; set; }

        [ProtoMember(2)]
        public string CarName { get; set; }

        [ProtoMember(3)]
        public string CarClassName { get; set; }

        [ProtoMember(4)]
        public string CarClassId { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public int CompletedLaps { get; set; }

        public bool InPits { get; set; }

        [ProtoMember(6, IsRequired = true)]
        public bool IsPlayer { get; set; }

        [ProtoMember(7, IsRequired = true)]
        public int Position { get; set; }

        [ProtoMember(8, IsRequired = true)]
        public int PositionInClass { get; set; }

        public bool CurrentLapValid { get; set; }

        [ProtoMember(9, IsRequired = true)]
        public double LapDistance { get; set; }

        public double TotalDistance { get; set; }

        public double DistanceToPlayer { get; set; }

        public bool IsBeingLappedByPlayer { get; set; } = false;

        public bool IsLappingPlayer { get; set; } = false;

        public DriverFinishStatus FinishStatus { get; set; } = DriverFinishStatus.Na;

        [ProtoMember(10)]
        public CarInfo CarInfo { get;set; } = new CarInfo();

        public DriverTimingInfo Timing { get; set; } = new DriverTimingInfo();

        [ProtoMember(11, IsRequired = true)]
        public Point3D WorldPosition { get; set; }

        public DriverDebugInfo DriverDebugInfo { get; } = new DriverDebugInfo();

        [ProtoMember(12, IsRequired = true)]
        public Velocity Speed { get; set; }
    }
}
