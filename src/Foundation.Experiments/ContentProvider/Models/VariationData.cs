using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.DeveloperFullStack.Models
{
    [ContentType(DisplayName = "Variation", GUID = "43799e2e-da39-4887-9311-34cd06e0f356", Description = "")]
    public class VariationData : FullStackBaseData
    {
        [Display(Name = "Flag", Order = 10)]
        public virtual string Flag { get; set; }

        [Display(Name = "In-Use", Order = 11)]
        public virtual bool InUse { get; set; }
    }
}