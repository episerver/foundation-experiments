namespace Optimizely.DeveloperFullStack.Models
{
    public interface IFullStackData
    {
        string FullStackId { get; set; }

        string ProjectId { get; set; }

        bool Enabled { get; set; }
    }
}