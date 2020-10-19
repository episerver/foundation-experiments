using EPiServer.ServiceLocation;

namespace Foundation.Experiments.Projects.Config
{
    [Options]
    public class ProjectsFeatureVariableNames
    {
        public string ProjectsFeatureKey { get; set; }
        public string ProjectIdVariableKey { get; set; }

        public ProjectsFeatureVariableNames()
        {
            ProjectsFeatureKey = "project_versions";
            ProjectIdVariableKey = "project_id";
        }
    }
}
