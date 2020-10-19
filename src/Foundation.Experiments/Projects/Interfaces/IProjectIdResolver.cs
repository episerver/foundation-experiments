namespace Foundation.Experiments.Projects.Interfaces
{
    public interface IProjectIdResolver
    {
        int? GetProjectId();
        bool ShouldContinue();
    }
}