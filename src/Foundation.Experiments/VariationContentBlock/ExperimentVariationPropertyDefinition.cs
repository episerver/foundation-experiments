using EPiServer.Core;
using EPiServer.PlugIn;
using Foundation.Experiments.ExperimentContentArea.Blocks;

namespace Foundation.Experiments.ExperimentContentArea
{
    [PropertyDefinitionTypePlugIn]
    public class ExperimentVariationPropertyDefinition : PropertyList<ExperimentVariationBlock> { }
}