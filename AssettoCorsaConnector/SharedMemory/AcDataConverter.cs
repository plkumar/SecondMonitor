﻿namespace SecondMonitor.AssettoCorsaConnector.SharedMemory
{
    using System;

    using SecondMonitor.DataModel.BasicProperties;
    using SecondMonitor.DataModel.Snapshot;
    using SecondMonitor.DataModel.Snapshot.Drivers;
    using SecondMonitor.PluginManager.Extensions;

    public class AcDataConverter
    {
        private readonly AssettoCorsaConnector _connector;
        private DriverInfo _lastPlayer;

        public AcDataConverter(AssettoCorsaConnector assettoCorsaConnector)
        {
            _connector = assettoCorsaConnector;
        }

        public SimulatorDataSet CreateSimulatorDataSet(AssettoCorsaShared acData)
        {
            SimulatorDataSet simData = new SimulatorDataSet("Assetto Corsa");
            simData.SimulatorSourceInfo.HasLapTimeInformation = true;
            simData.SimulatorSourceInfo.OutLapIsValid = true;
            simData.SimulatorSourceInfo.SimNotReportingEndOfOutLapCorrectly = false;
            simData.SimulatorSourceInfo.SectorTimingSupport = DataInputSupport.NONE;

            FillSessionInfo(acData, simData);
            AddDriversData(simData, acData);

            FillPlayersGear(acData, simData);

            // PEDAL INFO
            AddPedalInfo(acData, simData);

            // WaterSystemInfo
            AddWaterSystemInfo(simData);

            // OilSystemInfo
            AddOilSystemInfo(simData);

            // Brakes Info
            AddBrakesInfo(acData, simData);

            // Tyre Pressure Info
            AddTyresAndFuelInfo(simData, acData);

            // Acceleration
            AddAcceleration(simData, acData);



            return simData;
        }

        private static void AddAcceleration(SimulatorDataSet simData, AssettoCorsaShared acData)
        {
            simData.PlayerInfo.CarInfo.Acceleration.XinMs = acData.AcsPhysics.accG[0] * 10;
            simData.PlayerInfo.CarInfo.Acceleration.YinMs = acData.AcsPhysics.accG[1] * 10;
            simData.PlayerInfo.CarInfo.Acceleration.ZinMs = acData.AcsPhysics.accG[2] * 10;
        }

        private static void AddTyresAndFuelInfo(SimulatorDataSet simData, AssettoCorsaShared acData)
        {
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.TyrePressure = Pressure.FromPsi(acData.AcsPhysics.wheelsPressure[(int)AcWheels.FL]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.TyrePressure = Pressure.FromPsi(acData.AcsPhysics.wheelsPressure[(int)AcWheels.FR]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.TyrePressure = Pressure.FromPsi(acData.AcsPhysics.wheelsPressure[(int)AcWheels.RL]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.TyrePressure = Pressure.FromPsi(acData.AcsPhysics.wheelsPressure[(int)AcWheels.RR]);

            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.TyreWear = 1 - (acData.AcsPhysics.tyreWear[(int)AcWheels.FL] / 100);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.TyreWear = 1 - (acData.AcsPhysics.tyreWear[(int)AcWheels.FR] / 100);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.TyreWear = 1 - (acData.AcsPhysics.tyreWear[(int)AcWheels.RL] / 100);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.TyreWear = 1 - (acData.AcsPhysics.tyreWear[(int)AcWheels.FR] / 100);

            // Front Left Tyre Temps
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.LeftTyreTemp = Temperature.FromCelsius(acData.AcsPhysics.tyreTempI[(int)AcWheels.FL]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.RightTyreTemp = Temperature.FromCelsius(acData.AcsPhysics.tyreTempM[(int)AcWheels.FL]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.CenterTyreTemp = Temperature.FromCelsius(acData.AcsPhysics.tyreTempO[(int)AcWheels.FL]);

            // Front Right Tyre Temps
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.LeftTyreTemp = Temperature.FromCelsius(acData.AcsPhysics.tyreTempI[(int)AcWheels.FR]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.RightTyreTemp = Temperature.FromCelsius(acData.AcsPhysics.tyreTempM[(int)AcWheels.FR]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.CenterTyreTemp = Temperature.FromCelsius(acData.AcsPhysics.tyreTempO[(int)AcWheels.FR]);


            // Rear Left Tyre Temps
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.LeftTyreTemp = Temperature.FromCelsius(acData.AcsPhysics.tyreTempI[(int)AcWheels.RL]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.RightTyreTemp = Temperature.FromCelsius(acData.AcsPhysics.tyreTempM[(int)AcWheels.RL]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.CenterTyreTemp = Temperature.FromCelsius(acData.AcsPhysics.tyreTempO[(int)AcWheels.RL]);

            // Rear Right Tyre Temps
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.LeftTyreTemp = Temperature.FromCelsius(acData.AcsPhysics.tyreTempI[(int)AcWheels.RR]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.RightTyreTemp = Temperature.FromCelsius(acData.AcsPhysics.tyreTempM[(int)AcWheels.RR]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.CenterTyreTemp = Temperature.FromCelsius(acData.AcsPhysics.tyreTempO[(int)AcWheels.RR]);

            // Fuel System
            simData.PlayerInfo.CarInfo.FuelSystemInfo.FuelCapacity = Volume.FromLiters(acData.AcsStatic.maxFuel);
            simData.PlayerInfo.CarInfo.FuelSystemInfo.FuelRemaining = Volume.FromLiters(acData.AcsPhysics.fuel);
        }

        private static void AddBrakesInfo(AssettoCorsaShared acData, SimulatorDataSet simData)
        {
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.BrakeTemperature = Temperature.FromCelsius(acData.AcsPhysics.brakeTemp[(int)AcWheels.FL]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.BrakeTemperature = Temperature.FromCelsius(acData.AcsPhysics.brakeTemp[(int)AcWheels.FR]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.BrakeTemperature = Temperature.FromCelsius(acData.AcsPhysics.brakeTemp[(int)AcWheels.RL]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.BrakeTemperature = Temperature.FromCelsius(acData.AcsPhysics.brakeTemp[(int)AcWheels.RR]);
        }

        private static void AddOilSystemInfo(SimulatorDataSet simData)
        {
            simData.PlayerInfo.CarInfo.OilSystemInfo.OilTemperature = Temperature.FromCelsius(100);
        }

        private static void AddWaterSystemInfo(SimulatorDataSet simData)
        {
            simData.PlayerInfo.CarInfo.WaterSystemInfo.WaterTemperature = Temperature.FromCelsius(100);
        }

        private static void AddPedalInfo(AssettoCorsaShared acData, SimulatorDataSet simData)
        {
            simData.PedalInfo.ThrottlePedalPosition = acData.AcsPhysics.gas;
            simData.PedalInfo.BrakePedalPosition = acData.AcsPhysics.brake;
            simData.PedalInfo.ClutchPedalPosition = 1 - acData.AcsPhysics.clutch;
        }


        private static void FillPlayersGear(AssettoCorsaShared acData, SimulatorDataSet simData)
        {
            int gear = acData.AcsPhysics.gear - 1;
            switch (gear)
            {
                case 0:
                    simData.PlayerInfo.CarInfo.CurrentGear = "N";
                    break;
                case -1:
                    simData.PlayerInfo.CarInfo.CurrentGear = "R";
                    break;
                case -2:
                    simData.PlayerInfo.CarInfo.CurrentGear = string.Empty;
                    break;
                default:
                    simData.PlayerInfo.CarInfo.CurrentGear = gear.ToString();
                    break;
            }
        }

        internal void AddDriversData(SimulatorDataSet data, AssettoCorsaShared acData)
        {
            if (acData.AcsSecondMonitor.numVehicles  < 1)
            {
                return;
            }

            data.DriversInfo = new DriverInfo[acData.AcsSecondMonitor.numVehicles];
            DriverInfo playersInfo = null;

            for (int i = 0; i < acData.AcsSecondMonitor.numVehicles; i++)
            {
                AcsVehicleInfo acVehicleInfo = acData.AcsSecondMonitor.vehicle[i];
                DriverInfo driverInfo = CreateDriverInfo(acData, acVehicleInfo);

                if (driverInfo.IsPlayer)
                {
                    playersInfo = driverInfo;

                }

                driverInfo.CurrentLapValid = acVehicleInfo.currentLapInvalid == 0;
                data.DriversInfo[i] = driverInfo;
                if (driverInfo.Position == 1)
                {
                    data.SessionInfo.LeaderCurrentLap = driverInfo.CompletedLaps + 1;
                    data.LeaderInfo = driverInfo;
                }

                AddLappingInformation(data, acData, driverInfo);
                FillTimingInfo(driverInfo, acVehicleInfo);


            }
            CheckValidityByPlayer(playersInfo);
            _lastPlayer = playersInfo;
            if (playersInfo != null)
            {
                data.PlayerInfo = playersInfo;
            }
        }

        private void CheckValidityByPlayer(DriverInfo driver)
        {
            if (_lastPlayer == null || driver == null || (!_lastPlayer.InPits && driver.InPits))
            {
                return;
            }

            /*Distance distance = Point3D.GetDistance(driver.WorldPosition, _lastPlayer.WorldPosition);
            if (distance.DistanceInM > 200)
            {
                currentlyIgnoredPackage++;
                if (currentlyIgnoredPackage < MaxConsecutivePackagesIgnored)
                {
                    throw new RF2InvalidPackageException("Players distance was :" + distance.DistanceInM);
                }
            }*/

        }

        internal void FillTimingInfo(DriverInfo driverInfo, AcsVehicleInfo acVehicleInfo)
        {
            driverInfo.Timing.LastLapTime = CreateTimeSpan(acVehicleInfo.lastLapTimeMS);
            driverInfo.Timing.CurrentLapTime = CreateTimeSpan(acVehicleInfo.currentLapTimeMS);
            driverInfo.Timing.CurrentSector = -1;
        }

        private TimeSpan CreateTimeSpan(double miliseconds)
        {
            return miliseconds > 0 ? TimeSpan.FromMilliseconds(miliseconds) : TimeSpan.Zero;
        }

        private void AddLappingInformation(SimulatorDataSet data, AssettoCorsaShared acData, DriverInfo driverInfo)
        {
            if (data.SessionInfo.SessionType == SessionType.Race && _lastPlayer != null
                && _lastPlayer.CompletedLaps != 0)
            {
                driverInfo.IsBeingLappedByPlayer =
                    driverInfo.TotalDistance < (_lastPlayer.TotalDistance - acData.AcsStatic.trackSPlineLength * 0.5);
                driverInfo.IsLappingPlayer =
                    _lastPlayer.TotalDistance < (driverInfo.TotalDistance - acData.AcsStatic.trackSPlineLength * 0.5);
            }
        }

        private DriverInfo CreateDriverInfo(AssettoCorsaShared acData, AcsVehicleInfo acVehicleInfo)
        {
            DriverInfo driverInfo = new DriverInfo
            {
                DriverName = StringExtensions.FromArray(acVehicleInfo.driverName),
                CompletedLaps = acVehicleInfo.lapCount,
                CarName = StringExtensions.FromArray(acVehicleInfo.carModel),
                InPits = acVehicleInfo.isCarInPit == 1 || acVehicleInfo.isCarInPitlane == 1
            };

            driverInfo.IsPlayer = acVehicleInfo.carId == 0;
            driverInfo.Position = acVehicleInfo.carLeaderboardPosition;
            driverInfo.Speed = Velocity.FromMs(acVehicleInfo.speedMS);
            driverInfo.LapDistance = acData.AcsStatic.trackSPlineLength * acVehicleInfo.spLineLength;
            driverInfo.TotalDistance = acVehicleInfo.lapCount * acData.AcsStatic.trackSPlineLength + acVehicleInfo.spLineLength * acData.AcsStatic.trackSPlineLength;
            driverInfo.FinishStatus = FromAcStatus(acVehicleInfo.finishedStatus);
            driverInfo.WorldPosition = new Point3D(Distance.FromMeters(acVehicleInfo.worldPosition.x), Distance.FromMeters(acVehicleInfo.worldPosition.y), Distance.FromMeters(acVehicleInfo.worldPosition.z));
            ComputeDistanceToPlayer(_lastPlayer, driverInfo, acData);
            return driverInfo;
        }

        internal static void ComputeDistanceToPlayer(DriverInfo player, DriverInfo driverInfo, AssettoCorsaShared assettoCorsaShared)
        {
            if (player == null)
            {
                return;
            }

            if (driverInfo.FinishStatus == DriverInfo.DriverFinishStatus.Dq || driverInfo.FinishStatus == DriverInfo.DriverFinishStatus.Dnf ||
                driverInfo.FinishStatus == DriverInfo.DriverFinishStatus.Dnq || driverInfo.FinishStatus == DriverInfo.DriverFinishStatus.Dns)
            {
                driverInfo.DistanceToPlayer = double.MaxValue;
                return;
            }

            double trackLength = assettoCorsaShared.AcsStatic.trackSPlineLength;
            double playerLapDistance = player.LapDistance;

            double distanceToPlayer = playerLapDistance - driverInfo.LapDistance;
            if (distanceToPlayer < -(trackLength / 2))
            {
                distanceToPlayer = distanceToPlayer + trackLength;
            }

            if (distanceToPlayer > (trackLength / 2))
            {
                distanceToPlayer = distanceToPlayer - trackLength;
            }

            driverInfo.DistanceToPlayer = distanceToPlayer;
        }

        internal void FillSessionInfo(AssettoCorsaShared data, SimulatorDataSet simData)
        {
            // Timing
            simData.SessionInfo.SessionTime = _connector.SessionTime;
            simData.SessionInfo.TrackInfo.LayoutLength = data.AcsStatic.trackSPlineLength;
            simData.SessionInfo.TrackInfo.TrackName = data.AcsStatic.track;
            simData.SessionInfo.TrackInfo.TrackLayoutName = data.AcsStatic.trackConfiguration;
            simData.SessionInfo.WeatherInfo.AirTemperature = Temperature.FromCelsius(data.AcsPhysics.airTemp);
            simData.SessionInfo.WeatherInfo.TrackTemperature = Temperature.FromCelsius(data.AcsPhysics.roadTemp);
            simData.SessionInfo.WeatherInfo.RainIntensity = 0;

            switch (data.AcsGraphic.session)
            {
                case AcSessionType.AC_DRAG:
                case AcSessionType.AC_PRACTICE:
                case AcSessionType.AC_TIME_ATTACK:
                case AcSessionType.AC_HOT_LAP:
                    simData.SessionInfo.SessionType = SessionType.Practice;
                    break;
                case AcSessionType.AC_QUALIFY:
                    simData.SessionInfo.SessionType = SessionType.Qualification;
                    break;
                case AcSessionType.AC_DRIFT:
                case AcSessionType.AC_RACE:
                    simData.SessionInfo.SessionType = SessionType.Race;
                    break;
                default:
                    simData.SessionInfo.SessionType = SessionType.Na;
                    break;
            }

            simData.SessionInfo.SessionPhase = SessionPhase.Green;

            simData.SessionInfo.IsActive = data.AcsGraphic.status == AcStatus.AC_LIVE;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if ((data.AcsStatic.isTimedRace == 1 || simData.SessionInfo.SessionType != SessionType.Race)
                && data.AcsGraphic.sessionTimeLeft < 0)
            {

                simData.SessionInfo.SessionLengthType = SessionLengthType.Time;
                simData.SessionInfo.SessionTimeRemaining = 120000;
            }
            else if (data.AcsStatic.isTimedRace == 1 || simData.SessionInfo.SessionType != SessionType.Race)
            {
                simData.SessionInfo.SessionLengthType = SessionLengthType.Time;
                simData.SessionInfo.SessionTimeRemaining = data.AcsGraphic.sessionTimeLeft / 1000;
            }
            else
            {
                simData.SessionInfo.SessionLengthType = SessionLengthType.Laps;
                simData.SessionInfo.TotalNumberOfLaps = data.AcsGraphic.numberOfLaps;
            }
        }

        internal static DriverInfo.DriverFinishStatus FromAcStatus(int finishStatus)
        {
            return finishStatus == 0 ? DriverInfo.DriverFinishStatus.None : DriverInfo.DriverFinishStatus.Finished;
        }
    }
}