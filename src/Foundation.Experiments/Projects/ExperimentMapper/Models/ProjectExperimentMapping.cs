using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Experiments.Projects.ExperimentMapper.SelectionFactories;

namespace Foundation.Experiments.Projects.ExperimentMapper.Models
{
    [ContentType(
        DisplayName = "Project Experiment Mapping",
        Description = "Maps experiment variations to projects which are used as part of the experiment",
        GUID = "7BFD2720-472A-4C23-BD53-E27253EDF204",
        GroupName = "Experiments",
        AvailableInEditMode = true)]
    [ImageUrl("../Foundation.Experiments/Projects-test.png")]
    public class ProjectExperimentMapping : BlockData
    {
        [Display(Name = "Experiment name",
            GroupName = "Experiment mapping",
            Order = 10)]
        [Required]
        [SelectOne(SelectionFactoryType = typeof(AbExperimentsSelectionFactory))]
        public virtual string ExperimentKey { get; set; }

        [Display(Name = "Experiment variation mapping",
            Description = "Maps a experiment variation to a project Id",
            GroupName = "Experiment mapping",
            Order = 20)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ExperimentMapItem>))]
        public virtual IList<ExperimentMapItem> ExperimentMapping { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
        }
    }
}