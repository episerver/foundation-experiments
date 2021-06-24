using EPiServer.Core;
using EPiServer.Shell;
using Optimizely.DeveloperFullStack.Models;

namespace Optimizely.DeveloperFullStack.EditorDescriptors
{
    [UIDescriptorRegistration]
    public class FullStackFolderUIDescriptor : UIDescriptor<IFullStackFolder>
    {
        public FullStackFolderUIDescriptor()
            : base("fullstack-folder")
        {
            CommandIconClass = "fullstack-folder";
            IsPrimaryType = true;
            ContainerTypes = new[]
            {
                typeof (ContentFolder)
            };
        }
    }
}