using Newtonsoft.Json;

namespace Optimizely.DeveloperFullStack.REST.Models.Audiences
{
    public class SaveAudienceRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("conditions")]
        public string Conditions { get; set; } = "[]";

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("is_classic")]
        public bool IsClassic { get; set; }

        [JsonProperty("segmentation")]
        public bool Segmentation { get; set; }
    }
}