using Newtonsoft.Json;
using Optimizely.DeveloperFullStack.REST.Models.Variables;
using System.Collections.Generic;

namespace Optimizely.DeveloperFullStack.REST.Models.Features
{
    public class Feature : FullStackBase
    {
        [JsonProperty("archived")]
        public bool Archived { get; set; }

        //[JsonProperty("environments")]
        //public Environments Environments { get; set; } = new Environments();

        [JsonProperty("variables")]
        public List<Variable> Variables { get; set; } = new List<Variable>();
    }
}