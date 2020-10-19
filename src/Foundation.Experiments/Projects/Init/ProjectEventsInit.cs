using System;
using System.Linq;
using EPiServer.DataAbstraction;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Foundation.Experiments.Projects.Interfaces;

namespace Foundation.Experiments.Projects.Init
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ProjectEventsInit : IInitializableModule
    {
        private Lazy<IExperimentProjectIdentifier> _experimentProjectIdentifier;

        public void Initialize(InitializationEngine context)
        {
            _experimentProjectIdentifier = context.Locate.Advanced.GetInstance<Lazy<IExperimentProjectIdentifier>>();
            ProjectRepository.ProjectItemsSaved += ProjectRepository_ProjectItemsSaved;
        }

        private void ProjectRepository_ProjectItemsSaved(object sender, ProjectItemsEventArgs e)
        {
            var projectId = e.ProjectItems.First().ProjectID;
            _experimentProjectIdentifier.Value.InvalidateCache(projectId.ToString());
        }

        public void Uninitialize(InitializationEngine context)
        {
            ProjectRepository.ProjectItemsSaved += ProjectRepository_ProjectItemsSaved;
        }
    }
}