using EPiServer.Core;

namespace Foundation.Experiments.Projects.Interfaces
{
    /// <summary>
    /// Helps identify project and version in the anonymous context
    /// </summary>
    public interface IExperimentProjectIdentifier
    {
        /// <summary>
        /// Gets the project version of the content, otherwise returns the published reference.
        /// </summary>
        /// <param name="publishedContentReference"></param>
        /// <returns>A <see cref="ContentReference"/></returns>
        ContentReference GetProjectVersion(ContentReference publishedContentReference);

        ContentReference GetProjectReference(ContentReference publishedReference, int projectId);

        void InvalidateCache(string projectId);
    }
}