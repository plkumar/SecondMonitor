namespace SecondMonitor.ViewModels.Controllers
{
    using System.Collections.Generic;
    using Ninject;
    using Ninject.Syntax;

    public class ChildControllerFactory : IChildControllerFactory
    {
        private readonly IResolutionRoot _resolutionRoot;

        public ChildControllerFactory(IResolutionRoot resolutionRoot)
        {
            _resolutionRoot = resolutionRoot;
        }

        public T Create<T>() where T : IController
        {
            return _resolutionRoot.Get<T>();
        }

        public IEnumerable<T> CreateAll<T>() where T : IController
        {
            return _resolutionRoot.GetAll<T>();
        }
    }
}