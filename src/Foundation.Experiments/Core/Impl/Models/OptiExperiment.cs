using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Foundation.Experiments.Core.Impl.Models
{
    public class OptiExperiment
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")] 
        public string Description { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("status")] 
        public string Status { get; set; }
        [JsonProperty("created")] 
        public DateTime Created { get; set; }
        [JsonProperty("variations")]
        public IList<OptiVariation> Variations { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class OptiVariation
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}