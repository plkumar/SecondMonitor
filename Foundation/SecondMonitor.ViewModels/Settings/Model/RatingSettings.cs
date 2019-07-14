namespace SecondMonitor.ViewModels.Settings.Model
{
    public class RatingSettings
    {
        public bool IsEnabled { get; set; } = false;

        public string SelectedReferenceRatingProvider { get; set; } = "Leading Group";

        public int GraceLapsCount { get; set; } = 1;
    }
}