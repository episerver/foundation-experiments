using System;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Tracking.Core;
using Foundation.Experiments.Core.Config;
using Foundation.Experiments.Core.Interfaces;
using Foundation.Experiments.Rest;
using Newtonsoft.Json;
using OptimizelySDK.Entity;

namespace Foundation.Experiments.Tracking.VisitorIntellegence
{
    [ServiceConfiguration(ServiceType = typeof(ITrackingDataInterceptor), Lifecycle = ServiceInstanceScope.Singleton)]
    public class ExperimentationDataTracker : ITrackingDataInterceptor
    {
        private static readonly Lazy<IExperimentationFactory> ExperimentationFactory = new Lazy<IExperimentationFactory>(() => ServiceLocator.Current.GetInstance<IExperimentationFactory>());
        private static readonly Lazy<IContentLoader> ContentLoader = new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());
        private static readonly object PadLock = new object();

        public int SortOrder => 9999;
        public void Intercept<T>(TrackingData<T> trackingData)
        {
            var options = ServiceLocator.Current.GetInstance<ExperimentationOptions>();
            if (!options.TrackEventsInOptimizely)
                return;

            var userRetriever = ServiceLocator.Current.GetInstance<IUserRetriever>();
            var userId = userRetriever.GetUserId(new HttpContextWrapper(HttpContext.Current));
            var userAttributes = userRetriever.GetUserAttributes(new HttpContextWrapper(HttpContext.Current));

            EventTags eventTags;
            if (trackingData.EventType == DefaultKeys.EventBasket)
            {
                // Todo: Once bug in foundation is fixed to supply information if an item has been added / updated / removed, this will only measure the event itself
                eventTags = new EventTags();

                if (trackingData.Payload != null)
                {
                    var payload = trackingData.Payload as dynamic;
                    eventTags.Add(DefaultKeys.Items, JsonConvert.SerializeObject(payload.Basket.Items));
                    eventTags.Add(DefaultKeys.Currency, payload.Basket.Currency);
                    eventTags.Add(DefaultKeys.Language, payload.Lang);
                    eventTags.Add(DefaultKeys.Channel, payload.Channel);
                }

                Track(DefaultKeys.EventBasket, userId, userAttributes, eventTags);//add to basket
            }

            else if (trackingData.EventType == DefaultKeys.EventProduct)
            {
                eventTags = new EventTags();

                if (trackingData.Payload != null)
                {
                    var payload = trackingData.Payload as dynamic;
                    eventTags.Add(DefaultKeys.Language, payload.Lang);
                    eventTags.Add(DefaultKeys.Channel, payload.Channel);
                    eventTags.Add(DefaultKeys.ParentPageUri, payload.PreviousUri?.ToString());
                }

                Track(DefaultKeys.EventProduct, userId, userAttributes, eventTags);
            }

            else if (trackingData.EventType == DefaultKeys.EventCategory)
            {
                eventTags = new EventTags();
                
                if (trackingData.Payload != null)
                {
                    var payload = trackingData.Payload as dynamic;
                    eventTags.Add(DefaultKeys.CategoryName, payload.Category);
                    eventTags.Add(DefaultKeys.Language, payload.Lang);
                    eventTags.Add(DefaultKeys.Channel, payload.Channel);
                    eventTags.Add(DefaultKeys.ParentPageUri, payload.PreviousUri?.ToString());

                    if (options.RegisterAndTrackCommerceCategoriesInOptimizely)
                    {
                        var isSuccessful = Track(payload.Category, userId, userAttributes, eventTags);
                        if (!isSuccessful)
                        {
                            isSuccessful = CreateEvent(payload.Category, "");
                            if (isSuccessful)
                                Track(payload.Category, userId, userAttributes, eventTags);
                        }
                    }
                }
                
                Track(DefaultKeys.EventCategory, userId, userAttributes, eventTags);
            }

            else if (trackingData.EventType == DefaultKeys.EventPageView)
            {
                eventTags = new EventTags();
                
                if (trackingData.Payload != null)
                {
                    var payload = trackingData.Payload as dynamic;
                    eventTags.Add(DefaultKeys.Language, payload.Epi.Language);

                    var item = ContentLoader.Value.Get<IContent>(payload.Epi.ContentGuid);
                    eventTags.Add(DefaultKeys.PageType, item.PageTypeName);

                    if (options.RegisterAndTrackPageTypesInOptimizely)
                    {
                        var isSuccessful = Track(item.PageTypeName, userId, userAttributes, eventTags);
                        if (!isSuccessful)
                        {
                            isSuccessful = CreateEvent(item.PageTypeName, item.PageDescription);
                            if (isSuccessful)
                                Track(item.PageTypeName, userId, userAttributes, eventTags);
                        }
                    }
                }

                Track(DefaultKeys.EventPageView, userId, userAttributes, eventTags);
            }

            else if (trackingData.EventType == DefaultKeys.EventWishlist)
            {
                eventTags = new EventTags();
               
                if (trackingData.Payload != null)
                {
                    var payload = trackingData.Payload as dynamic;
                    eventTags.Add(DefaultKeys.Language, payload.Lang);
                    eventTags.Add(DefaultKeys.Channel, payload.Channel);
                }

                Track(DefaultKeys.EventWishlist, userId, userAttributes, eventTags);
            }

            else if (trackingData.EventType == DefaultKeys.EventOrder)
            {
                eventTags = new EventTags();

                if (trackingData.Payload != null)
                {
                    var payload = trackingData.Payload as dynamic;
                    eventTags.Add(DefaultKeys.Items, JsonConvert.SerializeObject(payload.Order.Items));
                    eventTags.Add(DefaultKeys.OneProductInOrder, payload.Order.Items.Count == 1);
                    eventTags.Add(DefaultKeys.TwoThreeProductInOrder, payload.Order.Items.Count == 2 || payload.Order.Items.Count == 3);
                    eventTags.Add(DefaultKeys.FourOrMoreProductInOrder, payload.Order.Items.Count > 3);
                    eventTags.Add(DefaultKeys.Currency, payload.Order.Currency);
                    eventTags.Add(DefaultKeys.Language, payload.Lang);
                    eventTags.Add(DefaultKeys.Channel, payload.Channel);
                }

                Track(DefaultKeys.EventOrder, userId, userAttributes, eventTags);

                if (trackingData.Payload != null)
                {
                    eventTags = new EventTags();

                    var payload = trackingData.Payload as dynamic;
                    var revenueValueInPennies = ((decimal?)payload.Order.Total).Value * 100;
                    eventTags.Add(DefaultKeys.Revenue, Convert.ToInt32(revenueValueInPennies));
                    Track(DefaultKeys.EventRevenue, userId, userAttributes, eventTags);
                }
            }

            bool CreateEvent(string key, string description)
            {
                lock (PadLock)
                {
                    var client = ServiceLocator.Current.GetInstance<IExperimentationClient>();
                    try
                    {
                        var result = client.CreateEventIfNotExists(key, description: description);
                        return result;
                    }
                    catch { }
                }
                return false;
            }
        }

        private static bool Track(string type, string userId, UserAttributes userAttributes, EventTags eventTags)
        {
            try
            {
                if (ExperimentationFactory.Value.IsConfigured)
                {
                    ExperimentationFactory.Value.Instance?.Track(type, userId, userAttributes, eventTags);
                }
                return true;
            }
            catch (Exception e)
            {
                ServiceLocator.Current.TryGetExistingInstance(out ILogger epiErrorLogger);
                epiErrorLogger?.Log(Level.Warning, "Optimizely tracking failed", e);
            }

            return false;
        }
    }
}
