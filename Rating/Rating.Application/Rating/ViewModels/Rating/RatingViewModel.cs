namespace SecondMonitor.Rating.Application.Rating.ViewModels.Rating
{
    using Common.DataModel.Player;
    using SecondMonitor.ViewModels;

    public class RatingViewModel : AbstractViewModel<DriversRating>, IRatingViewModel
    {
        private string _secondaryRating;
        private int _ratingChange;
        private string _mainRating;
        private bool _ratingChangeVisible;
        private int _difficulty;
        private int _rating;

        public string SecondaryRating
        {
            get => _secondaryRating;
            set => SetProperty(ref _secondaryRating, value);
        }

        public int RatingChange
        {
            get => _ratingChange;
            set => SetProperty(ref _ratingChange, value);
        }

        public bool RatingChangeVisible
        {
            get => _ratingChangeVisible;
            set => SetProperty(ref _ratingChangeVisible, value);
        }

        public int Difficulty
        {
            get => _difficulty;
            set
            {
                SetProperty(ref _difficulty, value);
                RefreshMainRating();
            }
        }

        private void RefreshMainRating()
        {
            MainRating = $"{_rating}-{_difficulty}";
        }

        public string MainRating
        {
            get => _mainRating;
            set => SetProperty(ref _mainRating, value);
        }

        protected override void ApplyModel(DriversRating model)
        {
            _rating = model.Rating;
            _difficulty = model.Difficulty;
            RefreshMainRating();
            SecondaryRating = $"{model.Deviation}";
        }

        public override DriversRating SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}