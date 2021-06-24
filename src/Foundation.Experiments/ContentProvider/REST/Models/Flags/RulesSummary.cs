using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Optimizely.DeveloperFullStack.REST.Models.Flags
{
    public partial class RulesSummary
    {
        [JsonExtensionData]
        private Dictionary<string, JToken> _additionalData { get; set; } = new Dictionary<string, JToken>();

        public Dictionary<string, List<string>> Rules { get; set; } = new Dictionary<string, List<string>>();

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (this._additionalData.Keys.Any())
            {
                foreach (var key in this._additionalData.Keys)
                {
                    var rules = this._additionalData[key].ToObject<Rules>();
                    this.Rules.Add(key, rules.Keys);
                }
            }
        }
    }
}