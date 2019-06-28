using System;
using SecondMonitor.DataModel.Snapshot;
using SecondMonitor.DataModel.Snapshot.Drivers;

namespace SecondMonitor.DataModel.Telemetry
{
    using System.Diagnostics;
    using System.Xml.Serialization;
    using BasicProperties;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    [DebuggerDisplay("Lap time: {LapTime}")]
    public class TimedTelemetrySnapshot : TelemetrySnapshot
    {
        public TimedTelemetrySnapshot()
        {

        }

        public TimedTelemetrySnapshot(TimeSpan lapTime, DriverInfo playerInfo, WeatherInfo weatherInfo, InputInfo inputInfo, SimulatorSourceInfo simulatorSourceInfo) : base(playerInfo, weatherInfo, inputInfo, simulatorSourceInfo)
        {
            LapTime = lapTime;
        }

        [XmlIgnore]
        public TimeSpan LapTime { get; set; }

        [XmlAttribute]
        [ProtoMember(1, IsRequired = true)]
        public double LapTimeSeconds
        {
            get => LapTime.TotalSeconds;
            set => LapTime = TimeSpan.FromSeconds(value);
        }
    }
}