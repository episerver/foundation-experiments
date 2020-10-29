using System;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing.Segments.Internal;
using Foundation.Experiments.Core.Interfaces;
using Foundation.Experiments.Tracking.TrackingBlock;

namespace Foundation.Experiments.Tracking.Init
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ExperimentEventTrackingInit : IInitializableModule
    {
        private Lazy<IExperimentationFactory> _experimentationFactory;
        private Lazy<IUserRetriever> _userRetriever;
        private Lazy<IContentLoader> _contentLoader;

        public void Initialize(InitializationEngine context)
        {
            _experimentationFactory = context.Locate.Advanced.GetInstance<Lazy<IExperimentationFactory>>();
            _userRetriever = context.Locate.Advanced.GetInstance<Lazy<IUserRetriever>>();
            _contentLoader = context.Locate.Advanced.GetInstance<Lazy<IContentLoader>>();
            var contentEvents = context.Locate.Advanced.GetInstance<IContentEvents>();
            contentEvents.LoadedContent += ContentEvents_LoadedContent;
        }

        private void ContentEvents_LoadedContent(object sender, ContentEventArgs e)
        {
            if (HttpContext.Current == null || 
                HttpContext.Current.Request.RawUrl.StartsWith("/episerver/"))
                return;

            if (RequestSegmentContext.CurrentContextMode == ContextMode.Default && 
                e.Content != null &&
                e.Content.Property[AddExperimentTrackingPropertyInit.ExperimentEventsPropertyName]?.Value is ContentArea experimentEvents &&
                experimentEvents.IsEmpty == false)
            {
                var httpContext = new HttpContextWrapper(HttpContext.Current);
                var contextKey = "skip-ContentEvents-LoadedContent-" + e.Content.ContentLink.ID;

                if (httpContext.Items[contextKey] != null)
                    return;
                httpContext.Items.Add(contextKey, true);

                foreach (var experimentEventsItem in experimentEvents.Items)
                {
                    var trackingEvent =
                        _contentLoader.Value.Get<ExperimentEventTracking>(experimentEventsItem.ContentLink);

                    if (_experimentationFactory.Value.IsConfigured)
                    {
                        var userId = _userRetriever.Value.GetUserId(httpContext);
                        var userAttributes = _userRetriever.Value.GetUserAttributes(httpContext);
                        // Avoid double tracking in the same request
                        var trackingKey = "trackedEvent-" + trackingEvent.EventName;
                        if (httpContext.Items[trackingKey] != null)
                            return;
                        try
                        {
                            _experimentationFactory.Value.Instance.Track(trackingEvent.EventName, userId, userAttributes);
                        }
                        catch (Exception ex)
                        {
                            ServiceLocator.Current.TryGetExistingInstance(out ILogger epiErrorLogger);
                            epiErrorLogger?.Log(Level.Warning, "Event tracking failed for " + trackingEvent.EventName, ex);
                        }

                        httpContext.Items.Add(trackingKey, true);
                    }
                }
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            var contentEvents = context.Locate.Advanced.GetInstance<IContentEvents>();
            contentEvents.LoadedContent -= ContentEvents_LoadedContent;
        }
    }
}