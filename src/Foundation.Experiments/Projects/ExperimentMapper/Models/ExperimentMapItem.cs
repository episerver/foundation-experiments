using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Experiments.Projects.ExperimentMapper.SelectionFactories;

namespace Foundation.Experiments.Projects.ExperimentMapper.Models
{
    public class ExperimentMapItem
    {
        [Display(Name = "Experiment variation")]
        [ReadOnly(true)]
        [Required]
        public virtual string VariationKey { get; set; }

        [Display(Name = "Experiment Description")]
        [ReadOnly(true)]
        public virtual string VariationDescription { get; set; }

        [Display(Name = "Project")]
        [SelectOne(SelectionFactoryType = typeof(ProjectsSelectionFactory))]
        public virtual string ProjectId { get; set; }
    }
}