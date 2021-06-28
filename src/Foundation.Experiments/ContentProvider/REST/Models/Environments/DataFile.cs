using Newtonsoft.Json;
using System;

namespace Optimizely.DeveloperFullStack.REST.Models.Environments
{
    public class DataFile
    {
        [JsonProperty("latest_file_size")]
        public long LatestFileSize { get; set; }

        [JsonProperty("ip_anonymization")]
        public bool IpAnonymization { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("other_urls")]
        public Uri[] OtherUrls { get; set; }

        [JsonProperty("sdk_key")]
        public string SdkKey { get; set; }

        [JsonProperty("cache_ttl")]
        public long CacheTtl { get; set; }

        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }

        [JsonProperty("revision")]
        public long Revision { get; set; }
    }
}