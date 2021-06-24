using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Optimizely.DeveloperFullStack.Models
{
    [ContentType(DisplayName = "Event Folder", AvailableInEditMode = false, GUID = "7879f50d-8ffe-42ce-9000-9eb7fb9d3876", Description = "A folder used to group event and navigation tree of fullstack objects")]
    [AvailableContentTypes(Availability = Availability.Specific, Include = new[] { typeof(EventData) })]
    public class EventFolderData : ContentFolder, IFullStackFolder
    {
    }
}