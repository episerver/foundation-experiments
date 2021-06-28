using EPiServer.ServiceLocation;

namespace Optimizely.DeveloperFullStack.Core
{
    [Options]
    public class FullStackSettingsOptions
    {
        public int CacheInMinutes { get; } = 1;

        public string RestAuthToken { get; }

        public string ProjectId { get; set; }

        public string EnviromentKey { get; set; } = "development";

        public string SDKKey { get; set; }

        public string APIVersion { get; set; } = "v2";
    }
}