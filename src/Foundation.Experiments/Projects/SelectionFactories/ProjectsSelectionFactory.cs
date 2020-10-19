using System.Collections.Generic;
using System.Linq;
using EPiServer.DataAbstraction;
using EPiServer.Shell.ObjectEditing;

namespace Foundation.Experiments.Projects.SelectionFactories
{
    public class ProjectsSelectionFactory : ISelectionFactory
    {
        private readonly ProjectRepository _projectRepository;

        public ProjectsSelectionFactory(ProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var items = new List<SelectItem>();

            foreach (var project in _projectRepository.List().OrderBy(x => x.Name))
            {
                items.Add(new SelectItem() { Text = project.Name, Value = project.ID.ToString() });
            }

            return items;
        }
    }
}
