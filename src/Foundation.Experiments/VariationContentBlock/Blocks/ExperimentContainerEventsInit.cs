using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using Optimizely.DeveloperFullStack.Core;
using Optimizely.DeveloperFullStack.Models;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Experiments.ExperimentContentArea.Blocks
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ExperimentContainerEventsInit : IInitializableModule
    {
        private IContentRepository _contentRepository;

        private IContentLoader _contentLoader;

        private ContentAssetHelper _contentAssetHelper;

        private static IOptimizelyFullStackContentLoader _fullStackContentLoader;

        public void Initialize(InitializationEngine context)
        {
            _contentRepository = context.Locate.ContentRepository();
            _contentLoader = context.Locate.ContentLoader();
            _contentAssetHelper = context.Locate.Advanced.GetInstance<ContentAssetHelper>();
            _fullStackContentLoader = context.Locate.Advanced.GetInstance<IOptimizelyFullStackContentLoader>();
            var contentEvents = context.Locate.Advanced.GetInstance<IContentEvents>();
            contentEvents.SavedContent += ContentEvents_SavedContent;
            contentEvents.PublishingContent += ContentEvents_PublishingContent;
        }

        private void ContentEvents_PublishingContent(object sender, ContentEventArgs e)
        {
            if (e.Content is IContent content && content is ExperimentContainer experimentContainer)
            {
                var assetFolder = _contentAssetHelper.GetOrCreateAssetFolder(e.ContentLink);
                var contentVariationContentArea = CreateVariationContentArea(assetFolder.ContentLink, experimentContainer.ExperimentKey);

                if (!string.IsNullOrWhiteSpace(experimentContainer.ExperimentKey))
                {
                    experimentContainer.ExperimentContentArea = contentVariationContentArea;
                }
            }
        }

        private void ContentEvents_SavedContent(object sender, ContentEventArgs e)
        {
            if (e.Content is IContent content && content is ExperimentContainer experimentContainer)
            {
                var contentUpdate = experimentContainer.CreateWritableClone() as ExperimentContainer;
                var assetFolder = _contentAssetHelper.GetOrCreateAssetFolder(e.ContentLink);
                var contentVariationContentArea = CreateVariationContentArea(assetFolder.ContentLink, experimentContainer.ExperimentKey);

                if (experimentContainer.ExperimentContentArea == null && !string.IsNullOrWhiteSpace(experimentContainer.ExperimentKey))
                {
                    contentUpdate.ExperimentContentArea = contentVariationContentArea;
                    this._contentRepository.Save((IContent)contentUpdate, SaveAction.Publish, AccessLevel.NoAccess);
                }
            }
        }

        private ContentArea CreateVariationContentArea(ContentReference parent, string experimentKey)
        {
            if (string.IsNullOrEmpty(experimentKey))
            {
                return null;
            }

            var flag = _fullStackContentLoader.GetDescendants<FlagData>()
                .FirstOrDefault(x => x.Key.Equals(experimentKey));

            var variations = new List<VariationData>();
            if (flag != null)
            {
                variations.AddRange(_fullStackContentLoader.GetChildren<VariationData>(flag.ContentLink));
            }

            var experimentsContentArea = new ContentArea();
            var children = this._contentRepository.GetChildren<ExperimentVariationBlock>(parent);

            foreach (var variation in variations)
            {
                // check to see if children exists under this node already
                if (children.Any())
                {
                    var variationContentBlock = children.FirstOrDefault(x => x.VariationKey.Equals(variation.Key));
                    if (variationContentBlock != null)
                    {
                        experimentsContentArea.Items.Add(new ContentAreaItem() { ContentLink = ((IContent)variationContentBlock).ContentLink });
                        continue;
                    }
                }

                var experimentContent = _contentRepository.GetDefault<ExperimentVariationBlock>(parent);

                ((IContent)experimentContent).Name = $"Variation: {(!string.IsNullOrWhiteSpace(variation.Description) ? variation.Description : variation.Name)}-({experimentKey})";
                experimentContent.VariationKey = variation.Key;
                experimentContent.VariationDescription = !string.IsNullOrWhiteSpace(variation.Description) ? variation.Description : variation.Name;

                var variationReference = _contentRepository.Publish((IContent)experimentContent, AccessLevel.NoAccess);

                experimentsContentArea.Items.Add(new ContentAreaItem() { ContentLink = variationReference });
            }

            return experimentsContentArea;
        }

        public void Uninitialize(InitializationEngine context)
        {
            var contentEvents = context.Locate.Advanced.GetInstance<IContentEvents>();
            contentEvents.SavedContent -= ContentEvents_SavedContent;
        }
    }
}