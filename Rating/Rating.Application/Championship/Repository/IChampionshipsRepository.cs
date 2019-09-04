namespace SecondMonitor.Rating.Application.Championship.Repository
{
    using Common.DataModel.Championship;

    public interface IChampionshipsRepository
    {
        AllChampionshipsDto LoadRatingsOrCreateNew();
        void Save(AllChampionshipsDto allChampionshipsDto);
    }
}