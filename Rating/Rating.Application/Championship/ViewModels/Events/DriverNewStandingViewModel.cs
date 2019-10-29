namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using Common.DataModel.Championship.Events;
    using SecondMonitor.ViewModels;

    public class DriverNewStandingViewModel : AbstractViewModel<DriverSessionResultDto>
    {
        public int Position { get; private set; }
        public string Name { get; private set; }
        public int TotalPoints { get; private set; }
        public int PositionsGained { get; private set; }
        public bool IsPlayer { get; private set; }
        public int GapToPrevious { get; set; }
        public int GapToLeader { get; set; }

        protected override void ApplyModel(DriverSessionResultDto model)
        {
            Position = model.AfterEventPosition;
            Name = model.DriverName;
            TotalPoints = model.TotalPoints;
            PositionsGained = model.PositionGained;
            IsPlayer = model.IsPlayer;
        }

        public override DriverSessionResultDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}