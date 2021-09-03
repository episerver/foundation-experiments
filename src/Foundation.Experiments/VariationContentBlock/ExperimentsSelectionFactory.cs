using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Optimizely.DeveloperFullStack.Core;
using Optimizely.DeveloperFullStack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Experiments.ExperimentContentArea
{
    public class ExperimentsSelectionFactory : ISelectionFactory
    {
#pragma warning disable 649
        private static Lazy<IOptimizelyFullStackContentLoader> _fullStackContentLoader;

#pragma warning restore 649

        public ExperimentsSelectionFactory()
        {
            _fullStackContentLoader = new Lazy<IOptimizelyFullStackContentLoader>(() => ServiceLocator.Current.GetInstance<IOptimizelyFullStackContentLoader>());
        }

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var items = new List<SelectItem>();

            foreach (var experiment in _fullStackContentLoader.Value.GetDescendants<FlagData>()
                .OrderBy(x => x.Name))
            {
                items.Add(new SelectItem() { Text = experiment.Name, Value = experiment.Key });
            }

            return items;
        }
    }
}