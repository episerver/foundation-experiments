using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Optimizely.DeveloperFullStack.REST.Models.Flags
{
    public partial class FlagEnvironment
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        public Dictionary<string, List<string>> Rules { get; set; } = new Dictionary<string, List<string>>();

        [JsonProperty("enable_url")]
        public string EnableUrl { get; set; }

        [JsonProperty("disable_url")]
        public string DisableUrl { get; set; }

        [JsonProperty("rules_summary")]
        private RulesSummary _RuleSummary { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_RuleSummary.Rules.Any())
            {
                Rules = _RuleSummary.Rules;
            }
        }
    }
}