using System;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Routing.Segments.Internal;
using Foundation.Experiments.Core.Interfaces;
using Foundation.Experiments.Projects.ExperimentMapper.Models;
using Foundation.Experiments.Projects.Interfaces;

namespace Foundation.Experiments.Projects.Impl
{
    public class DefaultProjectIdResolver : IProjectIdResolver
    {
        private readonly IExperimentationFactory _experimentationFactory;
        private readonly IUserRetriever _userRetriever;
        private readonly Lazy<HttpContextBase> _httpContextBase;
        private readonly Lazy<IContentRepository> _contentRepo;
        private readonly IPublishedStateAssessor _publishedStateAssessor;

        public DefaultProjectIdResolver(IUserRetriever userRetriever, 
            IExperimentationFactory experimentationFactory,
            Lazy<HttpContextBase> httpContextBase, Lazy<IContentRepository> contentRepo, 
            IPublishedStateAssessor publishedStateAssessor)
        {
            _userRetriever = userRetriever;
            _experimentationFactory = experimentationFactory;
            _httpContextBase = httpContextBase;
            _contentRepo = contentRepo;
            _publishedStateAssessor = publishedStateAssessor;
        }

        public int? GetProjectId()
        {
            try
            {
                if (ShouldContinue() == false) 
                    return null;

                var instance = _experimentationFactory.Instance;
                if (instance == null)
                    return null;

                //if (_httpContextBase?.Value.Items["ProjectId"] != null)
                //{
                //    return int.Parse(_httpContextBase.Value.Items["ProjectId"].ToString());
                //}

                var experimentMap = GetProjectExperimentMapping();
                if (experimentMap == null)
                    return null;

                var experimentKey = experimentMap.ExperimentKey;

                var userId = _userRetriever.GetUserId();
                if (userId == string.Empty)
                    return 0;

                var userAttributes = _userRetriever.GetUserAttributes();

                var projectVariation = instance.Activate(experimentKey, userId, userAttributes);
                if (projectVariation != null)
                {
                    //Map the experimentation variation key to a project Id
                    var mappingItem = experimentMap.ExperimentMapping.Where(x => x.VariationKey == projectVariation.Key).ToList();
                    if (mappingItem.Count() == 1 && !string.IsNullOrEmpty(mappingItem.First().ProjectId))
                    {
                        if (int.TryParse(mappingItem.First().ProjectId, out var projectId))
                        {
                            if (projectId > 0)
                            {
                                //if (_httpContextBase != null)
                                //{
                                //    _httpContextBase.Value.Items["ProjectId"] = projectId;
                                //}

                                return projectId;
                            }
                        }
                    }

                    return null;
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }

        public bool ShouldContinue()
        {
            if (RequestSegmentContext.CurrentContextMode == ContextMode.Default)
            {
                if (_httpContextBase != null && _httpContextBase.Value.Request.Url != null &&
                    !_httpContextBase.Value.Request.Url.AbsolutePath.ToLower().StartsWith("/episerver"))
                {
                    return true;
                }
            }

            return false;
        }

        private ProjectExperimentMapping GetProjectExperimentMapping()
        {
            //Currently a single instance of experiment > project mapping stored as a ProjectExperimentMapping held at the site block root
            var projectMappings =
                _contentRepo.Value.GetChildren<ProjectExperimentMapping>(ContentReference.SiteBlockFolder);

            if (projectMappings != null)
            {
                if (_publishedStateAssessor.IsPublished(projectMappings.First() as IContent))
                {
                    return projectMappings?.First();
                }
            }

            return null;
        }
    }
}