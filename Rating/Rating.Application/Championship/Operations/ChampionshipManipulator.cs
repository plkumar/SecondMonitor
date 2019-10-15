namespace SecondMonitor.Rating.Application.Championship.Operations
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.Events;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;

    public class ChampionshipManipulator : IChampionshipManipulator
    {
        public void StartChampionship(ChampionshipDto championship, SimulatorDataSet dataSet)
        {
            InitializeDrivers(championship, dataSet);
            championship.ChampionshipState = ChampionshipState.Started;
        }

        public void StartNextEvent(ChampionshipDto championship, SimulatorDataSet dataSet)
        {
            var currentEvent = championship.GetCurrentEvent();
            currentEvent.TrackName = dataSet.SessionInfo.TrackInfo.TrackFullName;
        }

        public void AddResultsForCurrentSession(ChampionshipDto championship, SimulatorDataSet dataSet)
        {
            var currentEvent = championship.GetCurrentEvent();
            var currentSession = currentEvent.Sessions[championship.CurrentSessionIndex];
            currentSession.SessionResult = CreateResultDto(championship, dataSet);
        }

        public void UpdateAiDriversNames(ChampionshipDto championship, SimulatorDataSet dataSet)
        {
            List<DriverDto> driverPool  = championship.Drivers.Where(x => !x.IsPlayer).ToList();
            List<DriverInfo> driversToAssign = dataSet.DriversInfo.Where(x => !x.IsPlayer && x.CarClassId == dataSet.PlayerInfo.CarClassId).ToList();
            //First iteration = use previous names
            foreach (DriverInfo driver in driversToAssign.ToList())
            {
                var driverMatch = driverPool.FirstOrDefault(x => x.LastUsedName == driver.DriverName);
                if (driverMatch == null)
                {
                    continue;
                }


                driverPool.Remove(driverMatch);
                driversToAssign.Remove(driver);
            }

            //Second iteration, try find drivers that previously used that name
            foreach (DriverInfo driver in driversToAssign.ToList())
            {
                var driverMatch = driverPool.FirstOrDefault(x => x.OtherNames.Contains(driver.DriverName));
                if (driverMatch == null)
                {
                    continue;
                }


                driverMatch.SetAnotherName(driver.DriverName);
                driverPool.Remove(driverMatch);
                driversToAssign.Remove(driver);
            }

            for (int i = 0; i < driverPool.Count; i++)
            {
                driverPool[i].SetAnotherName(driversToAssign[i].DriverName);
            }
        }

        private SessionResultDto CreateResultDto(ChampionshipDto championship, SimulatorDataSet dataSet)
        {
            var scoring = championship.Scoring[championship.CurrentSessionIndex];
            SessionResultDto resultDto = new SessionResultDto();
            foreach (DriverDto championshipDriver in championship.Drivers)
            {
                DriverInfo sessionDriver = dataSet.DriversInfo.First(x => x.DriverName == championshipDriver.LastUsedName);
                DriverSessionResultDto driverResult = new DriverSessionResultDto()
                {
                    DriverGuid = championshipDriver.GlobalKey,
                    DriverName = championshipDriver.LastUsedName,
                    FinishPosition = sessionDriver.Position,
                    PointsGain = sessionDriver.PositionInClass <= scoring.Scoring.Count ? scoring.Scoring[sessionDriver.PositionInClass - 1] : 0,
                    BeforeEventPosition = championshipDriver.Position,
                };
                driverResult.TotalPoints = championshipDriver.TotalPoints + driverResult.PointsGain;

                resultDto.DriverSessionResult.Add(driverResult);
            }

            DriverSessionResultComparer comparer = new DriverSessionResultComparer(championship);
            List<DriverSessionResultDto> driversAfterRaceOrdered = resultDto.DriverSessionResult.OrderBy(x => x, comparer).ToList();
            for (int i = 0; i < driversAfterRaceOrdered.Count; i ++ )
            {
                driversAfterRaceOrdered[i].AfterEventPosition = i + 1;
            }

            return resultDto;
        }

        private void InitializeDrivers(ChampionshipDto championship, SimulatorDataSet dataSet)
        {
            int position = 0;
            List<DriverInfo> eligibleDrivers = dataSet.DriversInfo.Where(x => x.CarClassId == dataSet.PlayerInfo.CarClassId).ToList();
            championship.ClassName = dataSet.PlayerInfo.CarClassName;
            championship.TotalDrivers = eligibleDrivers.Count;
            championship.Drivers = eligibleDrivers.Select(x => new DriverDto()
            {
                LastUsedName = x.DriverName,
                IsPlayer = x.IsPlayer,
                Position = ++position,
                LastCarName = x.CarName,
            }).ToList();

            championship.Position = eligibleDrivers.FindIndex(x => x.IsPlayer) + 1;
        }
    }
}