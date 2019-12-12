namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using Common.DataModel.Championship.Events;
    using SecondMonitor.ViewModels;

    public class DriverFinishViewModel : AbstractViewModel<DriverSessionResultDto>
    {
        public string DriverName { get; private set; }
        public int FinishPosition { get; private set; }
        public int PointsGain { get; private set; }
        public bool IsPlayer { get; private set; }

        protected override void ApplyModel(DriverSessionResultDto model)
        {
            DriverName = model.DriverName;
            FinishPosition = model.FinishPosition;
            PointsGain = model.PointsGain;
            IsPlayer = model.IsPlayer;
        }

        public override DriverSessionResultDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}