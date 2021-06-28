using System.Configuration;

namespace Optimizely.DeveloperFullStack
{
    public class OptimizelyFullStackSettings
    {
        static OptimizelyFullStackSettings()
        {
            ProjectId = ConfigurationManager.AppSettings["optimizely:full-stack:projectId"];
            APIVersion = ConfigurationManager.AppSettings["optimizely:full-stack:apiVersion"];
            EnviromentKey = ConfigurationManager.AppSettings["optimizely:full-stack:environment"];
            SDKKey = ConfigurationManager.AppSettings["optimizely:full-stack:sdkkey"];
            if (ConfigurationManager.AppSettings["optimizely:full-stack:cacheinminutes"] != null)
                CacheInMinutes = int.Parse(ConfigurationManager.AppSettings["optimizely:full-stack:cacheinminutes"]);
            var token = ConfigurationManager.AppSettings["optimizely:full-stack:token"];
            if (token.StartsWith("Bearer "))
                RestAuthToken = token;
            else
                RestAuthToken = $"Bearer {token}";
        }

        public static int CacheInMinutes { get; } = 1;

        public static string RestAuthToken { get; }

        public static string ProjectId { get; set; }

        public static string EnviromentKey { get; set; } = "development";

        public static string SDKKey { get; set; }

        public static string APIVersion { get; set; } = "v2";
    }
}