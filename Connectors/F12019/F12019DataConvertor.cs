namespace SecondMonitor.F12019Connector
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Eventing.Reader;
    using System.Linq;
    using Datamodel;
    using DataModel.BasicProperties;
    using Datamodel.enums;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using DataModel.Snapshot.Systems;
    using PluginManager.Extensions;
    using PluginManager.GameConnector;

    internal class F12019DataConvertor : AbstractDataConvertor
    {
        public const string ConnectorName = "F1 2019";
        private readonly float[] _lapTimeCache;
        private readonly float[] _sector2TimeCache;
        private DriverInfo _lastPlayer;
        private double _layoutLength;
        private int _playerDataIndex;

        internal F12019DataConvertor()
        {
            _lapTimeCache = new float[20];
            _sector2TimeCache = new float[20];
        }

        internal SimulatorDataSet ConvertData(AllPacketsComposition data)
        {
            SimulatorDataSet simData = new SimulatorDataSet(ConnectorName)
            {
                SimulatorSourceInfo =
                {
                    HasLapTimeInformation = true,
                    SectorTimingSupport = DataInputSupport.Full,
                    AIInstantFinish = true,
                    GapInformationProvided = GapInformationKind.None,
                    SimNotReportingEndOfOutLapCorrectly = true,
                    HasRewindFunctionality = true,
                    OverrideBestLap = true,
                    TelemetryInfo =
                    {
                        ContainsOptimalTemperatures = true

                    }
                }
            };

            FillSessionInfo(data, simData);
            AddDriversData(ref data, simData);

            var rawTelemetryData = data.PacketCarTelemetryData.MCarTelemetryData[_playerDataIndex];
            var rawCarStatusInfo = data.PacketCarStatusData.MCarStatusData[_playerDataIndex];
            var rawCarMotionData = data.PacketMotionData.MCarMotionData[_playerDataIndex];

            FillPlayerCarInfo(ref rawTelemetryData, simData);

            // PEDAL INFO
            AddPedalInfo(ref rawTelemetryData, simData);

            // Add Engine Temperatures
            AddEngineTemperatures(ref rawTelemetryData, simData);

            // Brakes Info
            AddBrakesInfo(ref rawTelemetryData, simData);

            // Tyre Pressure Info
            AddTyresInfo(ref rawTelemetryData, ref rawCarStatusInfo, ref data.PacketMotionData, simData);

            // Acceleration
            AddAcceleration(ref rawCarMotionData, simData);

            //Add Additional Player Car Info
            AddPlayerCarInfo(ref rawCarStatusInfo, ref rawTelemetryData, simData);

            //Add Flags Info
            AddFlags(ref data.PacketSessionData, simData);

            simData.SessionInfo.IsMultiClass = simData.DriversInfo != null && simData.DriversInfo.Any(x => x != null && x.Position != x.PositionInClass);

            return simData;
        }


        private void ComputeDistanceToPlayer(DriverInfo player, DriverInfo driverInfo)
        {
            if (player == null)
            {
                return;
            }

            if (driverInfo.FinishStatus == DriverFinishStatus.Dq || driverInfo.FinishStatus == DriverFinishStatus.Dnf ||
                driverInfo.FinishStatus == DriverFinishStatus.Dnq || driverInfo.FinishStatus == DriverFinishStatus.Dns)
            {
                driverInfo.DistanceToPlayer = double.MaxValue;
                return;
            }

            double trackLength = _layoutLength;
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

        private static DriverFinishStatus TranslateFinishStatus(int resultStatus)
        {
            switch ((DriverResultKind)resultStatus)
            {
                case DriverResultKind.Inactive:
                    return DriverFinishStatus.Dns;
                case DriverResultKind.Active:
                    return DriverFinishStatus.None;
                case DriverResultKind.Finished:
                    return DriverFinishStatus.Finished;
                case DriverResultKind.Disqualified:
                    return DriverFinishStatus.Dnq;
                case DriverResultKind.NotClassified:
                    return DriverFinishStatus.Dnf;
                default:
                    return DriverFinishStatus.Na;
            }
        }

        private static void AddAcceleration(ref CarMotionData data, SimulatorDataSet simData)
        {
            simData.PlayerInfo.CarInfo.Acceleration.XinG = data.MGForceLateral;
            simData.PlayerInfo.CarInfo.Acceleration.YinG = data.MGForceVertical;
            simData.PlayerInfo.CarInfo.Acceleration.ZinG = data.MGForceLongitudinal;
        }

        private static void AddTyresInfo(ref CarTelemetryData rawTelemetryData, ref CarStatusData rawCarStatusData, ref PacketMotionData rawMotionData, SimulatorDataSet simData)
        {
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.TyrePressure.ActualQuantity = Pressure.FromPsi(rawTelemetryData.MTyresPressure[TyreIndices.FrontLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.TyrePressure.ActualQuantity = Pressure.FromPsi(rawTelemetryData.MTyresPressure[TyreIndices.FrontRight]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.TyrePressure.ActualQuantity = Pressure.FromPsi(rawTelemetryData.MTyresPressure[TyreIndices.RearLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.TyrePressure.ActualQuantity = Pressure.FromPsi(rawTelemetryData.MTyresPressure[TyreIndices.RearRight]);

            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.TyreWear.ActualWear = rawCarStatusData.MTyresWear[TyreIndices.FrontLeft] / 100.0;
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.TyreWear.ActualWear = rawCarStatusData.MTyresWear[TyreIndices.FrontRight] / 100.0;
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.TyreWear.ActualWear = rawCarStatusData.MTyresWear[TyreIndices.RearLeft] / 100.0;
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.TyreWear.ActualWear = rawCarStatusData.MTyresWear[TyreIndices.RearRight] / 100.0;

            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.Rps = rawMotionData.MWheelSpeed[TyreIndices.FrontLeft];
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.Rps = rawMotionData.MWheelSpeed[TyreIndices.FrontRight];
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.Rps = rawMotionData.MWheelSpeed[TyreIndices.RearLeft];
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.Rps = rawMotionData.MWheelSpeed[TyreIndices.RearRight];

            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.SuspensionTravel = Distance.FromCentimeters(rawMotionData.MSuspensionPosition[TyreIndices.FrontLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.SuspensionTravel = Distance.FromCentimeters(rawMotionData.MSuspensionPosition[TyreIndices.FrontRight]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.SuspensionTravel = Distance.FromCentimeters(rawMotionData.MSuspensionPosition[TyreIndices.RearLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.SuspensionTravel = Distance.FromCentimeters(rawMotionData.MSuspensionPosition[TyreIndices.RearRight]);

            /*simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.SuspensionVelocity = Velocity.FromMs(rawMotionData.MSuspensionVelocity[TyreIndices.FrontLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.SuspensionVelocity = Velocity.FromMs(rawMotionData.MSuspensionVelocity[TyreIndices.FrontRight]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.SuspensionVelocity = Velocity.FromMs(rawMotionData.MSuspensionVelocity[TyreIndices.RearLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.SuspensionVelocity = Velocity.FromMs(rawMotionData.MSuspensionVelocity[TyreIndices.RearRight]);*/

            // Front Left Tyre Temps
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.CenterTyreTemp.IdealQuantity = Temperature.FromCelsius(85);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.CenterTyreTemp.IdealQuantityWindow = Temperature.FromCelsius(20);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.LeftTyreTemp.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresSurfaceTemperature[TyreIndices.FrontLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.RightTyreTemp.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresSurfaceTemperature[TyreIndices.FrontLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.CenterTyreTemp.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresSurfaceTemperature[TyreIndices.FrontLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.TyreCoreTemperature.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresInnerTemperature[TyreIndices.FrontLeft]);


            // Front Right Tyre Temps
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.CenterTyreTemp.IdealQuantity = Temperature.FromCelsius(85);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.CenterTyreTemp.IdealQuantityWindow = Temperature.FromCelsius(20);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.LeftTyreTemp.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresSurfaceTemperature[TyreIndices.FrontRight]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.RightTyreTemp.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresSurfaceTemperature[TyreIndices.FrontRight]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.CenterTyreTemp.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresSurfaceTemperature[TyreIndices.FrontRight]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.TyreCoreTemperature.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresInnerTemperature[TyreIndices.FrontRight]);


            // Rear Left Tyre Temps
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.CenterTyreTemp.IdealQuantity = Temperature.FromCelsius(85);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.CenterTyreTemp.IdealQuantityWindow = Temperature.FromCelsius(20);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.LeftTyreTemp.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresSurfaceTemperature[TyreIndices.RearLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.RightTyreTemp.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresSurfaceTemperature[TyreIndices.RearLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.CenterTyreTemp.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresSurfaceTemperature[TyreIndices.RearLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.TyreCoreTemperature.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresInnerTemperature[TyreIndices.RearLeft]);

            // Rear Right Tyre Temps
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.CenterTyreTemp.IdealQuantity = Temperature.FromCelsius(85);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.CenterTyreTemp.IdealQuantityWindow = Temperature.FromCelsius(20);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.LeftTyreTemp.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresSurfaceTemperature[TyreIndices.RearRight]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.RightTyreTemp.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresSurfaceTemperature[TyreIndices.RearRight]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.CenterTyreTemp.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresSurfaceTemperature[TyreIndices.RearRight]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.TyreCoreTemperature.ActualQuantity = Temperature.FromCelsius(rawTelemetryData.MTyresInnerTemperature[TyreIndices.RearRight]);

            // Fuel System
            simData.PlayerInfo.CarInfo.FuelSystemInfo.FuelCapacity = Volume.FromLiters(rawCarStatusData.MFuelCapacity);
            simData.PlayerInfo.CarInfo.FuelSystemInfo.FuelRemaining = Volume.FromLiters(rawCarStatusData.MFuelInTank);
        }

        private static void AddBrakesInfo(ref CarTelemetryData data, SimulatorDataSet simData)
        {

            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.BrakeTemperature.ActualQuantity = Temperature.FromCelsius(data.MBrakesTemperature[TyreIndices.FrontLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.BrakeTemperature.IdealQuantityWindow = Temperature.FromCelsius(200);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontLeft.BrakeTemperature.IdealQuantity = Temperature.FromCelsius(600);

            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.BrakeTemperature.ActualQuantity = Temperature.FromCelsius(data.MBrakesTemperature[TyreIndices.FrontRight]);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.BrakeTemperature.IdealQuantityWindow = Temperature.FromCelsius(200);
            simData.PlayerInfo.CarInfo.WheelsInfo.FrontRight.BrakeTemperature.IdealQuantity = Temperature.FromCelsius(600);

            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.BrakeTemperature.ActualQuantity = Temperature.FromCelsius(data.MBrakesTemperature[TyreIndices.RearLeft]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.BrakeTemperature.IdealQuantityWindow = Temperature.FromCelsius(200);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearLeft.BrakeTemperature.IdealQuantity = Temperature.FromCelsius(600);

            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.BrakeTemperature.ActualQuantity = Temperature.FromCelsius(data.MBrakesTemperature[TyreIndices.RearRight]);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.BrakeTemperature.IdealQuantityWindow = Temperature.FromCelsius(200);
            simData.PlayerInfo.CarInfo.WheelsInfo.RearRight.BrakeTemperature.IdealQuantity = Temperature.FromCelsius(600);
        }

        private static void AddEngineTemperatures(ref CarTelemetryData data, SimulatorDataSet simData)
        {

            simData.PlayerInfo.CarInfo.OilSystemInfo.OptimalOilTemperature.ActualQuantity = Temperature.FromCelsius(data.MEngineTemperature);
            simData.PlayerInfo.CarInfo.WaterSystemInfo.OptimalWaterTemperature = simData.PlayerInfo.CarInfo.OilSystemInfo.OptimalOilTemperature;
        }

        private static void AddPedalInfo(ref CarTelemetryData rawData, SimulatorDataSet simData)
        {

            simData.InputInfo.ThrottlePedalPosition = rawData.MThrottle;
            simData.InputInfo.BrakePedalPosition = rawData.MBrake;
            simData.InputInfo.ClutchPedalPosition = rawData.MClutch / 100.0;
            simData.InputInfo.SteeringInput = rawData.MSteer;
            simData.InputInfo.WheelAngle = 180 * rawData.MSteer;
            simData.InputInfo.WheelAngleFilled = true;
        }

        private void AddDriversData(ref AllPacketsComposition rawData, SimulatorDataSet data)
        {
            int numOfCars = data.SessionInfo.SessionType == SessionType.Race ? 20 : rawData.PacketParticipantsData.MNumActiveCars;

            List<DriverInfo> driverInfos = new List<DriverInfo>(20);
            DriverInfo playersInfo = null;

            for (int i = 0; i < numOfCars; i++)
            {
                var rawDriverData = rawData.PacketParticipantsData.MParticipants[i];
                string driverName = rawDriverData.MName.FromArray();
                if (string.IsNullOrWhiteSpace(driverName))
                {
                    continue;
                }
                DriverInfo driverInfo = CreateDriverInfo(rawData, driverName, i);
                if (driverInfo.IsPlayer)
                {
                    playersInfo = driverInfo;
                }

                AddWheelInfo(driverInfo, rawData.PacketCarStatusData.MCarStatusData[i]);
                driverInfos.Add(driverInfo);
                if (driverInfo.Position == 1)
                {
                    data.SessionInfo.LeaderCurrentLap = driverInfo.CompletedLaps + 1;
                    data.LeaderInfo = driverInfo;
                }

                AddLappingInformation(data, driverInfo);
            }

            if (playersInfo != null)
            {
                data.PlayerInfo = playersInfo;
                _lastPlayer = playersInfo;
            }

            data.DriversInfo = driverInfos.ToArray();
        }

       private static void AddWheelInfo(DriverInfo driverInfo, CarStatusData rawCarStatus)
       {
           string tyreType = TranslationTable.GetTyreCompound(rawCarStatus.MTyreVisualCompound);
           driverInfo.CarInfo.WheelsInfo.FrontLeft.TyreType = tyreType;
           driverInfo.CarInfo.WheelsInfo.FrontRight.TyreType = tyreType;

           driverInfo.CarInfo.WheelsInfo.RearLeft.TyreType = tyreType;
           driverInfo.CarInfo.WheelsInfo.RearRight.TyreType = tyreType;

           string tyreVisualType = TranslationTable.GetTyreVisualCompound(rawCarStatus.MTyreVisualCompound);
           driverInfo.CarInfo.WheelsInfo.FrontLeft.TyreVisualType = tyreVisualType;
           driverInfo.CarInfo.WheelsInfo.FrontRight.TyreVisualType = tyreVisualType;

           driverInfo.CarInfo.WheelsInfo.RearLeft.TyreVisualType = tyreVisualType;
           driverInfo.CarInfo.WheelsInfo.RearRight.TyreVisualType = tyreVisualType;
        }

        private void AddLappingInformation(SimulatorDataSet data, DriverInfo driverInfo)
        {
            if (data.SessionInfo.SessionType != SessionType.Race || _lastPlayer == null || _lastPlayer.CompletedLaps == 0)
            {
                return;
            }

            driverInfo.IsBeingLappedByPlayer = driverInfo.TotalDistance < (_lastPlayer.TotalDistance - _layoutLength * 0.5);
            driverInfo.IsLappingPlayer = _lastPlayer.TotalDistance < (driverInfo.TotalDistance - _layoutLength * 0.5);
        }

        private DriverInfo CreateDriverInfo(AllPacketsComposition rawData, string driverName, int driverIndex)
        {
            var rawDriverData = rawData.PacketParticipantsData.MParticipants[driverIndex];
            var rawDriverLapInfo = rawData.PacketLapData.MLapData[driverIndex];
            var rawCarMotionData = rawData.PacketMotionData.MCarMotionData[driverIndex];
            DriverInfo driverInfo = new DriverInfo
            {
                DriverName = driverName,
                CompletedLaps = rawDriverLapInfo.MCurrentLapNum - 1,
                CarName = string.Empty,
                InPits = rawDriverLapInfo.MPitStatus != 0 || rawDriverLapInfo.MDriverStatus == 0,
                IsPlayer = driverIndex == rawData.PacketCarTelemetryData.MHeader.MPlayerCarIndex,
                Position = rawDriverLapInfo.MCarPosition,
                PositionInClass = rawDriverLapInfo.MCarPosition,
                Speed = Velocity.FromKph(rawData.PacketCarTelemetryData.MCarTelemetryData[driverIndex].MSpeed),
                LapDistance = rawDriverLapInfo.MLapDistance,
                TotalDistance = rawDriverLapInfo.MTotalDistance
            };

            driverInfo.CarName = TranslationTable.GetCarName(rawDriverData.MTeamId);
            driverInfo.CarClassName = TranslationTable.GetClass(rawData.PacketSessionData.MFormula);
            driverInfo.CarClassId = driverInfo.CarClassName;
            driverInfo.FinishStatus = rawData.AdditionalData.RetiredDrivers[driverIndex] ? DriverFinishStatus.Dnf : TranslateFinishStatus(rawDriverLapInfo.MResultStatus);
            driverInfo.WorldPosition = new Point3D(Distance.FromMeters(rawCarMotionData.MWorldPositionX), Distance.FromMeters(rawCarMotionData.MWorldPositionY), Distance.FromMeters(rawCarMotionData.MWorldPositionZ));
            driverInfo.CurrentLapValid = rawDriverLapInfo.MCurrentLapInvalid == 0 && rawDriverLapInfo.MDriverStatus != 2 && rawDriverLapInfo.MDriverStatus != 3;

            ComputeDistanceToPlayer(_lastPlayer, driverInfo);
            FillTimingInfo(driverInfo, rawDriverLapInfo, driverIndex);
            return driverInfo;
        }

        private void FillTimingInfo(DriverInfo driverInfo, LapData rawLapData, int driverIndex)
        {
            driverInfo.Timing.CurrentSector = rawLapData.MSector + 1 ;
            driverInfo.Timing.LastSector1Time = TimeSpan.FromSeconds(driverInfo.Timing.CurrentSector == 1 ? rawLapData.MCurrentLapTime : rawLapData.MSector1Time);
            driverInfo.Timing.LastSector2Time = TimeSpan.FromSeconds(rawLapData.MCurrentLapTime - rawLapData.MSector1Time);

            if (driverInfo.Timing.CurrentSector == 3)
            {
                _sector2TimeCache[driverIndex] = rawLapData.MSector2Time;
                _lapTimeCache[driverIndex] = rawLapData.MCurrentLapTime;
            }

            driverInfo.Timing.CurrentLapTime = TimeSpan.FromSeconds(rawLapData.MCurrentLapTime);
            driverInfo.Timing.LastSector3Time = TimeSpan.FromSeconds(driverInfo.Timing.CurrentSector == 3 ? rawLapData.MCurrentLapTime - rawLapData.MSector2Time : _lapTimeCache[driverIndex] - _sector2TimeCache[driverIndex]);
            driverInfo.Timing.LastLapTime = TimeSpan.FromSeconds(driverInfo.Timing.CurrentSector == 3 ? rawLapData.MLastLapTime : _lapTimeCache[driverIndex]);
            driverInfo.Timing.BestLapTime = TimeSpan.FromSeconds(rawLapData.MBestLapTime);
        }



        private void AddPlayerCarInfo(ref CarStatusData rawCarStatusData, ref CarTelemetryData rawCarTelemetryData, SimulatorDataSet simData)
        {
            CarInfo playerCar = simData.PlayerInfo.CarInfo;

            playerCar.CarDamageInformation.Bodywork.MediumDamageThreshold = 0.1;
            playerCar.CarDamageInformation.Engine.HeavyDamageThreshold = 0.9;
            playerCar.CarDamageInformation.Transmission.HeavyDamageThreshold = 0.75;
            playerCar.CarDamageInformation.Engine.MediumDamageThreshold = 0.75;
            playerCar.CarDamageInformation.Transmission.MediumDamageThreshold = 0.9;
            /*playerCar.CarDamageInformation.Suspension.Damage = rawCarStatusData.MTyresDamage.Max(x => x) / 100.0;*/
            playerCar.CarDamageInformation.Bodywork.Damage = Math.Max(rawCarStatusData.MRearWingDamage, Math.Max(rawCarStatusData.MFrontLeftWingDamage, rawCarStatusData.MFrontRightWingDamage)) / 100.0;
            playerCar.CarDamageInformation.Engine.Damage = rawCarStatusData.MEngineDamage / 100.0;
            playerCar.CarDamageInformation.Transmission.Damage = rawCarStatusData.MGearBoxDamage / 100.0;

            playerCar.SpeedLimiterEngaged = rawCarStatusData.MPitLimiterStatus == 1;

            /*playerCar.WorldOrientation = new Orientation()
            {
                Roll = Angle.GetFromRadians(rawCarStatusData.CarOrientation.Roll),
                Pitch = Angle.GetFromRadians(rawCarStatusData.CarOrientation.Pitch),
                Yaw = Angle.GetFromRadians(-rawCarStatusData.CarOrientation.Yaw),
            };*/

            FillDrsData(ref rawCarStatusData, ref rawCarTelemetryData, playerCar);
            FillBoostData(ref rawCarStatusData, playerCar);
        }

        private static void FillDrsData(ref CarStatusData carStatusData, ref CarTelemetryData rawCarTelemetryData, CarInfo playerCar)
        {
            DrsSystem drsSystem = playerCar.DrsSystem;

            drsSystem.DrsActivationLeft = -1;
            if (rawCarTelemetryData.MDrs == 1)
            {
                drsSystem.DrsStatus = DrsStatus.InUse;
                return;
            }

            switch ((int) carStatusData.MDrsAllowed)
            {
                case -1:
                case 0:
                    drsSystem.DrsStatus = DrsStatus.NotEquipped;
                    return;
                case 1:
                    drsSystem.DrsStatus = DrsStatus.Available;
                    break;
            }
        }

        private static void FillBoostData(ref CarStatusData rawCarStatusData, CarInfo playerCar)
        {
            BoostSystem boostSystem = playerCar.BoostSystem;
            boostSystem.ActivationsRemaining = (int)rawCarStatusData.MErsStoreEnergy / 100000;
            if (rawCarStatusData.MErsDeployMode <= 1)
            {
                boostSystem.BoostStatus = BoostStatus.Available;
            }else if (rawCarStatusData.MErsDeployMode <= 3)
            {
                boostSystem.BoostStatus = BoostStatus.InUse;
            }
            else
            {
                boostSystem.BoostStatus = BoostStatus.Cooldown;
            }
        }

        private void AddFlags(ref PacketSessionData data, SimulatorDataSet simData)
        {
            if (data.MSafetyCarStatus == 2)
            {
                simData.SessionInfo.ActiveFlags.Add(FlagKind.VirtualSafetyCar);
                return;
            }

            if (data.MSafetyCarStatus == 1)
            {
                simData.SessionInfo.ActiveFlags.Add(FlagKind.SafetyCar);
                return;
            }

            foreach (var zoneWithFlag in data.MMarshalZones.Take(data.MNumMarshalZones).Where(x => x.MZoneFlag == 3))
            {
                if (zoneWithFlag.MZoneStart < 0.33)
                {
                    simData.SessionInfo.ActiveFlags.Add(FlagKind.YellowSector1);
                    continue;
                }

                if (zoneWithFlag.MZoneStart < 0.66)
                {
                    simData.SessionInfo.ActiveFlags.Add(FlagKind.YellowSector2);
                    continue;
                }

                simData.SessionInfo.ActiveFlags.Add(FlagKind.YellowSector3);
            }
        }

        private void FillPlayerCarInfo(ref CarTelemetryData rawTelemetryData, SimulatorDataSet simData)
        {
            simData.PlayerInfo.CarInfo.EngineRpm = rawTelemetryData.MEngineRpm;
            switch (rawTelemetryData.MGear)
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
                    simData.PlayerInfo.CarInfo.CurrentGear = rawTelemetryData.MGear.ToString();
                    break;
            }
        }

        private void FillSessionInfo(AllPacketsComposition data, SimulatorDataSet simData)
        {
            // Timing
            simData.SessionInfo.SessionTime = TimeSpan.FromSeconds(data.PacketCarTelemetryData.MHeader.MSessionTime);
            _layoutLength = data.PacketSessionData.MTrackLength;
            _playerDataIndex = data.PacketSessionData.MHeader.MPlayerCarIndex;
            simData.SessionInfo.TrackInfo.LayoutLength = Distance.FromMeters(_layoutLength);
            simData.SessionInfo.IsActive = true;
            simData.SessionInfo.WeatherInfo.AirTemperature = Temperature.FromCelsius(data.PacketSessionData.MAirTemperature);
            simData.SessionInfo.WeatherInfo.TrackTemperature = Temperature.FromCelsius(data.PacketSessionData.MTrackTemperature);
            if (data.PacketSessionData.MWeather == 3)
            {
                simData.SessionInfo.WeatherInfo.RainIntensity = 30;
            }else if (data.PacketSessionData.MWeather == 4)
            {
                simData.SessionInfo.WeatherInfo.RainIntensity = 60;
            }
            else if (data.PacketSessionData.MWeather == 5)
            {
                simData.SessionInfo.WeatherInfo.RainIntensity = 100;
            }


            switch ((SessionKind)data.PacketSessionData.MSessionType)
            {
                case SessionKind.TimeTrial:
                case SessionKind.Practice1:
                case SessionKind.Practice2:
                case SessionKind.Practice3:
                case SessionKind.ShortPractice:
                    simData.SessionInfo.SessionType = SessionType.Practice;
                    break;
                case SessionKind.Qualification1:
                case SessionKind.Qualification2:
                case SessionKind.Qualification3:
                case SessionKind.ShortQualification:
                case SessionKind.OnlineQualification:
                    simData.SessionInfo.SessionType = SessionType.Qualification;
                    break;
                case SessionKind.Race:
                case SessionKind.Race2:
                    simData.SessionInfo.SessionType = SessionType.Race;
                    break;
                default:
                    simData.SessionInfo.SessionType = SessionType.Na;
                    break;
            }

            simData.SessionInfo.SessionPhase = SessionPhase.Green;

            simData.SessionInfo.TrackInfo.TrackName = TranslationTable.GetTrackName(data.PacketSessionData.MTrackId);
            simData.SessionInfo.TrackInfo.TrackLayoutName = string.Empty;

            if (data.PacketSessionData.MTotalLaps > 2 && simData.SessionInfo.SessionType == SessionType.Race)
            {
                simData.SessionInfo.SessionLengthType = SessionLengthType.Laps;
                simData.SessionInfo.TotalNumberOfLaps = data.PacketSessionData.MTotalLaps;
            }
            else
            {
                simData.SessionInfo.SessionLengthType = SessionLengthType.Time;
                simData.SessionInfo.SessionTimeRemaining = data.PacketSessionData.MSessionTimeLeft;
            }
        }
    }
}
