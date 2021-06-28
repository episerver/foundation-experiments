using Newtonsoft.Json;

namespace Optimizely.DeveloperFullStack.REST.Models.Attributes
{
    public class Attribute : FullStackBase
    {
        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("condition_type")]
        public string ConditionType { get; set; } = "custom_attribute";

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}