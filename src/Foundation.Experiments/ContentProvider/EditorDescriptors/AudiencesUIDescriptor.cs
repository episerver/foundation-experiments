using EPiServer.Shell;
using Optimizely.DeveloperFullStack.Models;

namespace Optimizely.DeveloperFullStack.EditorDescriptors
{
    [UIDescriptorRegistration]
    public class AudiencesUIDescriptor : UIDescriptor<AudienceData>
    {
        public AudiencesUIDescriptor()
            : base("fullstack-audience")
        {
            CommandIconClass = "fullstack-audience";
            IsPrimaryType = true;
            ContainerTypes = new[]
            {
                typeof (FullStackBaseData)
            };
        }
    }
}