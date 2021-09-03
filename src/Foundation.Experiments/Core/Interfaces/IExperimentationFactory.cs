using OptimizelySDK;

namespace Foundation.Experiments.Core.Interfaces
{
    public interface IExperimentationFactory
    {
        OptimizelySDK.Optimizely Instance { get; }

        bool IsConfigured { get; }
    }
}