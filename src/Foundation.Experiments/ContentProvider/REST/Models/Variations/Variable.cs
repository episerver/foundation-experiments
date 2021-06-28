using Newtonsoft.Json;
using System;

namespace Optimizely.DeveloperFullStack.REST.Models.Variations
{
    public class Variable
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public Uri Value { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }
    }
}