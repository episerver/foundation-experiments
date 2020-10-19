using System.Collections.Generic;
using System.Linq;
using EPiServer.Shell.ObjectEditing;
using Foundation.Experiments.Rest;

namespace Foundation.Experiments.Projects.SelectionFactories
{
    public class ExperimentsSelectionFactory : ISelectionFactory
    {
        private readonly IExperimentationClient _experimentationClient;

        public ExperimentsSelectionFactory(IExperimentationClient experimentationClient)
        {
            _experimentationClient = experimentationClient;
        }

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var items = new List<SelectItem>();

            foreach (var experiment in _experimentationClient.GetExperimentList().OrderBy(x => x.Name))
            {
                items.Add(new SelectItem() { Text = experiment.Name, Value = experiment.Key });
            }

            return items;
        }
    }
}