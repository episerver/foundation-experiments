using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;

namespace Foundation.Experiments.Tracking.TrackingBlock
{
    [ContentType(
        DisplayName = "Experiment Tracking",
        Description = "Defines an event that is tracked as part of an experiment",
        GUID = "301064A6-F20C-4CFC-8B57-AF7C6C64F2F4",
        GroupName = "Experiments",
        AvailableInEditMode = true)]
    [ImageUrl("../Foundation.Experiments/Projects-test.png")]
    public class ExperimentEventTracking : BlockData
    {
        [Display(Name = "Event name",
            Description = "The name of the event to track",
            GroupName = "Tracking event",
            Order = 10)]
        [SelectOne(SelectionFactoryType = typeof(ExperimentEventSelectionFactory))]
        [Required]
        public virtual string EventName { get; set; }
    }
}