namespace SecondMonitor.Rating.Application.Championship
{
    using Common.DataModel.Championship;
    using DataModel.Snapshot;

    public interface IChampionshipDialogProvider
    {
        void ShowWelcomeScreen(SimulatorDataSet dataSet, ChampionshipDto championship);
        void ShowLastEvenResultWindow(ChampionshipDto championship);
    }
}