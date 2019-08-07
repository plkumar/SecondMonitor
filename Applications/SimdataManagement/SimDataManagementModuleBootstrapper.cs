namespace SecondMonitor.SimdataManagement
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts.NInject;
    using Ninject.Modules;

    public class SimDataManagementModuleBootstrapper : INinjectModuleBootstrapper
    {
        public IList<INinjectModule> GetModules()
        {
            return new INinjectModule[] {new SimDataManagementModule(), }.ToList();
        }
    }
}