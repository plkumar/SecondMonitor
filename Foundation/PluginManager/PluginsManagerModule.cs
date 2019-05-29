namespace SecondMonitor.PluginManager
{
    using System;
    using DataModel.Snapshot;
    using DataModel.Visitors;
    using Ninject.Modules;

    public class PluginsManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISimulatorDataSetVisitor>().To<ComputeGapToPlayerVisitor>().WithConstructorArgument("informationValiditySpan", TimeSpan.FromMilliseconds(300));
            Bind<ISimulatorDataSetVisitor>().To<ComputeSuspensionVelocityVisitor>();
        }
    }
}