using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Foundation.Experiments.Projects.Interfaces;

namespace Foundation.Experiments.Projects.Impl
{
    public class ProjectFilterProvider : IFilterProvider
    {
        private FilterProviderCollection _filterProviders;
        private readonly Type _authorizeContent = typeof(AuthorizeContentAttribute);
        private readonly IProjectIdResolver _projectIdResolver;

        public ProjectFilterProvider(IProjectIdResolver projectIdResolver)
        {
            _projectIdResolver = projectIdResolver;
        }

        public void Init(IList<IFilterProvider> filters)
        {
            _filterProviders = new FilterProviderCollection(filters);
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            var filters = _filterProviders.GetFilters(controllerContext, actionDescriptor);

            var projectId = _projectIdResolver.GetProjectId(controllerContext.HttpContext);
            if (projectId == null || projectId.Value == 0)
                return filters;

            return projectId > 0 ? filters.Where(x => x.Instance.GetType() != _authorizeContent) : filters;
        }
    }
}