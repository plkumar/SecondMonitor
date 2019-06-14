namespace SecondMonitor.Rating.Common.DataModel
{
    public class DriverFinishState
    {
        public DriverFinishState(bool isPlayer, string name, string carName, string carClass, int finishPosition)
        {
            IsPlayer = isPlayer;
            Name = name;
            CarName = carName;
            CarClass = carClass;
            FinishPosition = finishPosition;
        }

        public bool IsPlayer { get;  }
        public string Name { get;  }
        public string CarClass { get; }

        public string CarName { get; }
        public int FinishPosition { get;  }
    }
}