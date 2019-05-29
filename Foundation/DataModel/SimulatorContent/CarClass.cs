namespace SecondMonitor.DataModel.SimulatorContent
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    public class CarClass
    {
        public CarClass()
        {

        }

        public CarClass(string className)
        {
            ClassName = className;
            Cars = new List<Car>();
        }


        [XmlAttribute]
        public string ClassName { get; set; }

        public List<Car> Cars { get; set; }

        public void AddCar(Car car)
        {
            if (Cars.Contains(car))
            {
                return;
            }

            Cars.Add(car);
        }

        public bool TryGetCar(string carName, out Car car)
        {
            car = Cars.FirstOrDefault(x => x.CarName == carName);
            return car != null;
        }
    }
}