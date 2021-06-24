using Newtonsoft.Json;

namespace Optimizely.DeveloperFullStack.REST.Models.Projects
{
    public partial class WebSnippet
    {
        [JsonProperty("code_revision")]
        public long CodeRevision { get; set; }

        [JsonProperty("enable_force_variation")]
        public bool EnableForceVariation { get; set; }

        [JsonProperty("exclude_disabled_experiments")]
        public bool ExcludeDisabledExperiments { get; set; }

        [JsonProperty("exclude_names")]
        public bool ExcludeNames { get; set; }

        [JsonProperty("include_jquery")]
        public bool IncludeJquery { get; set; }

        [JsonProperty("ip_anonymization")]
        public bool IpAnonymization { get; set; }

        [JsonProperty("js_file_size")]
        public long JsFileSize { get; set; }

        [JsonProperty("library")]
        public string Library { get; set; }
    }
}