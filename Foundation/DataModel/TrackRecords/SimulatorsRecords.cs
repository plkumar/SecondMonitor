namespace SecondMonitor.DataModel.TrackRecords
{
    using System.Collections.Generic;
    using System.Linq;

    public class SimulatorsRecords
    {
        public SimulatorsRecords()
        {
            SimulatorRecords = new List<SimulatorRecords>();
        }
        public List<SimulatorRecords> SimulatorRecords { get; set; }

        public SimulatorRecords GetOrCreateSimulatorRecords(string simulatorName)
        {
            var simulatorRecord = SimulatorRecords.FirstOrDefault(x => x.SimulatorName == simulatorName);
            if (simulatorRecord == null)
            {
                simulatorRecord = new SimulatorRecords()
                {
                    SimulatorName = simulatorName
                };
                SimulatorRecords.Add(simulatorRecord);
            }

            return simulatorRecord;
        }
    }
}