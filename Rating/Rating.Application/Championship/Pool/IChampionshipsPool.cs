namespace SecondMonitor.Rating.Application.Championship.Pool
{
    using System;
    using System.Collections.Generic;
    using Common.DataModel.Championship;

    public interface IChampionshipsPool
    {
        event EventHandler<ChampionshipEventArgs> ChampionshipAdded;
        event EventHandler<ChampionshipEventArgs> ChampionshipRemoved;

        IReadOnlyCollection<ChampionshipDto> GetAllChampionshipDtos();
        IReadOnlyCollection<ChampionshipDto> GetAllChampionshipDtos(string simulatorName);
        void AddNewChampionship(ChampionshipDto championshipDto);
        void RemoveChampionship(ChampionshipDto championshipDto);
    }
}