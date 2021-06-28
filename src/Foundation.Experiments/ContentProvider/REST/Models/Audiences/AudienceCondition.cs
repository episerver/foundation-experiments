using Newtonsoft.Json;

namespace Optimizely.DeveloperFullStack.REST.Models.Audiences
{
    public class AudienceCondition
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("value", Required = Required.Always)]
        public string Value { get; set; }

        [JsonProperty("match_type", Required = Required.Always)]
        public string MatchType { get; set; } = "exact";

        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; } = "custom_attribute";
    }
}