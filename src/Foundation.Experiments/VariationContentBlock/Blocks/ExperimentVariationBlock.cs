using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Experiments.ExperimentContentArea.Blocks
{
    [ContentType(
        DisplayName = "Experiment Variation",
        Description = "A variation of content that appears in a content area",
        GUID = "EF35A4CF-B9E8-4EED-B85E-A18FEB82C707",
        GroupName = "Experiments",
        AvailableInEditMode = true)]
    [ImageUrl("../Foundation.Experiments/Projects-test.png")]
    public class ExperimentVariationBlock : BlockData
    {
        [Display(Name = "Experiment variation")]
        [ScaffoldColumn(false)]
        public virtual string VariationDescription { get; set; }

        [Display(Name = "Experiment variation",
            GroupName = SystemTabNames.Content,
            Order = 250)]
        public virtual ContentArea VariationContent { get; set; }

        [ScaffoldColumn(false)]
        public virtual string VariationKey { get; set; }
    }
}