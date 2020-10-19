using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;

namespace Foundation.Experiments.VisitorGroups.FeatureVariable
{
    public class FeatureVariableCriterionModel : CriterionModelBase
    {
        [Required, DojoWidget(SelectionFactoryType = typeof(FeatureFlagsSelectionFactory))]
        public string Feature { get; set; }

        [Required]
        public string Variable { get; set; }

        [Required]
        public string Is { get; set; }

        public override ICriterionModel Copy() => ShallowCopy();
    }
}