using System.Collections.Generic;
using System.Linq;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;

namespace Foundation.Experiments.Projects.ExperimentMapper.SelectionFactories
{
    public class ProjectsSelectionFactory : ISelectionFactory
    {
#pragma warning disable 649
        private Injected<ProjectRepository> _projectRepository;
#pragma warning restore 649

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var items = new List<SelectItem>();

            foreach (var project in _projectRepository.Service.List().OrderBy(x => x.Name))
            {
                items.Add(new SelectItem() { Text = project.Name, Value = project.ID.ToString() });
            }

            return items;
        }
    }
}