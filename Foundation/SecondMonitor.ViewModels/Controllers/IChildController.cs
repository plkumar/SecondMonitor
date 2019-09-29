namespace SecondMonitor.ViewModels.Controllers
{
    public interface IChildController<TParent> : IController where TParent : IController
    {
        TParent ParentController { get; set; }
    }
}