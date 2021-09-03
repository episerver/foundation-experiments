using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Optimizely.DeveloperFullStack.Core;

namespace Optimizely.DeveloperFullStack.Initialization
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ConfigurationModule : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.ConfigurationComplete += (o, e) =>
            {
                context.Services
                    .AddTransient<IOptimizelyFullStackContentLoader, OptimizelyFullStackContentLoader>()
                    .AddSingleton<IOptimizelyFullStackRepository, OptimizelyFullStackRepository>();
            };
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}