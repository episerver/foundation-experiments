using System;
using System.Security.Principal;
using System.Web;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using Foundation.Experiments.Core.Interfaces;

namespace Foundation.Experiments.VisitorGroups.FeatureFlag
{
    [VisitorGroupCriterion(
        Category = "Optimization",
        Description = "Check if a feature has been enabled for the current user",
        DisplayName = "Feature enabled"
    )]
    public class FeatureEnabledTestCriterion : CriterionBase<FeatureFlagsCriterionModel>
    {
        private static readonly Lazy<IUserRetriever> UserRetriever = new Lazy<IUserRetriever>(() => ServiceLocator.Current.GetInstance<IUserRetriever>());
        private static readonly Lazy<IExperimentationFactory> ExperimentationFactory = new Lazy<IExperimentationFactory>(() => ServiceLocator.Current.GetInstance<IExperimentationFactory>());

        public override bool IsMatch(IPrincipal principal, HttpContextBase httpContext)
        {
            if (UserRetriever.Value.GetUserId() != string.Empty)
            {
                var userAttributes = UserRetriever.Value.GetUserAttributes();

                return ExperimentationFactory.Value.Instance
                    .IsFeatureEnabled(Model.Feature, UserRetriever.Value.GetUserId(), userAttributes);
            }

            return false;
        }
    }
}
