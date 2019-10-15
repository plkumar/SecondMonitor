namespace SecondMonitor.Rating.Application.Championship.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.Events;

    public class DriverSessionResultComparer : IComparer<DriverSessionResultDto>
    {
        private readonly List<DriverSessionResultDto> _results;

        public DriverSessionResultComparer(ChampionshipDto championshipDto)
        {
            _results = championshipDto.GetAllResults().SelectMany(x => x.DriverSessionResult).ToList();
        }

        public int Compare(DriverSessionResultDto x, DriverSessionResultDto y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null || y == null)
            {
                return -1;
            }

            if (x.TotalPoints != y.TotalPoints)
            {
                //inverse comparison - more position occurrences means that the driver is "lower"
                return y.TotalPoints.CompareTo(x.TotalPoints);
            }

            for (int i = 1; i < 10; i++)
            {
                int driverXPositionCount = GetPositionsCount(i, x.DriverGuid);
                int driverYPositionCount = GetPositionsCount(i, y.DriverGuid);
                if (driverXPositionCount != driverYPositionCount)
                {
                    //inverse comparison - more position occurrences means that the driver is "lower"
                    return driverYPositionCount.CompareTo(driverXPositionCount);
                }
            }

            return 0;
        }

        private int GetPositionsCount(int position, Guid guid )
        {
            return _results.Count(x => x.FinishPosition == position && x.DriverGuid == guid);
        }
    }
}