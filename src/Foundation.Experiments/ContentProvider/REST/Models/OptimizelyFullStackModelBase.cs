using Newtonsoft.Json;

namespace Optimizely.DeveloperFullStack.REST.Models
{
    public abstract class OptimizelyFullStackModelBase
    {
        [JsonProperty("id")]
        public long Id { get; set; }
    }
}