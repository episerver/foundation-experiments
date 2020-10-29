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
        private readonly Lazy<IUserRetriever> _userRetriever;
        private readonly Lazy<IExperimentationFactory> _experimentationFactory;

        public FeatureEnabledTestCriterion()
        {
            _userRetriever = new Lazy<IUserRetriever>(() => ServiceLocator.Current.GetInstance<IUserRetriever>());
            _experimentationFactory = new Lazy<IExperimentationFactory>(() => ServiceLocator.Current.GetInstance<IExperimentationFactory>());
        }

        public override bool IsMatch(IPrincipal principal, HttpContextBase httpContext)
        {
            if (_experimentationFactory.Value.IsConfigured == false)
                return false;

            if (_userRetriever.Value.GetUserId(httpContext) != string.Empty)
            {
                var userAttributes = _userRetriever.Value.GetUserAttributes(httpContext, false);

                return _experimentationFactory.Value.Instance
                    .IsFeatureEnabled(Model.Feature, _userRetriever.Value.GetUserId(httpContext), userAttributes);
            }

            return false;
        }
    }
}
