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

        public void AddResultsForCurrentSession(ChampionshipDto championship, SimulatorDataSet dataSet, bool shiftPlayerToLastPlace)
        {
            var currentEvent = championship.GetCurrentEvent();
            var currentSession = currentEvent.Sessions[championship.CurrentSessionIndex];
            currentSession.SessionResult = CreateResultDto(championship, dataSet, shiftPlayerToLastPlace);
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

        public void CommitLastSessionResults(ChampionshipDto championship)
        {
            var currentEvent = championship.GetCurrentEvent();
            var currentSession = currentEvent.Sessions[championship.CurrentSessionIndex];
            var guidDriverDictionary = championship.GetGuidToDriverDictionary();

            if (currentSession.SessionResult == null)
            {
                return;
            }

            foreach (DriverSessionResultDto driverSessionResultDto in currentSession.SessionResult.DriverSessionResult)
            {
                DriverDto driverDto = guidDriverDictionary[driverSessionResultDto.DriverGuid];
                driverDto.TotalPoints = driverSessionResultDto.TotalPoints;
                driverDto.Position = driverSessionResultDto.AfterEventPosition;
            }

            AdvanceChampionship(championship);
        }

        private void AdvanceChampionship(ChampionshipDto championship)
        {
            var currentEvent = championship.GetCurrentEvent();
            championship.CurrentSessionIndex = (championship.CurrentSessionIndex + 1) % currentEvent.Sessions.Count;
            if (championship.CurrentSessionIndex == 0)
            {
                championship.CurrentEventIndex++;
                currentEvent = championship.GetCurrentEvent();
                championship.NextTrack = currentEvent.TrackName;
            }

            championship.ChampionshipState = championship.CurrentEventIndex >= championship.Events.Count ? ChampionshipState.Finished : ChampionshipState.Started;
        }

        private SessionResultDto CreateResultDto(ChampionshipDto championship, SimulatorDataSet dataSet, bool shiftPlayerToLastPlace)
        {
            var scoring = championship.Scoring[championship.CurrentSessionIndex];
            Dictionary<string, int> positionMap = CreateFinishPositionDictionary(dataSet.DriversInfo.Where(x => x.CarClassId == dataSet.PlayerInfo.CarClassId).ToList(), shiftPlayerToLastPlace);
            SessionResultDto resultDto = new SessionResultDto();
            foreach (DriverDto championshipDriver in championship.Drivers)
            {
                DriverInfo sessionDriver = dataSet.DriversInfo.First(x => x.DriverName == championshipDriver.LastUsedName);
                int position = positionMap[sessionDriver.DriverName];
                DriverSessionResultDto driverResult = new DriverSessionResultDto()
                {
                    DriverGuid = championshipDriver.GlobalKey,
                    DriverName = championshipDriver.LastUsedName,
                    FinishPosition = position,
                    PointsGain = position <= scoring.Scoring.Count ? scoring.Scoring[position - 1] : 0,
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

        private static Dictionary<string, int> CreateFinishPositionDictionary(List<DriverInfo> eligibleDrivers, bool playerLast)
        {
            if (!playerLast)
            {
                return eligibleDrivers.ToDictionary(x => x.DriverName, x => x.PositionInClass);
            }

            bool playerShifted = false;
            Dictionary<string, int> positionMap = new Dictionary<string, int>();
            foreach (var driver in eligibleDrivers.OrderBy(x => x.Position))
            {
                if (driver.IsPlayer)
                {
                    var lastDriver = eligibleDrivers.Last(x => x.FinishStatus != DriverFinishStatus.Dnf);
                    positionMap[driver.DriverName] = lastDriver.PositionInClass;
                    playerShifted = true;
                    continue;
                }

                positionMap[driver.DriverName] = playerShifted ? driver.PositionInClass - 1 : driver.PositionInClass;
            }

            return positionMap;
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