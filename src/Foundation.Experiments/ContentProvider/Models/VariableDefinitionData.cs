using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.DeveloperFullStack.Models
{
    [ContentType(DisplayName = "Variable Definition", AvailableInEditMode = false, GUID = "233812f3-4c6a-42e4-a52e-ee37f5e8952d", Description = "", Order = 3)]
    public class VariableDefinitionData : BasicContent
    {
        [Display(Name = "Key")]
        public virtual string Key { get; set; }

        [Display(Name = "Type")]
        public virtual string Type { get; set; }

        [Display(Name = "Default Value")]
        public virtual string DefaultValue { get; set; }

        [Display(Name = "Description")]
        public virtual string Description { get; set; }
    }
}