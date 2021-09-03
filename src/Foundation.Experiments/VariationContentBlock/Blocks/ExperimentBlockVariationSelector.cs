using EPiServer;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Experiments.Core.Interfaces;
using Optimizely.DeveloperFullStack.Initialization;
using System.Web;

namespace Foundation.Experiments.ExperimentContentArea.Blocks
{
    [ModuleDependency(typeof(ConfigurationModule))]
    public class ProjectDependencyInit : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.ConfigurationComplete += Context_ConfigurationComplete;
        }

        private void Context_ConfigurationComplete(object sender, ServiceConfigurationEventArgs e)
        {
            var services = e.Services;
            services.AddTransient<IExperimentBlockVariationSelector, ExperimentBlockVariationSelector>();
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }

    public interface IExperimentBlockVariationSelector
    {
        ExperimentVariationResponse GetVariationContent(HttpContextBase httpContext, ExperimentContainer experimentBlock);
    }

    public class ExperimentBlockVariationSelector : IExperimentBlockVariationSelector
    {
        private readonly IExperimentationFactory _experimentationFactory;

        private readonly IUserRetriever _userRetriever;

        private readonly IContentLoader _contentLoader;

        public ExperimentBlockVariationSelector(IExperimentationFactory experimentationFactory, IUserRetriever userRetriever, IContentLoader contentLoader)
        {
            _experimentationFactory = experimentationFactory;
            _userRetriever = userRetriever;
            this._contentLoader = contentLoader;
        }

        public ExperimentVariationResponse GetVariationContent(HttpContextBase httpContext, ExperimentContainer experimentContainer)
        {
            var experimentVariationResponse = new ExperimentVariationResponse
            {
                ContentArea = experimentContainer.DefaultContentArea
            };

            if (PageEditing.PageIsInEditMode)
            {
                return experimentVariationResponse;
            }

            var userId = _userRetriever.GetUserId(httpContext);
            if (userId == string.Empty)
            {
                return null;
            }

            var userAttributes = this._userRetriever.GetUserAttributes(httpContext);
            userAttributes.Add("geo_locale", "NA");
            var experimentKey = experimentContainer.ExperimentKey;

            var context = this._experimentationFactory.Instance.CreateUserContext(userId, userAttributes);
            if (!ContentReference.IsNullOrEmpty(experimentContainer.ForcedVariationContentLink))
            {
                var experimentVariationBlock = this._contentLoader.Get<ExperimentVariationBlock>(experimentContainer.ForcedVariationContentLink);
                var variation = _experimentationFactory.Instance.GetForcedVariation(experimentVariationBlock.VariationKey, userId);

                experimentVariationResponse.Enabled = variation.IsFeatureEnabled;
                experimentVariationResponse.ContentArea = experimentVariationBlock.VariationContent;
                return experimentVariationResponse;
            }

            var decision = context.Decide(experimentKey);
            experimentVariationResponse.Enabled = decision.Enabled;

            if (decision.Enabled)
            {
                experimentVariationResponse.Variables = decision.Variables.ToDictionary();
                foreach (var item in experimentContainer.ExperimentContentArea.FilteredItems)
                {
                    var variation = (ExperimentVariationBlock)item.GetContent();
                    if (variation.VariationKey == decision.VariationKey)
                    {
                        experimentVariationResponse.ContentArea = variation.VariationContent;
                    }
                }
            }

            return experimentVariationResponse;
        }
    }
}