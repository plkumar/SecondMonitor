﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using SecondMonitor.Contracts.NInject;
using SecondMonitor.DataModel.Snapshot;
using SecondMonitor.PluginManager.Core;
using SecondMonitor.PluginManager.GameConnector;
using SecondMonitor.PluginsConfiguration.Common.Controller;

namespace SecondMonitorLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly string ConnectorsDir = "Connectors";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoadAssemblies();
            LoadUsingGameConnectorsFromDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConnectorsDir));
        }

        private static void LoadAssemblies()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
            Parallel.ForEach(toLoad, path => loadedAssemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(path)));
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogManager.GetCurrentClassLogger().Error("Application experienced an unhandled excpetion");
            LogManager.GetCurrentClassLogger().Error(e.ExceptionObject);
        }

        private static void LoadUsingGameConnectorsFromDirectory(string connectorsDir)
        {
            IEnumerable<string> files = Directory.EnumerateFiles(connectorsDir, "*.dll", SearchOption.AllDirectories);
            var connectors = new List<IGameConnector>();
            foreach (var file in files)
            {
                string assemblyPath = file;
                connectors.AddRange(GetConnectorsFromAssembly(assemblyPath));

            }

            if (connectors.Count == 0)
            {
                MessageBox.Show("No connectors loaded. Please place connectors .dll into " + connectorsDir, "No connectors", System.Windows.MessageBoxButton.OK);
                Environment.Exit(1);
            }

            ConnectAndLoadPlugins(connectors.ToArray<IGameConnector>());
        }

        public static ICollection<IGameConnector> GetConnectorsFromAssembly(string assemblyPath)
        {
            var connectorType = typeof(IGameConnector);
            Assembly assembly;
            try
            {
                assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);

            }
            catch (Exception)
            {
                return new List<IGameConnector>();
            }

            IEnumerable<Type> types = assembly.GetTypes().Where(c => !c.IsInterface && connectorType.IsAssignableFrom(c));
            return types.Select(type => Activator.CreateInstance(type) as IGameConnector).ToList();
        }

        private static void ConnectAndLoadPlugins(IGameConnector[] connectors)
        {
            KernelWrapper kernelWrapper = new KernelWrapper();
            IPluginSettingsProvider pluginSettingsProvider = kernelWrapper.Get<IPluginSettingsProvider>();
            IEnumerable<ISimulatorDataSetVisitor> dataVisitors = kernelWrapper.GetAll<ISimulatorDataSetVisitor>();
            PluginsManager pluginManager = new PluginsManager(pluginSettingsProvider, connectors, dataVisitors);
            pluginManager.InitializePlugins();
            pluginManager.Start();
        }
    }
}
