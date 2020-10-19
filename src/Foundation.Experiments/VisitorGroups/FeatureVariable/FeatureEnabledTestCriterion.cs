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
    public class FeatureEnabledTestCriterion : CriterionBase<FeatureVariableCriterionModel>
    {
        private static readonly Lazy<IUserRetriever> UserRetriever = new Lazy<IUserRetriever>(() => ServiceLocator.Current.GetInstance<IUserRetriever>());
        private static readonly Lazy<IExperimentationFactory> ExperimentationFactory = new Lazy<IExperimentationFactory>(() => ServiceLocator.Current.GetInstance<IExperimentationFactory>());

        public override bool IsMatch(IPrincipal principal, HttpContextBase httpContext)
        {
            if (UserRetriever.Value.GetUserId() != string.Empty)
            {
                if (ExperimentationFactory.Value.Instance
                    .IsFeatureEnabled(Model.Feature, UserRetriever.Value.GetUserId()))
                {
                    var allVars = ExperimentationFactory.Value.Instance
                        .GetAllFeatureVariables(Model.Feature,
                            UserRetriever.Value.GetUserId(),
                            UserRetriever.Value.GetUserAttributes());
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