namespace SecondMonitor.ViewModels.Track
{
    using System.Windows.Media;
    using DataModel.TrackMap;

    public class TrackGeometryViewModel : AbstractViewModel<TrackGeometryDto>
    {
        private double _width;
        private double _height;
        private double _leftOffset;
        private double _topOffset;
        private Geometry _trackGeometry;
        private bool _isGeometryFilled;

        public double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        public double LeftOffset
        {
            get => _leftOffset;
            set => SetProperty(ref _leftOffset, value);
        }

        public double TopOffset
        {
            get => _topOffset;
            set => SetProperty(ref _topOffset, value);
        }

        public Geometry TrackGeometry
        {
            get => _trackGeometry;
            set => SetProperty(ref _trackGeometry, value);
        }

        public bool IsGeometryFilled
        {
            get => _isGeometryFilled;
            set => SetProperty(ref _isGeometryFilled, value);
        }

        protected override void ApplyModel(TrackGeometryDto model)
        {
            Width = model.Width;
            Height = model.Height;
            LeftOffset = -model.LeftOffset;
            TopOffset = -model.TopOffset;
            TrackGeometry = Geometry.Parse(model.FullMapGeometry);
            IsGeometryFilled = true;
        }

        public override TrackGeometryDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}