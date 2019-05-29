namespace SecondMonitor.DataModel.SimulatorContent
{
    using System.Collections.Generic;
    using System.Linq;

    public class SimulatorsContent
    {
        public SimulatorsContent()
        {
            SimulatorContents = new List<SimulatorContent>();
        }

        public List<SimulatorContent> SimulatorContents { get; set; }

        public void AddCar(string simulatorName, string carName, string className)
        {
            GetOrCreateSimulatorContent(simulatorName).AddCar(carName, className);
        }

        public void AddTrack(string simulatorName, string trackName, double lapDistance)
        {
           GetOrCreateSimulatorContent(simulatorName).AddTrack(trackName, lapDistance);
        }

        public SimulatorContent GetOrCreateSimulatorContent(string simulatorName)
        {
            SimulatorContent content = SimulatorContents.FirstOrDefault(x => x.SimulatorName == simulatorName);
            if (content == null)
            {
                content = new SimulatorContent(simulatorName);
                SimulatorContents.Add(content);
            }

            return content;
        }
    }
}