using Newtonsoft.Json;
using System.Collections.Generic;

namespace Optimizely.DeveloperFullStack.REST.Models.Flags
{
    public class FlagList
    {
        [JsonProperty("items")]
        public List<Flag> Flags { get; set; } = new List<Flag>();

        [JsonProperty("fetch_flag_url")]
        public string FetchFlagUrl { get; set; }

        [JsonProperty("last_url")]
        public string LastUrl { get; set; }

        [JsonProperty("create_url")]
        public string CreateUrl { get; set; }

        [JsonProperty("first_url")]
        public string FirstUrl { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}