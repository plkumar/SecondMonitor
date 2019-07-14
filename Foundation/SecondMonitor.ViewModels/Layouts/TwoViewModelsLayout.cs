namespace SecondMonitor.ViewModels.Layouts
{
    public class TwoViewModelsLayout : AbstractViewModel
    {
        public IViewModel FirstViewModel { get; set; }
        public IViewModel SecondViewModel { get; set; }
    }
}