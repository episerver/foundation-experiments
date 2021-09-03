using Newtonsoft.Json;
using System.Collections.Generic;

namespace Optimizely.DeveloperFullStack.REST.Models.Variations
{
    public class VariationList
    {
        [JsonProperty("total_pages")]
        public long TotalPages { get; set; }

        [JsonProperty("update_url")]
        public string UpdateUrl { get; set; }

        [JsonProperty("last_url")]
        public string LastUrl { get; set; }

        [JsonProperty("create_url")]
        public string CreateUrl { get; set; }

        [JsonProperty("items")]
        public List<Variation> Items { get; set; } = new List<Variation>();

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("fetch_variation_url")]
        public string FetchVariationUrl { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("first_url")]
        public string FirstUrl { get; set; }

        [JsonProperty("total_count")]
        public long TotalCount { get; set; }
    }
}