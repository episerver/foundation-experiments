using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Optimizely.DeveloperFullStack.Models
{
    [ContentType(DisplayName = "Flag Folder", AvailableInEditMode = false, GUID = "fbc5150e-c0cc-483f-bbf0-8a7462d82430", Description = "A folder used to group flags and navigation tree of fullstack objects")]
    [AvailableContentTypes(Availability = Availability.Specific, Include = new[] { typeof(FlagData) })]
    public class FlagFolderData : ContentFolder, IFullStackFolder
    {
    }
}