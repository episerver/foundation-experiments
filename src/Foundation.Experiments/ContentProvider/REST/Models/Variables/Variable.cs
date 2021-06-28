using Newtonsoft.Json;

namespace Optimizely.DeveloperFullStack.REST.Models.Variables
{
    public partial class Variable
    {
        [JsonProperty("default_value")]
        public string DefaultValue { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}