using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Shell.ObjectEditing;
using Foundation.Experiments.Projects.ExperimentMapper.Models;

namespace Foundation.Experiments.Projects.ExperimentMapper
{
    public class ProjectExperimentMappingMetadataExtender : IMetadataExtender
    {
        public void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            // When content is being created the content link is 0
            if (metadata.Model is ProjectExperimentMapping && ((IContent) metadata.Model).ContentLink.ID != 0)
            {
                foreach (var modelMetadata in metadata.Properties)
                {
                    if (modelMetadata.PropertyName == "ExperimentId")
                    {
                        modelMetadata.IsReadOnly = true;
                    }
                }
            }
        }
    }
}