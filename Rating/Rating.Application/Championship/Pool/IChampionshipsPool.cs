namespace SecondMonitor.Rating.Application.Championship.Pool
{
    using System.Collections.Generic;
    using Common.DataModel.Championship;

    public interface IChampionshipsPool
    {
        IReadOnlyCollection<ChampionshipDto> GetAllChampionshipDtos();
        IReadOnlyCollection<ChampionshipDto> GetAllChampionshipDtos(string simulatorName);
        void AddNewChampionship(ChampionshipDto championshipDto);
    }
}