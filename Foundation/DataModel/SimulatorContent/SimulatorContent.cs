namespace SecondMonitor.DataModel.SimulatorContent
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    public class SimulatorContent
    {
        public SimulatorContent()
        {

        }

        public SimulatorContent(string simulatorName)
        {
            SimulatorName = simulatorName;
            Tracks = new List<Track>();
            Classes = new List<CarClass>();
        }

        [XmlAttribute]
        public string SimulatorName { get; set; }

        public List<Track> Tracks { get; set; }
        public List<CarClass> Classes { get; set; }

        public void AddCar(string carName, string className)
        {
            CarClass carClass = GetOrCreateClass(className);
            carClass.AddCar(new Car(carName, className));
        }

        public void AddTrack(string trackName, double lapDistance)
        {
            if (Tracks.FirstOrDefault(x => x.Name == trackName) == null)
            {
                Tracks.Add(new Track(trackName, lapDistance));
            }
        }

        protected CarClass GetOrCreateClass(string className)
        {
            CarClass carClass = Classes.FirstOrDefault(x => x.ClassName == className);

            if (carClass == null)
            {
                carClass = new CarClass(className);
                Classes.Add(carClass);
            }

            return carClass;
        }

    }
}