using Optimizely.DeveloperFullStack.REST.Models.Attributes;
using Optimizely.DeveloperFullStack.REST.Models.Audiences;
using Optimizely.DeveloperFullStack.REST.Models.Environments;
using Optimizely.DeveloperFullStack.REST.Models.Events;
using Optimizely.DeveloperFullStack.REST.Models.Experiments;
using Optimizely.DeveloperFullStack.REST.Models.Features;
using Optimizely.DeveloperFullStack.REST.Models.Flags;
using Optimizely.DeveloperFullStack.REST.Models.Projects;
using Optimizely.DeveloperFullStack.REST.Models.Variations;
using System.Collections.Generic;

namespace Optimizely.DeveloperFullStack.Core
{
    public interface IOptimizelyFullStackRepository
    {
        #region Audience

        List<Audience> GetAudiences();

        Audience GetAudience(string id);

        Audience SaveAudience(SaveAudienceRequest saveAudienceRequest);

        #endregion Audience

        #region Attribute

        Attribute GetAttribute(string attributeId);

        List<Attribute> GetAttributes(int perPage = 20, int page = 1);

        Attribute SaveAttribute(SaveAttributeRequest attributeRequest);

        #endregion Attribute

        #region Enviroment

        Environment GetEnvironment(string id);

        List<Environment> GetEnvironments();

        #endregion Enviroment

        #region Event

        Event GetEvent(string id);

        List<Event> GetEvents();

        #endregion Event

        #region Experiments

        Experiment GetExperiment(string id);

        List<Experiment> GetExperiments();

        #endregion Experiments

        #region Features

        Feature GetFeature(string id);

        List<Feature> GetFeatures();

        #endregion Features

        #region Flags

        Flag GetFlag(string flagKey);

        FlagList GetFlags(int perPage = 20);

        #endregion Flags

        #region Projects

        Project GetProject(string id);

        List<Project> GetProjects();

        #endregion Projects

        #region Variation

        VariationList GetVariations(string flagKey, int perPage = 20);

        VariationItem GetVariation(string flagKey, string variationKey);

        VariationItem GetVariation(string uri);

        #endregion Variation
    }
}