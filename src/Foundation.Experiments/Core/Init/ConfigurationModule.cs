using System;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Experiments.Core.Config;
using Foundation.Experiments.Core.Impl;
using Foundation.Experiments.Core.Interfaces;
using Foundation.Experiments.Rest;
using OptimizelySDK.Logger;

namespace Foundation.Experiments.Core.Init
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ConfigurationModule : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.ConfigurationComplete += Context_ConfigurationComplete;
        }

        private void Context_ConfigurationComplete(object sender, ServiceConfigurationEventArgs e)
        {
            var services = e.Services;
            services.AddTransient<IUserRetriever, DefaultUserRetriever>();
            services.AddTransient<IExperimentationFactory, DefaultExperimentationFactory>();
            services.AddSingleton<IExperimentationClient, ExperimentationClient>();
            services.AddTransient<ILogger, DefaultExperimentationErrorLogger>();
            services.AddSingleton(x => GetProjectConfigManager(x));
        }

        public ExperimentationProjectConfigManager GetProjectConfigManager(IServiceLocator s)
        {
            var options = s.GetInstance<ExperimentationOptions>();
            var errorLogger = s.GetInstance<ILogger>();

            var projectConfigManager =
                new ExperimentationProjectConfigManager.Builder()
                    .WithSdkKey(options.SdkKey)
                    .WithAutoUpdate(true)
                    .WithLogger(errorLogger);

            if (options.DataFilePollingIntervallInSeconds > 0)
                projectConfigManager = projectConfigManager.WithPollingInterval(TimeSpan.FromSeconds(options.DataFilePollingIntervallInSeconds));

            if (options.BlockingTimeoutPeriodInSeconds > 0)
                projectConfigManager = projectConfigManager.WithBlockingTimeoutPeriod(TimeSpan.FromSeconds(options.BlockingTimeoutPeriodInSeconds));

            var projectConfig = projectConfigManager.Build();
            return projectConfig;
        }

        public void Initialize(InitializationEngine context) { }

        public void Uninitialize(InitializationEngine context) { }
    }
}