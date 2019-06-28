namespace SecondMonitor.DataModel.Telemetry
{
    using System;
    using BasicProperties;
    using ProtoBuf;
    using Snapshot;
    using Snapshot.Drivers;

    [ProtoContract]
    [Serializable]
    [ProtoInclude(100, typeof(TimedTelemetrySnapshot))]
    public class TelemetrySnapshot
    {
        public TelemetrySnapshot()
        {

        }

        public TelemetrySnapshot(DriverInfo playerInfo, WeatherInfo weatherInfo, InputInfo inputInfo, SimulatorSourceInfo simulatorSourceInfo)
        {
            PlayerData = playerInfo;
            WeatherInfo = weatherInfo;
            InputInfo = inputInfo;
            SimulatorSourceInfo = simulatorSourceInfo;
        }

        [ProtoMember(2)]
        public DriverInfo PlayerData { get; set; }

        [ProtoMember(3)]
        public WeatherInfo WeatherInfo { get; set; }

        [ProtoMember(4)]
        public InputInfo InputInfo { get; set; }

        [ProtoMember(5)]
        public SimulatorSourceInfo SimulatorSourceInfo { get; set; }
    }
}