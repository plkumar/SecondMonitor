namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Session
{
    public class SessionDefinitionViewModelFactory : ISessionDefinitionViewModelFactory
    {
        public SessionDefinitionViewModel CreateBase()
        {
            return new SessionDefinitionViewModel()
            {
                Pos1Points = 25,
                Pos2Points = 18,
                Pos3Points = 15,
                Pos4Points = 12,
                Pos5Points = 10,
                Pos6Points = 8,
                Pos7Points = 6,
                Pos8Points = 4,
                Pos9Points = 2,
                Pos10Points = 1,
            };
        }
    }
}