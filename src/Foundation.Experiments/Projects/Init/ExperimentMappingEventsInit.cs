using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using Foundation.Experiments.Projects.ExperimentMapper.Models;
using Foundation.Experiments.Rest;

namespace Foundation.Experiments.Projects.Init
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ExperimentMappingEventsInit : IInitializableModule
    {
        private Lazy<IExperimentationClient> _experimentationClient;
        private Lazy<IContentRepository> _contentRepo;
        private Lazy<ProjectRepository> _projectRepo;

        public void Initialize(InitializationEngine context)
        {
            _experimentationClient = context.Locate.Advanced.GetInstance<Lazy<IExperimentationClient>>();
            _contentRepo = context.Locate.Advanced.GetInstance<Lazy<IContentRepository>>();
            _projectRepo = context.Locate.Advanced.GetInstance<Lazy<ProjectRepository>>();
            var contentEvents = context.Locate.Advanced.GetInstance<IContentEvents>();
            contentEvents.CreatedContent += ContentEvents_CreatedContent;
        }

        private void ContentEvents_CreatedContent(object sender, EPiServer.ContentEventArgs e)
        {
            if (e.Content is ProjectExperimentMapping mapping)
            {
                if (mapping.ExperimentMapping == null)
                {
                    var updatedContent = mapping.CreateWritableClone() as ProjectExperimentMapping;
                    if (updatedContent.ExperimentMapping == null)
                        updatedContent.ExperimentMapping = new List<ExperimentMapItem>();

                    var variations = _experimentationClient.Value.GetExperiment(mapping.ExperimentKey).Variations;
                    var appProjects = _projectRepo.Value.List().ToList();
                    foreach (var variation in variations)
                    {
                        var mappingItem = new ExperimentMapItem()
                        {
                            VariationKey = variation.Key,
                            VariationDescription = variation.Description
                        };
                        var project = appProjects.Where(x => x.Name == variation.Description);
                        if (project.Count() == 1)
                        {
                            mappingItem.ProjectId = project.First().ID.ToString();
                        }
                        updatedContent.ExperimentMapping.Add(mappingItem);
                    }

                    _contentRepo.Value.Save(updatedContent as IContent, SaveAction.ForceCurrentVersion, AccessLevel.NoAccess);
                    e.CancelAction = true;
                }
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            var contentEvents = context.Locate.Advanced.GetInstance<IContentEvents>();
            contentEvents.CreatedContent -= ContentEvents_CreatedContent;
        }
    }
}