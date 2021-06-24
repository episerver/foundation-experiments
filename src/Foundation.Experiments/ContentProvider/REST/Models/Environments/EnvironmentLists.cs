using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Optimizely.DeveloperFullStack.REST.Models.Flags;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Optimizely.DeveloperFullStack.REST.Models.Environments
{
    public class EnvironmentLists
    {
        [JsonExtensionData]
        private Dictionary<string, JToken> _additionalData { get; set; }

        public Dictionary<string, FlagEnvironment> Environments { get; set; } = new Dictionary<string, FlagEnvironment>();

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_additionalData.Keys.Any())
            {
                foreach (var key in _additionalData.Keys)
                {
                    Environments.Add(key, _additionalData[key].ToObject<FlagEnvironment>());
                }
            }
        }
    }
}