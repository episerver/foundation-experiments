using Newtonsoft.Json;
using Optimizely.DeveloperFullStack.REST.Models.Environments;
using System;

namespace Optimizely.DeveloperFullStack.REST.Models.Flags
{
    public class Flag : FullStackBase
    {
        [JsonProperty("urn")]
        public string Urn { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("variable_definitions")]
        public VariableDefinitions VariableDefinitions { get; set; } = new VariableDefinitions();

        [JsonProperty("account_id")]
        public long AccountId { get; set; }

        [JsonProperty("created_time")]
        public DateTimeOffset CreatedTime { get; set; }

        [JsonProperty("updated_time")]
        public DateTimeOffset UpdatedTime { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("environments")]
        public EnvironmentLists Enviroments { get; set; } = new EnvironmentLists();

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("update_url")]
        public string UpdateUrl { get; set; }

        [JsonProperty("archive_url")]
        public string ArchiveUrl { get; set; }

        [JsonProperty("unarchive_url")]
        public string UnarchiveUrl { get; set; }

        [JsonProperty("delete_url")]
        public string DeleteUrl { get; set; }

        [JsonProperty("outlier_filtering_enabled")]
        public bool OutlierFilteringEnabled { get; set; }

        [JsonProperty("revision")]
        public long Revision { get; set; }
    }
}