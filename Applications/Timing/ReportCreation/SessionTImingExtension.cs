namespace SecondMonitor.Timing.ReportCreation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.Summary;
    using DataModel.BasicProperties;
    using DataModel.Extensions;
    using DataModel.Snapshot.Drivers;
    using SecondMonitor.Timing.SessionTiming.Drivers.ViewModel;
    using SecondMonitor.Timing.SessionTiming.ViewModel;

    public static class SessionTimingExtension
    {
        public static SessionSummary ToSessionSummary(this SessionTiming timing)
        {
            SessionSummary summary = new SessionSummary();
            FillSessionInfo(summary, timing);
            AddDrivers(summary, timing);
            return summary;
        }

        private static void FillSessionInfo(SessionSummary summary, SessionTiming timing)
        {
            summary.SessionType = timing.SessionType;
            summary.TrackInfo = timing.LastSet.SessionInfo.TrackInfo;
            summary.Simulator = timing.LastSet.Source;
            summary.SessionLength = TimeSpan.FromSeconds(timing.TotalSessionLength);
            summary.SessionLengthType = timing.LastSet.SessionInfo.SessionLengthType;
            summary.TotalNumberOfLaps = timing.LastSet.SessionInfo.TotalNumberOfLaps;
            summary.SessionRunDuration = timing.SessionTime;
            summary.WasGreen = timing.WasGreen;
            summary.IsMultiClass = timing.IsMultiClass;
        }

        private static void AddDrivers(SessionSummary summary, SessionTiming timing)
        {
            summary.Drivers.AddRange(timing.Drivers.Select(d => ConvertToSummaryDriver(d.Value, timing.SessionType, timing.Player)));
            FillRaceGapsInfo(summary.Drivers, summary);
        }

        private static Driver ConvertToSummaryDriver(DriverTiming driverTiming, SessionType sessionType, DriverTiming playerTiming)
        {
            Driver driverSummary = new Driver()
                                       {
                                           DriverName = driverTiming.Name,
                                           CarName = driverTiming.CarName,
                                           Finished = driverTiming.IsActive,
                                           FinishingPosition = driverTiming.Position,
                                           TopSpeed = driverTiming.TopSpeed,
                                           IsPlayer = driverTiming.DriverInfo.IsPlayer,
                                           FinishStatus = driverTiming.DriverInfo.FinishStatus,
                                           ClassName = driverTiming.CarClassName,
                                           ClassId = driverTiming.CarClassId,
                                           TotalDistance = driverTiming.TotalDistanceTraveled
                                       };
            int lapNumber = 1;
            bool allLaps = sessionType == SessionType.Race;
            driverSummary.Laps.AddRange(driverTiming.Laps.Where(l => l.LapTelemetryInfo != null && l.Completed && (allLaps || l.Valid)).Select(l => ConvertToSummaryLap(driverSummary, l, lapNumber++, sessionType)));
            driverSummary.TotalLaps = driverSummary.Laps.Count;
            FillGapInfo(driverSummary, driverTiming, playerTiming, sessionType);
            return driverSummary;
        }

        private static void FillGapInfo(Driver driverToFill, DriverTiming driverToFillTiming, DriverTiming playerTiming, SessionType sessionType)
        {
            if (playerTiming == null)
            {
                return;
            }

            if (sessionType == SessionType.Race)
            {
                driverToFill.LapsDifferenceToPlayer = (int) ((driverToFillTiming.TotalDistanceTraveled - playerTiming.TotalDistanceTraveled) / driverToFillTiming.Session.LastSet.SessionInfo.TrackInfo.LayoutLength.InMeters);
            }
            else if(playerTiming.BestLap != null && driverToFillTiming.BestLap != null)
            {
                driverToFill.GapToPlayerRelative = driverToFillTiming.BestLap.LapTime - playerTiming.BestLap.LapTime;
            }
        }

        private static void FillRaceGapsInfo(List<Driver> driversToFill, SessionSummary sessionSummary)
        {
            var player = driversToFill.FirstOrDefault(x => x.IsPlayer);
            if (player == null || sessionSummary.SessionType != SessionType.Race)
            {
                return;
            }

            driversToFill.Where(x => !x.IsPlayer && x.FinishStatus != DriverFinishStatus.Dnf).ForEach(x => FillRaceGapsInfo(x, player));

        }

        private static void FillRaceGapsInfo(Driver driverToFill, Driver player)
        {
            int lapsCount = Math.Min(driverToFill.Laps.Count, player.Laps.Count);
            double totalTimeDriver = driverToFill.Laps.Take(lapsCount).Sum(x => x.LapTime.TotalSeconds);
            double totalTimePlayer = driverToFill.Laps.Take(lapsCount).Sum(x => x.LapTime.TotalSeconds);
            driverToFill.GapToPlayerRelative = TimeSpan.FromSeconds(totalTimeDriver - totalTimePlayer);
        }

        private static Lap ConvertToSummaryLap(Driver summaryDriver,  ILapInfo lapInfo, int lapNumber, SessionType sessionType)
        {
            Lap summaryLap = new Lap(summaryDriver, lapInfo.Valid)
                                 {
                                     LapNumber = lapNumber,
                                     LapTime = lapInfo.LapTime,
                                     Sector1 = lapInfo.Sector1?.Duration ?? TimeSpan.Zero,
                                     Sector2 = lapInfo.Sector2?.Duration ?? TimeSpan.Zero,
                                     Sector3 = lapInfo.Sector3?.Duration ?? TimeSpan.Zero,
                                     LapEndSnapshot = lapInfo.LapTelemetryInfo.LapEndSnapshot,
                                     LapStartSnapshot = lapInfo.LapTelemetryInfo.LapStarSnapshot,
                                     SessionType = sessionType,
            };
            return summaryLap;
        }
    }
}