using System.Web;

namespace Foundation.Experiments.Projects.Interfaces
{
    public interface IProjectIdResolver
    {
        int? GetProjectId(HttpContextBase httpContext);
        bool ShouldContinue(HttpContextBase httpContext);
    }
}