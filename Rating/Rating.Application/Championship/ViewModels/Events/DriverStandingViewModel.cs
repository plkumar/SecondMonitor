namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels;

    public class DriverStandingViewModel : AbstractViewModel<DriverDto>
    {
        public string DriverName { get; private set; }
        public string CarName { get; private set; }
        public int TotalPoints { get; set; }
        public int Position { get; private set; }
        public bool IsPlayer { get; private set; }


        protected override void ApplyModel(DriverDto model)
        {
            DriverName = model.LastUsedName;
            CarName = model.LastCarName;
            TotalPoints = model.TotalPoints;
            Position = model.Position;
            IsPlayer = model.IsPlayer;
        }

        public override DriverDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}