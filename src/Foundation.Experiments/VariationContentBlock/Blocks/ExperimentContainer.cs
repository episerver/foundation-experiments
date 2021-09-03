using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Experiments.ExperimentContentArea.Blocks
{
    [ContentType(
        DisplayName = "Experiment Container",
        Description = "A container to hold one more more content items for each variation in an experiment",
        GUID = "337B5020-CDED-415F-BCD1-5364BDB09D6D",
        GroupName = "Experiments",
        AvailableInEditMode = true)]
    [ImageUrl("../Foundation.Experiments/Projects-test.png")]
    public class ExperimentContainer : BlockData
    {
        [Display(Name = "Experiment name", Order = 10)]
        [SelectOne(SelectionFactoryType = typeof(ExperimentsSelectionFactory))]
        public virtual string ExperimentKey { get; set; }

        [Display(Name = "Set Forced Variation", Order = 15)]
        [AllowedTypes(AllowedTypes = new[] { typeof(ExperimentVariationBlock) })]
        public virtual ContentReference ForcedVariationContentLink { get; set; }

        [Display(Name = "Default content", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual ContentArea DefaultContentArea { get; set; }

        [Display(Name = "Experiment variations", GroupName = SystemTabNames.Content, Order = 30)]
        [AllowedTypes(AllowedTypes = new[] { typeof(ExperimentVariationBlock) })]
        public virtual ContentArea ExperimentContentArea { get; set; }

        [Display(Name = "Content area used for display", GroupName = SystemTabNames.Content, Order = 200)]
        [ScaffoldColumn(false)]
        public virtual ContentArea DisplayContentArea { get; set; }
    }
}