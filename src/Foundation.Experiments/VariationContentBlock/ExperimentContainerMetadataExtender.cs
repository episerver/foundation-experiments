using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Shell.ObjectEditing;

namespace Foundation.Experiments.ExperimentContentArea
{
    public class ExperimentContainerMetadataExtender : IMetadataExtender
    {
        public void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (metadata.Model is Blocks.ExperimentContainer experimentContainer)
            {
                ToggleControls(metadata, metadata.Properties, experimentContainer);
            }
            else
            {
                foreach (var modelProperty in metadata.Properties)
                {
                    if (!modelProperty.ModelType.IsAssignableFrom(typeof(Blocks.ExperimentContainer))) continue;

                    var data = (EPiServer.SpecializedProperties.PropertyBlock<Blocks.ExperimentContainer>)modelProperty.Model;
                    if (data.Value is Blocks.ExperimentContainer actualContainer)
                    {
                        actualContainer.ExperimentContentArea?.MakeReadOnly();
                        ToggleControls(metadata, modelProperty.Properties, actualContainer);
                    }
                }
            }
        }

        private void ToggleControls(ExtendedMetadata metadata, IEnumerable<ModelMetadata> properties, Blocks.ExperimentContainer experimentContainer)
        {
            if (metadata.Model is IContent data)
            {
                if (data.ContentLink.ID == 0)
                {
                    foreach (var modelMetadata in properties)
                    {
                        if (modelMetadata.PropertyName.StartsWith("ExperimentContentArea"))
                        {
                            modelMetadata.ShowForDisplay = false;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var modelMetadata in properties)
                    {
                        if (modelMetadata.PropertyName.StartsWith("ExperimentContentArea"))
                        {
                            modelMetadata.ShowForDisplay = true;
                            modelMetadata.IsReadOnly = true;
                        }
                    }
                }
            }
        }
    }
}