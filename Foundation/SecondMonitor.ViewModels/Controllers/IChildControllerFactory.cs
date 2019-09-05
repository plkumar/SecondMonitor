namespace SecondMonitor.ViewModels.Controllers
{
    using System.Collections.Generic;

    public interface IChildControllerFactory
    {
        T Create<T>() where T : IController;

        IEnumerable<T> CreateAll<T>() where T : IController;
    }
}