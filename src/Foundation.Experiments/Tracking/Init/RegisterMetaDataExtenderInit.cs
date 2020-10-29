using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Shell.ObjectEditing;
using Foundation.Experiments.Tracking.TrackingBlock;

namespace Foundation.Experiments.Tracking.Init
{
    [InitializableModule]
    [ModuleDependency(typeof(AddExperimentTrackingPropertyInit))]
    public class RegisterMetaDataExtenderInit : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            if (context.HostType == HostType.WebApplication)
            {
                var registry = context.Locate.Advanced.GetInstance<MetadataHandlerRegistry>();
                registry.RegisterMetadataHandler(typeof(ContentData), new ExperimentTrackingDataExtender());
            }
        }

        public void Uninitialize(InitializationEngine context) { }
    }
}