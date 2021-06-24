using Newtonsoft.Json;
using System;

namespace Optimizely.DeveloperFullStack.REST.Models.Variations
{
    public partial class VariationItem
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("flag_key")]
        public string FlagKey { get; set; }

        [JsonProperty("environment_usage_count")]
        public EnvironmentUsageCount EnvironmentUsageCount { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("in_use")]
        public bool InUse { get; set; }

        [JsonProperty("created_time")]
        public DateTimeOffset CreatedTime { get; set; }

        [JsonProperty("updated_time")]
        public DateTimeOffset UpdatedTime { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("fetch_flag_url")]
        public string FetchFlagUrl { get; set; }

        [JsonProperty("variables")]
        public VariableList Variables { get; set; } = new VariableList();
    }
}