namespace SecondMonitor.DataModel.Snapshot
{
    using System;

    using BasicProperties;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public sealed class WeatherInfo
    {
        public WeatherInfo()
        {

        }

        [ProtoMember(1)]
        public Temperature AirTemperature { get; set; } = Temperature.FromCelsius(-1);

        [ProtoMember(2)]
        public Temperature TrackTemperature { get; set; } = Temperature.FromCelsius(-1);

        [ProtoMember(3)]
        public int RainIntensity { get; set; } = 0;
    }
}
