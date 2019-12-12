namespace SecondMonitor.ViewModels.Controllers
{
    public interface IChildControllerFactory
    {
        T Create<T, TParent>(TParent parentInstance) where T : IChildController<TParent> where TParent : IController;
    }
}