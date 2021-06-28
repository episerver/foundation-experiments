using Newtonsoft.Json;
using Optimizely.DeveloperFullStack.REST.Models.Environments;
using System;
using System.Collections.Generic;

namespace Optimizely.DeveloperFullStack.REST.Models.Experiments
{
    public partial class Experiment : FullStackBase
    {
        [JsonProperty("allocation_policy")]
        public string AllocationPolicy { get; set; }

        [JsonProperty("audience_conditions")]
        public string AudienceConditions { get; set; }

        [JsonProperty("campaign_id")]
        public long CampaignId { get; set; }

        [JsonProperty("changes")]
        public List<object> Changes { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("earliest")]
        public DateTimeOffset Earliest { get; set; }

        [JsonIgnore]
        public List<SimpleEnvironment> Environments { get; set; } = new List<SimpleEnvironment>();

        [JsonProperty("holdback")]
        public long Holdback { get; set; }

        [JsonProperty("metrics")]
        public List<Metric> Metrics { get; set; }

        [JsonProperty("page_ids")]
        public List<object> PageIds { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("traffic_allocation")]
        public long TrafficAllocation { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("variations")]
        public List<Variation> Variations { get; set; }
    }
}