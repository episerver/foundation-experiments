using EPiServer.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.DeveloperFullStack.Models
{
    public abstract class FullStackBaseData : BasicContent, IFullStackData
    {
        [Display(Order = 1)]
        [ReadOnly(true)]
        public virtual string FullStackId { get; set; }

        [Display(Order = 2)]
        [ReadOnly(true)]
        public virtual string ProjectId { get; set; }

        [Display(Order = 3)]
        [ReadOnly(true)]
        public virtual string Key { get; set; }

        [Display(Order = 4)]
        [ReadOnly(true)]
        public virtual string Description { get; set; }

        [Display(Order = 5)]
        [ReadOnly(true)]
        public virtual bool IsClassic { get; set; }

        [ScaffoldColumn(false)]
        public virtual bool Enabled { get; set; }
    }
}