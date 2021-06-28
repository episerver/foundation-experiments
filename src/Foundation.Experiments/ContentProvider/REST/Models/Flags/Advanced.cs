using Newtonsoft.Json;
using System;

namespace Optimizely.DeveloperFullStack.REST.Models.Flags
{
    public partial class Advanced
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("default_value")]
        public string DefaultValue { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("created_time")]
        public DateTimeOffset CreatedTime { get; set; }

        [JsonProperty("updated_time")]
        public DateTimeOffset UpdatedTime { get; set; }
    }
}