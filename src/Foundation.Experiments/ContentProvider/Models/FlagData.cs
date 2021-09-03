using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.DeveloperFullStack.Models
{
    [ContentType(DisplayName = "Flag", AvailableInEditMode = false, GUID = "b470d11f-edbf-47f8-9045-37529a493443", Description = "", Order = 2)]
    [AvailableContentTypes(Include = new[] { typeof(ExperimentData), typeof(VariableDefinitionData), typeof(VariationData) })]
    public class FlagData : FullStackBaseData, IFullStackFolder
    {
        [Display(Name = "Urn", Order = 6)]
        [ScaffoldColumn(false)]
        public virtual string Urn { get; set; }

        [Display(Name = "Url", Order = 7)]
        [ScaffoldColumn(false)]
        public virtual string Url { get; set; }
    }
}