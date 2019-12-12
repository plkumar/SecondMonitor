namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Session
{
    using SecondMonitor.ViewModels.Factory;

    public class SessionDefinitionViewModelFactory : ISessionDefinitionViewModelFactory
    {
        private readonly IViewModelFactory _viewModelFactory;

        public SessionDefinitionViewModelFactory(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public SessionDefinitionViewModel CreateBase()
        {
            return new SessionDefinitionViewModel(_viewModelFactory)
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