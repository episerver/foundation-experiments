using Newtonsoft.Json;
using System;

namespace Optimizely.DeveloperFullStack.REST.Models
{
    public abstract class FullStackBase : OptimizelyFullStackModelBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("last_modified")]
        public DateTimeOffset LastModified { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("is_classic")]
        public bool IsClassic { get; set; }
    }
}