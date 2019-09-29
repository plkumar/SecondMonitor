namespace SecondMonitor.ViewModels.Controllers
{
    using Ninject;
    using Ninject.Syntax;

    public class ChildControllerFactory : IChildControllerFactory
    {
        private readonly IResolutionRoot _resolutionRoot;

        public ChildControllerFactory(IResolutionRoot resolutionRoot)
        {
            _resolutionRoot = resolutionRoot;
        }

        public T Create<T, TParent>(TParent parentInstance) where T : IChildController<TParent> where TParent : IController
        {
            var childController = _resolutionRoot.Get<T>();
            childController.ParentController = parentInstance;
            return childController;
        }
    }
}