using Newtonsoft.Json;
using System.Collections.Generic;

namespace Optimizely.DeveloperFullStack.REST.Models.Projects
{
    public class Project : FullStackBase
    {
        [JsonProperty("account_id")]
        public long AccountId { get; set; }

        [JsonProperty("confidence_threshold")]
        public double ConfidenceThreshold { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("sdks")]
        public List<object> Sdks { get; set; } = new List<object>();

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("web_snippet", NullValueHandling = NullValueHandling.Ignore)]
        public WebSnippet WebSnippet { get; set; } = new WebSnippet();
    }
}