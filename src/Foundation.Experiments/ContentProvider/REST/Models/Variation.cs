using Newtonsoft.Json;
using System.Collections.Generic;

namespace Optimizely.DeveloperFullStack.REST.Models
{
    public class Variation
    {
        [JsonProperty("actions")]
        public List<object> Actions { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("variation_id")]
        public object VariationId { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }
    }
}