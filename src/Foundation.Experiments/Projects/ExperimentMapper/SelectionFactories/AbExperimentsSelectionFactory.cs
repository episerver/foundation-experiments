using System.Collections.Generic;
using System.Linq;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Foundation.Experiments.Rest;

namespace Foundation.Experiments.Projects.ExperimentMapper.SelectionFactories
{
    public class AbExperimentsSelectionFactory : ISelectionFactory
    {
#pragma warning disable 649
        private Injected<IExperimentationClient> _experimentationClient;
#pragma warning restore 649

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var items = new List<SelectItem>();

            foreach (var experiment in _experimentationClient.Service.GetExperimentList()
                .Where(x => x.Status != "archived" && x.Type == "a/b")
                .OrderBy(x => x.Name))
            {
                items.Add(new SelectItem() { Text = experiment.Name, Value = experiment.Key });
            }

            return items;
        }
    }
}