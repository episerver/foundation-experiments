using EPiServer.Shell;
using Optimizely.DeveloperFullStack.Models;

namespace Optimizely.DeveloperFullStack.Descriptors
{
    [UIDescriptorRegistration]
    public class FlagUIDescriptor : UIDescriptor<FlagData>
    {
        public FlagUIDescriptor()
            : base("fullstack-flag")
        {
            CommandIconClass = "fullstack-flag";
            IsPrimaryType = true;
            ContainerTypes = new[]
            {
                typeof (FlagFolderData)
            };
        }
    }
}