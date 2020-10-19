using OptimizelySDK;

namespace Foundation.Experiments.Core.Interfaces
{
    public interface IExperimentationFactory
    {
        Optimizely Instance { get; }
        bool IsConfigured { get; }
    }
}