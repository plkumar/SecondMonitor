namespace SecondMonitor.Rating.Application.Championship.Pool
{
    using System;
    using Common.DataModel.Championship;

    public class ChampionshipEventArgs : EventArgs
    {
        public ChampionshipEventArgs(ChampionshipDto championshipDto)
        {
            ChampionshipDto = championshipDto;
        }

        public ChampionshipDto ChampionshipDto { get; }
    }
}