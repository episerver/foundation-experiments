using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using Foundation.Experiments.Projects.Interfaces;

namespace Foundation.Experiments.Projects.Impl
{
    public class ExperimentProjectIdentifier : IExperimentProjectIdentifier
    {
        private readonly ProjectRepository _projectRepository;
        private readonly IProjectIdResolver _projectIdResolver;
        private static ObjectCache _cache;
        private static readonly object Padlock = new object();

        public ExperimentProjectIdentifier(ProjectRepository projectRepository, IProjectIdResolver projectIdResolver)
        {
            _projectRepository = projectRepository;
            _projectIdResolver = projectIdResolver;
            _cache = new MemoryCache("Experimentation-ProjectListing");
        }

        public ContentReference GetProjectVersion(ContentReference publishedReference, HttpContextBase httpContext)
        {
            var projectId = _projectIdResolver.GetProjectId(httpContext);
            return !projectId.HasValue ? publishedReference : GetProjectReference(publishedReference, projectId.Value);
        }

        public ContentReference GetProjectReference(ContentReference publishedReference, int projectId)
        {
            if (projectId < 1)
                return publishedReference;

            IEnumerable<ProjectItem> items;
            var cacheKey = projectId.ToString();
            lock (Padlock)
            {
                if (_cache.Contains(cacheKey))
                {
                    items = _cache[cacheKey] as IEnumerable<ProjectItem>;
                }
                else
                {
                    items = _projectRepository.ListItems(projectId);
                    _cache[cacheKey] = items;
                }
            }

            if (items == null)
            {
                return publishedReference;
            }

            var item = items.FirstOrDefault(x => x.ContentLink.ToReferenceWithoutVersion() == publishedReference.ToReferenceWithoutVersion());

            if (item == null)
            {
                return publishedReference;
            }
            else
            {
                return items.FirstOrDefault(x => x.ContentLink.ID == item.ContentLink.ID).ContentLink;
            }
        }

        public void InvalidateCache(string projectId)
        {
            lock (Padlock)
            {
                if (_cache.Contains(projectId.ToString()))
                {
                    _cache.Remove(projectId.ToString());
                }
            }
        }
    }
}