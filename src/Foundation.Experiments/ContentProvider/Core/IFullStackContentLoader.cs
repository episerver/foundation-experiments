using EPiServer.Core;
using System.Collections.Generic;

namespace Optimizely.DeveloperFullStack.Core
{
    public interface IOptimizelyFullStackContentLoader
    {
        T Get<T>(ContentReference fullStackContentLink) where T : BasicContent;

        T Get<T>(ContentReference fullStackContentLink, LoaderOptions loaderOptions) where T : BasicContent;

        IEnumerable<BasicContent> GetAll();

        IEnumerable<T> GetChildren<T>(ContentReference parentfullStackContentLink) where T : BasicContent;

        IEnumerable<T> GetChildren<T>(ContentReference parentfullStackContentLink, LoaderOptions loaderOptions) where T : BasicContent;

        IEnumerable<T> GetDescendants<T>(ContentReference parentfullStackContentLink) where T : BasicContent;

        IEnumerable<T> GetDescendants<T>() where T : BasicContent;

        IContent GetRoot();

        bool TryGet<T>(ContentReference contentLink, out T fullStackItem) where T : BasicContent;
    }
}