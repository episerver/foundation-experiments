using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.DeveloperFullStack.Models
{
    [ContentType(DisplayName = "Event", AvailableInEditMode = false, GUID = "ac825115-e159-4dcf-9777-4a6fb8b7af45", Description = "")]
    public class EventData : FullStackBaseData
    {
        [Display(Name = "Event Type", Order = 40)]
        public virtual string EventType { get; set; }

        [Display(Name = "Category", Order = 50)]
        public virtual string CategoryName { get; set; }
    }
}