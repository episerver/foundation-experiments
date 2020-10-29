using System;
using System.Collections.Generic;
using EPiServer;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Foundation.Experiments.Tracking.Init;

namespace Foundation.Experiments.Tracking.TrackingBlock
{
    public class ExperimentTrackingDataExtender : IMetadataExtender
    {
        internal Injected<EPiServer.Shell.Modules.ModuleTable> ModuleTable { get; set; }

        public void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (typeof(IExperimentTracking).IsAssignableFrom(metadata.Model.GetOriginalType()))
            {
                foreach (var modelMetadata in metadata.Properties)
                {
                    var property = (ExtendedMetadata)modelMetadata;
                    if (property.PropertyName == AddExperimentTrackingPropertyInit.ExperimentEventsPropertyName)
                    {
                        var allowedTypes = new AllowedTypesAttribute(new[] { typeof(ExperimentEventTracking) });
                        property.InitializeFromAttributes(new Attribute[] { allowedTypes });
                        var fullName = typeof(ExperimentEventTracking).FullName;
                        if (fullName != null)
                        {
                            var allowedType = fullName.ToLower();
                            if (property.EditorConfiguration.ContainsKey("AllowedTypes"))
                            {
                                property.EditorConfiguration["AllowedTypes"] = new string[] {allowedType};
                            }
                            else
                            {
                                property.EditorConfiguration.Add("AllowedTypes", new string[] { allowedType });
                            }
                            if (property.EditorConfiguration.ContainsKey("AllowedDndTypes"))
                            {
                                property.EditorConfiguration["AllowedDndTypes"] = new string[] {allowedType + ".fragment"};
                            }
                            else
                            {
                                property.EditorConfiguration.Add("AllowedDndTypes",
                                    new string[] {allowedType + ".fragment"});
                            }
                        }
                    }
                }
            }
        }
    }
}