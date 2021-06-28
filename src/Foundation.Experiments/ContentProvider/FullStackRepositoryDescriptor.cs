using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using Optimizely.DeveloperFullStack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimizely.DeveloperFullStack
{
    [ServiceConfiguration(typeof(IContentRepositoryDescriptor))]
    public class AprimoRepositoryDescriptor : ContentRepositoryDescriptorBase
    {
        private readonly IContentProviderManager providerManager;

        public AprimoRepositoryDescriptor(IContentProviderManager providerManager)
        {
            this.providerManager = providerManager;
        }

        public override IEnumerable<ContentReference> Roots =>
            new ContentReference[] { this.providerManager.GetProvider(FullStackConstants.RepositoryKey).EntryPoint };

        public override string Key =>
            FullStackConstants.RepositoryKey;

        public override string Name =>
            "Optimizely Fullstack";

        public override string SearchArea =>
            FullStackConstants.RepositoryKey;

        public override IEnumerable<Type> ContainedTypes =>
            new[] { typeof(FullStackBaseData), typeof(VariableDefinitionData) };

        public override IEnumerable<Type> CreatableTypes =>
            Enumerable.Empty<Type>();

        public override IEnumerable<Type> MainNavigationTypes =>
            new[] { typeof(ContentFolder), typeof(FlagData) };
    }
}