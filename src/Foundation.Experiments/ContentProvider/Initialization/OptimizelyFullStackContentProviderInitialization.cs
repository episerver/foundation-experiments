using EPiServer.Configuration;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using System.Collections.Specialized;

namespace Optimizely.DeveloperFullStack.Initialization
{
    [InitializableModule, ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class OptimizelyFullStackContentProviderInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var optimizelyRoot = OptimizelyFullStackContentProvider.GetEntryPoint();
            var contentProviderManager = context.Locate.Advanced.GetInstance<IContentProviderManager>();
            var configValues = new NameValueCollection { { ContentProviderElement.EntryPointString, optimizelyRoot.ContentLink.ToString() } };
            var provider = context.Locate.Advanced.GetInstance<OptimizelyFullStackContentProvider>();
            provider.Initialize(FullStackConstants.RepositoryKey, configValues);
            contentProviderManager.ProviderMap.AddProvider(provider);
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}