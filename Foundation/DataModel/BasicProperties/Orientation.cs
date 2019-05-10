namespace SecondMonitor.DataModel.BasicProperties
{
    using System;

    [Serializable]
    public class Orientation
    {
        public Angle Roll { get; set; } = new Angle();
        public Angle Pitch { get; set; } = new Angle();
        public Angle Yaw { get; set; } = new Angle();
    }
}