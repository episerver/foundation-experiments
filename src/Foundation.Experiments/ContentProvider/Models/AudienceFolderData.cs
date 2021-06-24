using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Optimizely.DeveloperFullStack.Models
{
    [ContentType(DisplayName = "Audience Folder", AvailableInEditMode = false, GUID = "7464e728-d568-40e2-baec-61ffdced184d", Description = "A folder used to group audiences and navigation tree of fullstack objects")]
    [AvailableContentTypes(Availability = Availability.Specific, Include = new[] { typeof(AudienceData) })]
    public class AudienceFolderData : ContentFolder, IFullStackFolder
    {
    }
}