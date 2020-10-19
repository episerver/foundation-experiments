using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Foundation.Experiments.Core.Impl.Models
{
    public class OptiFeature
    {
        [JsonProperty("archived")]
        public bool Archived { get; set; }
        [JsonProperty("created")]
        public DateTime Created { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        //[JsonProperty("environments")]
        //public Environments EnvironmentList { get; set; }
        [JsonProperty("environments")]
        public Dictionary<string, Environment> Environments { get; set; } = new Dictionary<string, Environment>();
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("project_id")]
        public ulong ProjectId { get; set; }
        [JsonProperty("variables")]
        public List<Variable> Variables { get; set; }

        public class RolloutRule
        {
            [JsonProperty("audience_conditions")]
            public string AudienceConditions { get; set; }
            [JsonProperty("enabled")]
            public bool Enabled { get; set; }
            [JsonProperty("percentage_included")]
            public int PercentageIncluded { get; set; }
        }

        public class Environment
        {
            [JsonProperty("id")]
            public ulong Id { get; set; }
            [JsonProperty("is_primary")]
            public bool IsPrimary { get; set; }
            [JsonProperty("rollout_rules")]
            public List<RolloutRule> RolloutRules { get; set; }
        }

        public class Variable
        {
            [JsonProperty("archived")]
            public bool Archived { get; set; }
            [JsonProperty("default_value")]
            public string DefaultValue { get; set; }
            [JsonProperty("id")]
            public ulong Id { get; set; }
            [JsonProperty("key")]
            public string Key { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
        }

    }
}
/*
 * // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Environments    {
    }

    public class Variable    {
        public bool archived { get; set; } 
        public string default_value { get; set; } 
        public int id { get; set; } 
        public string key { get; set; } 
        public string type { get; set; } 
    }

    public class MyArray    {
        public bool archived { get; set; } 
        public DateTime created { get; set; } 
        public string description { get; set; } 
        public Environments environments { get; set; } 
        public int id { get; set; } 
        public string key { get; set; } 
        public DateTime last_modified { get; set; } 
        public string name { get; set; } 
        public int project_id { get; set; } 
        public List<Variable> variables { get; set; } 
    }

    public class Root    {
        public List<MyArray> MyArray { get; set; } 
    }


 */
