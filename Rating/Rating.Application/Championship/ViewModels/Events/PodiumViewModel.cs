namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using System.Linq;
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels;

    public class PodiumViewModel : AbstractViewModel<ChampionshipDto>
    {
        public string First { get; private set; }
        public string Second { get; private set; }
        public string Third { get; private set; }

        protected override void ApplyModel(ChampionshipDto model)
        {
            var result = model.GetAllResults().LastOrDefault();
            if (result == null)
            {
                return;
            }

            First = result.DriverSessionResult.First(x => x.FinishPosition == 1).DriverName;
            Second = result.DriverSessionResult.First(x => x.FinishPosition == 2).DriverName;
            Third = result.DriverSessionResult.First(x => x.FinishPosition == 3).DriverName;
        }

        public override ChampionshipDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}