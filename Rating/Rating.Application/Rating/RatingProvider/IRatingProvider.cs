namespace SecondMonitor.Rating.Application.Rating.RatingProvider
{
    using Common.DataModel.Player;

    public interface IRatingProvider
    {
        bool  TryGetRatingForDriverCurrentSession(string driverName, out DriversRating driversRating);
    }
}