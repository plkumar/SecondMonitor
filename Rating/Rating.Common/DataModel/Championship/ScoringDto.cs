namespace SecondMonitor.Rating.Common.DataModel.Championship
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class ScoringDto
    {
        public ScoringDto()
        {
            Scoring = new List<int>();
        }

        public List<int> Scoring { get; set; }
    }
}