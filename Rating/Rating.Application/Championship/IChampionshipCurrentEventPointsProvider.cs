namespace SecondMonitor.Rating.Application.Championship
{
    public interface IChampionshipCurrentEventPointsProvider
    {
        bool TryGetPointsForDriver(string driverName, out int points);
    }
}