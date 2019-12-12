namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using System.Linq;
    using Common.DataModel.Championship.Events;
    using SecondMonitor.ViewModels;

    public class PodiumViewModel : AbstractViewModel<SessionResultDto>
    {
        public string First { get; private set; }
        public string Second { get; private set; }
        public string Third { get; private set; }

        protected override void ApplyModel(SessionResultDto result)
        {
            if (result == null)
            {
                return;
            }

            First = result.DriverSessionResult.First(x => x.FinishPosition == 1).DriverName;
            Second = result.DriverSessionResult.First(x => x.FinishPosition == 2).DriverName;
            Third = result.DriverSessionResult.First(x => x.FinishPosition == 3).DriverName;
        }

        public override SessionResultDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}