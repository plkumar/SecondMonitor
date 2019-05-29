namespace SecondMonitor.DataModel.SimulatorContent
{
    using System.Xml.Serialization;

    public class Track
    {
        public Track()
        {

        }

        public Track(string name, double lapDistance)
        {
            Name = name;
            LapDistance = lapDistance;
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public double LapDistance { get; set; }

        protected bool Equals(Track other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Track) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}