using Newtonsoft.Json;

namespace Optimizely.DeveloperFullStack.REST.Models.Attributes
{
    public class SaveAttributeRequest
    {
        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }
    }
}