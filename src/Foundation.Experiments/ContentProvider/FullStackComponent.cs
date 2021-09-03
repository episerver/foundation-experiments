using EPiServer.Shell;
using EPiServer.Shell.ViewComposition;
using Optimizely.DeveloperFullStack.Models;

namespace Optimizely.DeveloperFullStack
{
    [Component]
    public class FullStackComponent : ComponentDefinitionBase
    {
        public FullStackComponent()
            : base("epi-cms/component/Media")
        {
            Title = "Opti Full Stack";
            Description = "Allows you to connect to Optimizely Fullstack.";
            Categories = new string[] { "content" };
            PlugInAreas = new[] { PlugInArea.AssetsDefaultGroup };
            Settings.Add(new Setting("repositoryKey", FullStackConstants.RepositoryKey));
            Settings.Add(new Setting("allowedTypes", new[] { typeof(IFullStackFolder), typeof(FullStackBaseData) }));
            SortOrder = 900;
        }
    }
}