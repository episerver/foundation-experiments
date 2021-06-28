using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Optimizely.DeveloperFullStack.REST.Models.Flags
{
    public partial class VariableDefinitions
    {
        [JsonExtensionData]
        private Dictionary<string, JToken> _additionalData { get; set; } = new Dictionary<string, JToken>();

        public Dictionary<string, Advanced> Definitions { get; set; } = new Dictionary<string, Advanced>();

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_additionalData.Keys.Any())
            {
                foreach (var key in _additionalData.Keys)
                {
                    Definitions.Add(key, _additionalData[key].ToObject<Advanced>());
                }
            }
        }
    }
}