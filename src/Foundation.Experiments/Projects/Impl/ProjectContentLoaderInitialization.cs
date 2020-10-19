using System;
using System.Runtime.Caching;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Foundation.Experiments.Projects.ExperimentMapper.Models;
using Foundation.Experiments.Projects.Init;
using Foundation.Experiments.Projects.Interfaces;

namespace Foundation.Experiments.Projects.Impl
{
    [ModuleDependency(typeof(ProjectDependencyInit))]
    public class ProjectContentLoaderInitialization : IInitializableModule
    {
        private Lazy<IProjectIdResolver> _projectIdResolver;
        private IExperimentProjectIdentifier _experimentProjectIdentifier;
        private IContentLoader _contentLoader;
        private ObjectCache _cache;
        private static readonly object Padlock = new object();

        public void Initialize(InitializationEngine context)
        {
            _cache = new MemoryCache("YS-Cache");

            _projectIdResolver = context.Locate.Advanced.GetInstance<Lazy<IProjectIdResolver>>();
            _contentLoader = context.Locate.Advanced.GetInstance<IContentLoader>();
            _experimentProjectIdentifier = context.Locate.Advanced.GetInstance<IExperimentProjectIdentifier>();

            var events = context.Locate.Advanced.GetInstance<IContentEvents>();
            events.LoadedContent += Events_LoadedContent;
            events.SavedContent += Events_SavedContent;
            events.PublishedContent += HandleProjectExperimentMappingChange;
            events.DeletedContent += HandleProjectExperimentMappingChange;
        }

        private void HandleProjectExperimentMappingChange(object sender, ContentEventArgs e)
        {
            if (e.Content is ProjectExperimentMapping)
            {
                lock (Padlock)
                {
                    _cache = new MemoryCache("YS-Cache");
                    foreach (var projectMap in (e.Content as ProjectExperimentMapping).ExperimentMapping)
                    {
                        if (projectMap.ProjectId != null)
                        {
                            _experimentProjectIdentifier.InvalidateCache(projectMap.ProjectId);
                        }
                    }
                }
            }
        }

        private void Events_SavedContent(object sender, ContentEventArgs e)
        {
            lock (Padlock)
            {
                var cacheKey = e.ContentLink.ID + "_" + e.ContentLink.WorkID;
                if (_cache.Contains(cacheKey))
                {
                    _cache.Remove(cacheKey);
                }
            }
        }

        private void Events_LoadedContent(object sender, ContentEventArgs e)
        {
            if (HttpContext.Current == null)
                return;

            if (!ShouldContinue(e.Content))
                return;

            var projectId = _projectIdResolver.Value.GetProjectId(new HttpContextWrapper(HttpContext.Current));
            if (!projectId.HasValue || projectId.Value == 0)
            {
                return;
            }

            lock (Padlock)
            {
                if (HttpContext.Current?.Items["SkipLoading" + e.ContentLink.ID] != null)
                {
                    var skip = HttpContext.Current?.Items["SkipLoading" + e.ContentLink.ID] is bool && (bool)HttpContext.Current?.Items["SkipLoading" + e.ContentLink.ID];
                    if (skip)
                        return;
                }
            }

            var projectContentRef = _experimentProjectIdentifier.GetProjectReference(e.ContentLink, projectId.Value);
            if (projectContentRef != e.ContentLink)
            {
                try
                {
                    lock (Padlock)
                    {
                        HttpContext.Current.Items["SkipLoading" + e.ContentLink.ID] = true;

                        var cacheKey = projectContentRef.ID + "_" + projectContentRef.WorkID;
                        if (_cache[cacheKey] != null)
                        {
                            e.ContentLink = projectContentRef;
                            e.Content = _cache[cacheKey] as IContent;
                        }
                        else
                        {
                            var replacementContent = _contentLoader.Get<IContent>(projectContentRef) as ContentData;
                            if (replacementContent != null)
                            {
                                var replaceContent = replacementContent.CreateWritableClone() as IContent;
                                var versionable = (replaceContent as IVersionable);
                                if (versionable != null)
                                {
                                    versionable.Status = VersionStatus.Published;
                                    versionable.StartPublish = DateTime.Now.AddSeconds(-5);
                                }

                                e.ContentLink = projectContentRef;
                                e.Content = replaceContent;
                                _cache[cacheKey] = replaceContent;
                            }
                        }
                    }
                }
                catch
                {
                    // ignored
                }

                HttpContext.Current.Items["SkipLoading" + e.ContentLink.ID] = false;
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            var events = context.Locate.Advanced.GetInstance<IContentEvents>();
            events.LoadedContent -= Events_LoadedContent;
        }

        private bool ShouldContinue(IContent content)
        {
            if (content == null)
                return false;

            if (content.ContentLink.ID == ContentReference.RootPage.ID)
                return false;

            if (content.ContentLink.ID == ContentReference.SiteBlockFolder.ID)
                return false;

            if (content.ContentLink.ID == ContentReference.GlobalBlockFolder.ID)
                return false;

            if (content.ContentLink.ID == ContentReference.WasteBasket.ID)
                return false;

            if (content.GetOriginalType().IsAssignableFrom(typeof(ProjectExperimentMapping)))
                return false;

            if ((content as IVersionable) == null)
                return false;

            return true;
        }
    }
}