using EPiServer.Cms.Shell.Search;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.Localization;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using EPiServer.Shell.Search;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Optimizely.DeveloperFullStack.Core;
using Optimizely.DeveloperFullStack.Models;
using System.Collections.Generic;

namespace Optimizely.DeveloperFullStack
{
    [SearchProvider]
    public class OptimizelyFullStackSearchProvider : ContentSearchProviderBase<IContent, ContentType>
    {
        private readonly IOptimizelyFullStackContentLoader _optimizelyFullStackContentLoader;

        public OptimizelyFullStackSearchProvider(LocalizationService localizationService,
            ISiteDefinitionResolver siteDefinitionResolver,
            IContentTypeRepository contentTypeRepository,
            IOptimizelyFullStackContentLoader optimizelyFullStackContentLoader,
            EditUrlResolver editUrlResolver,
            LanguageResolver languageResolver,
            ServiceAccessor<SiteDefinition> siteDefinitionAccessorResolver,
            UrlResolver urlResolver,
            TemplateResolver templateResolver,
            UIDescriptorRegistry uiDescriptorRegistry)
            : base(localizationService, siteDefinitionResolver, contentTypeRepository, editUrlResolver, siteDefinitionAccessorResolver, languageResolver, urlResolver, templateResolver, uiDescriptorRegistry)
        {
            _optimizelyFullStackContentLoader = optimizelyFullStackContentLoader;
        }

        public override string Area { get { return FullStackConstants.RepositoryKey; } }

        public override string Category { get { return FullStackConstants.RepositoryName; } }

        protected override string IconCssClass { get { return "epi-resourceIcon epi-resourceIcon-block"; } }

        public override IEnumerable<SearchResult> Search(Query query)
        {
            var searchResults = new List<SearchResult>();

            var contents = _optimizelyFullStackContentLoader.GetDescendants<BasicContent>();
            foreach (var item in contents)
            {
                if (item.Name.ToLowerInvariant().Contains(query.SearchQuery.ToLowerInvariant()))
                {
                    var searchResult = CreateSearchResult(item);
                    if (item is FullStackBaseData fullStackBaseData)
                    {
                        searchResult.PreviewText = fullStackBaseData.Description;
                    }
                    searchResults.Add(searchResult);
                }
            }

            return searchResults;
        }
    }
}