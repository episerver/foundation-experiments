using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Optimizely.DeveloperFullStack.Enums;
using System.Collections.Generic;

namespace Optimizely.DeveloperFullStack.Models
{
    [ContentType(DisplayName = "Experiment", AvailableInEditMode = false, GUID = "944bb4ed-435e-4edd-b6f8-d988f666d0af", Description = "You can build an A/B test on a feature flag. The simplest kind of A/B test shows one cohort of users the flag -- and compares performance against a control cohort that doesn't see the flag.")]
    public class ExperimentData : FullStackBaseData
    {
        [BackingType(typeof(PropertyNumber))]
        public virtual RuleType RuleType { get; set; }

        public virtual IList<string> Keys { get; set; }
    }
}