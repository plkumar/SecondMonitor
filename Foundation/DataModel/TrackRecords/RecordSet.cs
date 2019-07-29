namespace SecondMonitor.DataModel.TrackRecords
{
    using System.Collections.Generic;
    using System.Linq;
    using BasicProperties;

    public class RecordSet
    {
        public RecordEntryDto BestPracticeRecord { get; set; }

        public RecordEntryDto BestQualiRecord { get; set; }

        public RecordEntryDto BestRaceRecord { get; set; }

        public RecordEntryDto GetOverAllBest()
        {
            return GetAllRecords().OrderBy(x => x.LapTimeSeconds).FirstOrDefault();
        }

        public RecordEntryDto GetProperEntry(SessionType sessionType)
        {
            switch (sessionType)
            {
                case SessionType.Practice:
                    return BestPracticeRecord;
                case SessionType.Qualification:
                    return BestQualiRecord;
                case SessionType.WarmUp:
                    return BestPracticeRecord;
                case SessionType.Race:
                    return BestRaceRecord;
                default:
                    return null;
            }
        }

        public void SetProperEntry(SessionType sessionType, RecordEntryDto recordEntry)
        {
            switch (sessionType)
            {
                case SessionType.Practice:
                    BestPracticeRecord = recordEntry;
                    return;
                case SessionType.Qualification:
                    BestQualiRecord = recordEntry;
                    return;
                case SessionType.WarmUp:
                    BestPracticeRecord = recordEntry;
                    return;
                case SessionType.Race:
                    BestRaceRecord = recordEntry;
                    return;
                default:
                    return;
            }
        }

        protected IEnumerable<RecordEntryDto> GetAllRecords()
        {
            if (BestPracticeRecord != null)
            {
                yield return BestPracticeRecord;
            }

            if (BestQualiRecord != null)
            {
                yield return BestQualiRecord;
            }

            if (BestRaceRecord != null)
            {
                yield return BestRaceRecord;
            }
        }
    }
}
