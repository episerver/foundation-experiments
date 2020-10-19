using System.Collections.Generic;
using Foundation.Experiments.Core.Impl.Models;

namespace Foundation.Experiments.Rest
{
    public interface IExperimentationClient
    {
        bool CreateOrUpdateAttribute(string key, string description = null);
        bool CreateOrUpdateEvent(string key, OptiEvent.Types type = OptiEvent.Types.Other, string description = null);
        bool CreateEventIfNotExists(string key, OptiEvent.Types type = OptiEvent.Types.Other, string description = null);

        List<OptiFeature> GetFeatureList();
        List<OptiAttribute> GetAttributeList();
        List<OptiEvent> GetEventList();
        List<OptiEnvironment> GetEnvironmentList();
        List<OptiExperiment> GetExperimentList();
        OptiExperiment GetExperiment(long experimentId);
        OptiExperiment GetExperiment(string experimentKey);
    }
}
