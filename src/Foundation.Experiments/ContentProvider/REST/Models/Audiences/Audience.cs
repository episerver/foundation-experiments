using Newtonsoft.Json;

namespace Optimizely.DeveloperFullStack.REST.Models.Audiences
{
    public partial class Audience : FullStackBase
    {
        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("conditions")]
        public string Conditions { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("segmentation")]
        public bool Segmentation { get; set; }
    }
}