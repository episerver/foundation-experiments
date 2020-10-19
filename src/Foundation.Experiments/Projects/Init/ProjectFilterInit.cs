using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Foundation.Experiments.Core.Init;
using Foundation.Experiments.Projects.Impl;
using Foundation.Experiments.Projects.Interfaces;

namespace Foundation.Experiments.Projects.Init
{
    [ModuleDependency(typeof(ConfigurationModule))]
    public class ProjectFilterInit : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var providers = FilterProviders.Providers.ToList();
            FilterProviders.Providers.Clear();
            var filter = context.Locate.Advanced.GetInstance<ProjectFilterProvider>();
            filter.Init(providers);

            FilterProviders.Providers.Add(filter);

            context.Locate.Advanced.GetInstance<IContentRouteEvents>()
                .RoutedContent += ContentRoute_RoutedContent;
        }

        public void Uninitialize(InitializationEngine context) { }

        public void ConfigureContainer(ServiceConfigurationContext context) { }

        private void ContentRoute_RoutedContent(object sender, RoutingEventArgs e)
        {
            var experimentProjectIdentifier = ServiceLocator.Current.GetInstance<IExperimentProjectIdentifier>();
            var d = experimentProjectIdentifier.GetProjectVersion(e.RoutingSegmentContext.RoutedContentLink, new HttpContextWrapper(HttpContext.Current));
            e.RoutingSegmentContext.RoutedContentLink = d;
        }
    }
}

