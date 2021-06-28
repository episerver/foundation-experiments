using Newtonsoft.Json;
using System.Collections.Generic;

namespace Optimizely.DeveloperFullStack.REST.Models.Flags
{
    public partial class Rules
    {
        [JsonProperty("keys")]
        public List<string> Keys { get; set; } = new List<string>();
    }
}