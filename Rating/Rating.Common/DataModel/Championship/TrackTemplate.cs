namespace SecondMonitor.Rating.Common.DataModel.Championship
{
    public class TrackTemplate
    {
        public TrackTemplate(string trackName) : this(trackName, 0)
        {

        }

        public TrackTemplate(string trackName, double layoutLength)
        {
            TrackName = trackName;
            LayoutLength = layoutLength;
        }

        public string TrackName { get; }
        public double LayoutLength { get; }
    }
}