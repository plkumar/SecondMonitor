namespace SecondMonitor.ViewModels.Controllers
{
    using System.Threading.Tasks;

    public abstract class AbstractChildController<TParent>  : IChildController<TParent> where TParent : IController
    {
        public TParent ParentController { get; set; }

        public abstract Task StartControllerAsync();

        public abstract Task StopControllerAsync();
    }
}