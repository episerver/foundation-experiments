using Newtonsoft.Json;

namespace Optimizely.DeveloperFullStack.REST.Models.Environments
{
    public class Environment : FullStackBase
    {
        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("datafile")]
        public DataFile Datafile { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("has_restricted_permissions")]
        public bool HasRestrictedPermissions { get; set; }

        [JsonProperty("is_primary")]
        public bool IsPrimary { get; set; }

        [JsonProperty("priority")]
        public long Priority { get; set; }
    }
}