using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Optimizely.DeveloperFullStack.REST.Models.Variations
{
    public partial class EnvironmentUsageCount
    {
        [JsonExtensionData]
        private Dictionary<string, JToken> _enviromentusages { get; set; } = new Dictionary<string, JToken>();

        public Dictionary<string, int> Usages { get; set; } = new Dictionary<string, int>();

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (this._enviromentusages.Keys.Any())
                foreach (var key in this._enviromentusages.Keys)
                    this.Usages.Add(key, _enviromentusages[key].ToObject<int>());
        }
    }
}