using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Experiments.Core.Config;
using Foundation.Experiments.Core.Impl.Models;
using Foundation.Experiments.Rest;

namespace Foundation.Experiments.Core.Init
{
    [ModuleDependency(typeof(ConfigurationModule))]
    public class InitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            SyncAudienceAttributesToOptimizely();
            SyncEventsToOptimizely();
        }
        
        private void SyncAudienceAttributesToOptimizely()
        {
            var options = ServiceLocator.Current.GetInstance<ExperimentationOptions>();
            if (!options.SyncDefaultAudienceAttributesToOptimizely)
                return;

            var client = ServiceLocator.Current.GetInstance<IExperimentationClient>();
            client.CreateOrUpdateAttribute(DefaultKeys.VisitorGroup, "Matches the visitor against Episerver visitor groups");
        }

        private void SyncEventsToOptimizely()
        {
            var options = ServiceLocator.Current.GetInstance<ExperimentationOptions>();
            if (!options.SyncDefaultEventsToOptimizely)
                return;

            var client = ServiceLocator.Current.GetInstance<IExperimentationClient>();
            client.CreateOrUpdateEvent(DefaultKeys.EventBasket, OptiEvent.Types.Other, "Basket interaction");
            client.CreateOrUpdateEvent(DefaultKeys.EventCategory, OptiEvent.Types.Other, "Category page visited");
            client.CreateOrUpdateEvent(DefaultKeys.EventOrder, OptiEvent.Types.Other, "Order placed");
            client.CreateOrUpdateEvent(DefaultKeys.EventProduct, OptiEvent.Types.Other, "Product page visited");
            client.CreateOrUpdateEvent(DefaultKeys.EventPageView, OptiEvent.Types.Other, "Standard page view");
            client.CreateOrUpdateEvent(DefaultKeys.EventWishlist, OptiEvent.Types.Other, "Wishlist interaction");
            client.CreateOrUpdateEvent(DefaultKeys.EventRevenue, OptiEvent.Types.Other, "Purchase order values");
        }

        public void Uninitialize(InitializationEngine context) { }
    }
}
