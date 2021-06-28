using EPiServer.Shell;
using Optimizely.DeveloperFullStack.Models;

namespace Optimizely.DeveloperFullStack.Descriptors
{
    [UIDescriptorRegistration]
    public class ExperimentUIDescriptor : UIDescriptor<ExperimentData>
    {
        public ExperimentUIDescriptor()
            : base("fullstack-experiment")
        {
            CommandIconClass = "fullstack-experiment";
            IsPrimaryType = true;
            ContainerTypes = new[]
            {
                typeof (FlagData)
            };
        }
    }
}