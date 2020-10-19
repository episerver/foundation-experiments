using System;
using Newtonsoft.Json;

namespace Foundation.Experiments.Core.Impl.Models
{
    public class OptiAttribute
    {
        [JsonProperty("archived")]
        public bool Archived { get; set; }
        [JsonProperty("condition_type")]
        public string ConditionType { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("project_id")]
        public ulong ProjectId { get; set; }
    }
}
/*
https://library.optimizely.com/docs/api/app/v2/index.html#operation/list_attributes
 */
