﻿using SecondMonitor.Timing.Model.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondMonitor.Timing.Model
{
    public class LapInfo
    {
        private DriverTiming driver;
        private TimeSpan lapEnd;
        private TimeSpan lapProgressTime;
        private TimeSpan lapTime;
        
        public LapInfo(TimeSpan startSeesionTine, int lapNumber, DriverTiming driver)
        {
            Driver = driver;
            LapStart = startSeesionTine;
            lapProgressTime = new TimeSpan(0, 0, 0);
            LapNumber = lapNumber;
            Valid = true;
            FirstLap = false;
            PitLap = false;
        }

        public LapInfo(TimeSpan startSeesionTine, int lapNumber, DriverTiming driver, bool firstLap)
        {
            Driver = driver;
            LapStart = startSeesionTine;
            lapProgressTime = new TimeSpan(0, 0, 0);
            LapNumber = lapNumber;
            Valid = true;
            FirstLap = firstLap;
            PitLap = false;
        }
        public TimeSpan LapStart { get; private set; }
        public int LapNumber { get; private set; }
        public bool Valid { get; set; }
        public DriverTiming Driver { get => driver; private set => driver = value; } 
        public bool FirstLap { get; private set; }
        public bool InvalidBySim { get; set; }
        public bool PitLap { get; set; }

        public void FinishLap(TimeSpan sessionTime, Single modelLapTime)
        {
            if (modelLapTime == -1)
            {
                lapEnd = sessionTime;
            }
            else
            {
                lapEnd = LapStart.Add(TimeSpan.FromSeconds(modelLapTime));
            }
            lapTime = LapEnd.Subtract(LapStart);
        }
        public void Tick(TimeSpan sessionTime)
        {
            lapProgressTime = sessionTime.Subtract(LapStart);
        }

        public TimeSpan LapEnd { get => lapEnd; }
        public TimeSpan LapTime { get => lapTime; }
        public TimeSpan LapProgressTime { get => lapProgressTime; }
    }
}
