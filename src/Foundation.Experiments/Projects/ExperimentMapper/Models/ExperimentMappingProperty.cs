using EPiServer.Core;
using EPiServer.PlugIn;

namespace Foundation.Experiments.Projects.ExperimentMapper.Models
{
    [PropertyDefinitionTypePlugIn]
    public class ExperimentMappingProperty : PropertyList<ExperimentMapItem> { }
}