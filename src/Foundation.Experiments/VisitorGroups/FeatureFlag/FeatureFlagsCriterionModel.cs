using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;

namespace Foundation.Experiments.VisitorGroups.FeatureFlag
{
    public class FeatureFlagsCriterionModel : CriterionModelBase
    {
        [Required, DojoWidget(SelectionFactoryType = typeof(FeatureFlagsSelectionFactory))]
        public string Feature { get; set; }

        public override ICriterionModel Copy() => ShallowCopy();
    }
}