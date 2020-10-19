using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Experiments.Core.Init;
using Foundation.Experiments.Projects.Impl;
using Foundation.Experiments.Projects.Interfaces;

namespace Foundation.Experiments.Projects.Init
{
    [ModuleDependency(typeof(ConfigurationModule))]
    public class ProjectDependencyInit : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.ConfigurationComplete += Context_ConfigurationComplete;
        }

        private void Context_ConfigurationComplete(object sender, ServiceConfigurationEventArgs e)
        {
            var services = e.Services;
            services.AddTransient<IProjectIdResolver, DefaultProjectIdResolver>();
            services.AddTransient<IExperimentProjectIdentifier, ExperimentProjectIdentifier>();
        }

        public void Initialize(InitializationEngine context) { }

        public void Uninitialize(InitializationEngine context) { }
    }
}