using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Optimizely.DeveloperFullStack.REST.Models.Variations
{
    public partial class VariableList
    {
        [JsonExtensionData]
        private Dictionary<string, JToken> _variables { get; set; } = new Dictionary<string, JToken>();

        public Dictionary<string, Variable> Variables { get; set; } = new Dictionary<string, Variable>();

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_variables.Keys.Any())
                foreach (var key in this._variables.Keys)
                    Variables.Add(key, _variables[key].ToObject<Variable>());
        }
    }
}