using System;
using System.Security.Principal;
using System.Web;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using Foundation.Experiments.Core.Interfaces;

namespace Foundation.Experiments.VisitorGroups.FeatureVariable
{
    [VisitorGroupCriterion(
        Category = "Optimization",
        Description = "Check if a feature variable is a value for the current user",
        DisplayName = "Feature variable"
    )]
    public class FeatureVariableTestCriterion : CriterionBase<FeatureVariableCriterionModel>
    {
        private readonly Lazy<IUserRetriever> _userRetriever;
        private readonly Lazy<IExperimentationFactory> _experimentationFactory;

        public FeatureVariableTestCriterion()
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
                if (_experimentationFactory.Value.Instance
                    .IsFeatureEnabled(Model.Feature, _userRetriever.Value.GetUserId(httpContext)))
                {
                    var allVars = _experimentationFactory.Value.Instance
                        .GetAllFeatureVariables(Model.Feature,
                            _userRetriever.Value.GetUserId(httpContext),
                            _userRetriever.Value.GetUserAttributes(httpContext));
                    foreach (var variable in allVars.ToDictionary())
                    {
                        if (variable.Key == Model.Variable)
                        {
                            return variable.Value.ToString() == Model.Is;
                        }
                    }

                }
            }

            return false;
        }
    }
}