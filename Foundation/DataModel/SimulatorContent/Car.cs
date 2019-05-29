namespace SecondMonitor.DataModel.SimulatorContent
{
    using System.Xml.Serialization;

    public class Car
    {
        public Car()
        {

        }

        public Car(string carName, string className)
        {
            CarName = carName;
            ClassName = className;
        }

        [XmlAttribute]
        public string CarName { get; set; }

        [XmlAttribute]
        public string ClassName { get; set; }

        protected bool Equals(Car other)
        {
            return string.Equals(CarName, other.CarName) && string.Equals(ClassName, other.ClassName);
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

            return Equals((Car) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((CarName != null ? CarName.GetHashCode() : 0) * 397) ^ (ClassName != null ? ClassName.GetHashCode() : 0);
            }
        }
    }
}