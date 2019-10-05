namespace SecondMonitor.Rating.Application.Championship.Operations
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;

    public class ChampionshipManipulator : IChampionshipManipulator
    {
        public void StartChampionship(ChampionshipDto championship, SimulatorDataSet dataSet)
        {
            InitializeDrivers(championship, dataSet);
            championship.ChampionshipState = ChampionshipState.Started;
        }

        private void InitializeDrivers(ChampionshipDto championship, SimulatorDataSet dataSet)
        {
            List<DriverInfo> eligibleDrivers = dataSet.DriversInfo.Where(x => x.CarClassId == dataSet.PlayerInfo.CarClassId).ToList();
            championship.ClassName = dataSet.PlayerInfo.CarClassName;
            championship.TotalDrivers = eligibleDrivers.Count;
            championship.Drivers = eligibleDrivers.Select(x => new DriverDto()
            {
                LastUsedName = x.DriverName,
                IsPlayer = x.IsPlayer,
            }).ToList();

            championship.Position = eligibleDrivers.FindIndex(x => x.IsPlayer) + 1;
        }
    }
}