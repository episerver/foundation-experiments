using Newtonsoft.Json;

namespace Optimizely.DeveloperFullStack.REST.Models.Environments
{
    public class SimpleEnvironment
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("environment_name")]
        public string EnvironmentName { get; set; }

        [JsonProperty("environment_id")]
        public long EnvironmentId { get; set; }

        [JsonProperty("percentage_included")]
        public object PercentageIncluded { get; set; }
    }
}