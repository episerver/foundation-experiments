using Newtonsoft.Json;

namespace Optimizely.DeveloperFullStack.REST.Models.Events
{
    public partial class Event : FullStackBase
    {
        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("event_type")]
        public string EventType { get; set; }
    }
}