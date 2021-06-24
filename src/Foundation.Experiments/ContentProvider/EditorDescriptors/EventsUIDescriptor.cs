using EPiServer.Shell;
using Optimizely.DeveloperFullStack.Models;

namespace Optimizely.DeveloperFullStack.EditorDescriptors
{
    [UIDescriptorRegistration]
    public class EventsUIDescriptor : UIDescriptor<EventData>
    {
        public EventsUIDescriptor()
            : base("fullstack-event")
        {
            CommandIconClass = "fullstack-event";
            IsPrimaryType = true;
            ContainerTypes = new[]
            {
                typeof (FullStackBaseData)
            };
        }
    }
}