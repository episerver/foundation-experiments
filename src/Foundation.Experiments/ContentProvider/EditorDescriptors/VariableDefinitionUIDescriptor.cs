using EPiServer.Shell;
using Optimizely.DeveloperFullStack.Models;

namespace Optimizely.DeveloperFullStack.EditorDescriptors
{
    [UIDescriptorRegistration]
    public class VariableDefinitionUIDescriptor : UIDescriptor<VariableDefinitionData>
    {
        public VariableDefinitionUIDescriptor()
            : base("epi-iconObjectVariation")
        {
            CommandIconClass = "epi-iconObjectVariation";
            IsPrimaryType = true;
            ContainerTypes = new[]
            {
                typeof (FlagData)
            };
        }
    }
}