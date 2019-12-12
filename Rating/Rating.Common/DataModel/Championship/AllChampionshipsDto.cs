namespace SecondMonitor.Rating.Common.DataModel.Championship
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class AllChampionshipsDto
    {
        public AllChampionshipsDto()
        {
            Championships = new List<ChampionshipDto>();
        }

        public List<ChampionshipDto> Championships { get; set; }
    }
}