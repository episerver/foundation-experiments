using EPiServer;
using EPiServer.Core;
using System.Collections.Generic;
using System.Linq;

namespace Optimizely.DeveloperFullStack.Core
{
    public class OptimizelyFullStackContentLoader : IOptimizelyFullStackContentLoader
    {
        protected readonly IContentLoader _contentLoader;

        public OptimizelyFullStackContentLoader(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public virtual T Get<T>(ContentReference fullStackContentLink) where T : BasicContent =>
            _contentLoader.Get<T>(fullStackContentLink, CreateDefaultLoadOptions());

        public virtual T Get<T>(ContentReference fullStackContentLink, LoaderOptions loaderOptions) where T : BasicContent =>
            _contentLoader.Get<T>(fullStackContentLink, loaderOptions);

        public virtual IEnumerable<T> GetChildren<T>(ContentReference parentfullStackContentLink) where T : BasicContent =>
            GetChildren<T>(parentfullStackContentLink, CreateDefaultListOptions());

        public virtual IEnumerable<T> GetChildren<T>(ContentReference parentfullStackContentLink, LoaderOptions loaderOptions) where T : BasicContent =>
            _contentLoader.GetChildren<T>(parentfullStackContentLink, loaderOptions);

        public virtual IEnumerable<T> GetDescendants<T>(ContentReference parentfullStackContentLink) where T : BasicContent
        {
            var decendantLinks = _contentLoader.GetDescendents(parentfullStackContentLink);
            return _contentLoader.GetItems(decendantLinks, LanguageSelector.AutoDetect())
                .OfType<T>();
        }

        /// <summary>
        /// Gets all items under Fullstack Content Provider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IEnumerable<T> GetDescendants<T>() where T : BasicContent
        {
            var decendantLinks = _contentLoader.GetDescendents(GetRoot().ContentLink);
            return _contentLoader.GetItems(decendantLinks, LanguageSelector.AutoDetect())
                .OfType<T>();
        }

        public virtual IEnumerable<BasicContent> GetAll()
        {
            var decendantLinks = _contentLoader.GetDescendents(GetRoot().ContentLink);
            return _contentLoader.GetItems(decendantLinks, LanguageSelector.AutoDetect())
                .OfType<BasicContent>();
        }

        public virtual IContent GetRoot() =>
                OptimizelyFullStackContentProvider.GetEntryPoint();

        public virtual bool TryGet<T>(ContentReference contentLink, out T fullStackItem) where T : BasicContent =>
            _contentLoader.TryGet(contentLink, out fullStackItem);

        protected virtual LoaderOptions CreateDefaultLoadOptions() => new LoaderOptions
            {
                LanguageLoaderOption.FallbackWithMaster(LanguageSelector.AutoDetect().Language)
            };

        protected virtual LoaderOptions CreateDefaultListOptions() => new LoaderOptions
            {
                LanguageLoaderOption.Fallback(LanguageSelector.AutoDetect().Language)
            };
    }
}