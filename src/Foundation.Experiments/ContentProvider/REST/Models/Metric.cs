using Newtonsoft.Json;

namespace Optimizely.DeveloperFullStack.REST.Models
{
    public class Metric
    {
        [JsonProperty("aggregator")]
        public string Aggregator { get; set; }

        [JsonProperty("event_id")]
        public long EventId { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("winning_direction")]
        public string WinningDirection { get; set; }
    }
}