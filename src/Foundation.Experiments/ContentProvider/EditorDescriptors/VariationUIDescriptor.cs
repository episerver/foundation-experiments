using EPiServer.Shell;
using Optimizely.DeveloperFullStack.Models;

namespace Optimizely.DeveloperFullStack.Descriptors
{
    [UIDescriptorRegistration]
    public class VariationUIDescriptor : UIDescriptor<VariationData>
    {
        public VariationUIDescriptor()
            : base("fullstack-feature")
        {
            CommandIconClass = "fullstack-feature";
            IsPrimaryType = true;
            ContainerTypes = new[]
            {
                typeof (FlagData)
            };
        }
    }
}