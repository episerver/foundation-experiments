using Newtonsoft.Json.Linq;
using Optimizely.DeveloperFullStack.REST.Models.Attributes;
using Optimizely.DeveloperFullStack.REST.Models.Audiences;
using Optimizely.DeveloperFullStack.REST.Models.Environments;
using Optimizely.DeveloperFullStack.REST.Models.Events;
using Optimizely.DeveloperFullStack.REST.Models.Experiments;
using Optimizely.DeveloperFullStack.REST.Models.Features;
using Optimizely.DeveloperFullStack.REST.Models.Flags;
using Optimizely.DeveloperFullStack.REST.Models.Projects;
using Optimizely.DeveloperFullStack.REST.Models.Variations;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System.Collections.Generic;
using System.Linq;

namespace Optimizely.DeveloperFullStack.Core
{
    public class OptimizelyFullStackRepository : IOptimizelyFullStackRepository
    {
        private readonly IRestClient _restClient;

        private readonly FullStackSettingsOptions _fullStackSettingsOptions;

        public OptimizelyFullStackRepository(FullStackSettingsOptions fullStackSettingsOptions)
        {
            _restClient = new RestClient(FullStackConstants.APIBaseUrl);
            _restClient.UseSerializer(() => new JsonNetSerializer());
            _restClient.AddDefaultHeader("Authorization", _fullStackSettingsOptions.RestAuthToken);
            _fullStackSettingsOptions = fullStackSettingsOptions;
        }

        #region Projects

        public List<Project> GetProjects()
        {
            var request = new RestRequest("/{apiVersion}/projects", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion);
            var environments = _restClient.Get<List<Project>>(request);
            return environments.Data;
        }

        public Project GetProject(string id)
        {
            var request = new RestRequest("/{apiVersion}/projects/{id}", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddUrlSegment("id", id);
            var environment = _restClient.Get<Project>(request);
            return environment.Data;
        }

        #endregion Projects

        #region Environments

        public List<Environment> GetEnvironments()
        {
            var request = new RestRequest("/{apiVersion}/environments", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddQueryParameter("project_id", _fullStackSettingsOptions.ProjectId);
            var environments = _restClient.Get<List<Environment>>(request);
            return environments.Data;
        }

        public Environment GetEnvironment(string id)
        {
            var request = new RestRequest("/{apiVersion}/environments/{id}", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddUrlSegment("id", id);
            var environment = _restClient.Get<Environment>(request);
            return environment.Data;
        }

        #endregion Environments

        #region Experiments

        public List<Experiment> GetExperiments()
        {
            var request = new RestRequest("/{apiVersion}/experiments", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddQueryParameter("project_id", _fullStackSettingsOptions.ProjectId);
            var experiments = _restClient.Get<List<Experiment>>(request);
            foreach (var experiment in experiments.Data)
            {
                experiment.Environments = GetSimpleEnvironments(experiments.Content);
            }
            return experiments.Data;
        }

        public Experiment GetExperiment(string id)
        {
            var request = new RestRequest("/{apiVersion}/experiments/{id}", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddUrlSegment("id", id);
            var experiments = _restClient.Get<Experiment>(request);
            return experiments.Data;
        }

        private List<SimpleEnvironment> GetSimpleEnvironments(string json)
        {
            JToken experiment = JArray.Parse(json)[0];
            var environments = from e in experiment["environments"]
                               select e.Value<JProperty>().Value;
            var list = new List<SimpleEnvironment>();
            if (environments.Any())
            {
                foreach (var env in environments)
                    list.Add(env.ToObject<SimpleEnvironment>());
            }

            return list;
        }

        #endregion Experiments

        #region Events

        public List<Event> GetEvents()
        {
            var request = new RestRequest("/{apiVersion}/events", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddQueryParameter("project_id", _fullStackSettingsOptions.ProjectId);
            var events = _restClient.Get<List<Event>>(request);
            return events.Data;
        }

        public Event GetEvent(string id)
        {
            var request = new RestRequest("/{apiVersion}/events/{event_id}", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddUrlSegment("event_id", id);
            var events = _restClient.Get<Event>(request);
            return events.Data;
        }

        #endregion Events

        #region Audiences

        public List<Audience> GetAudiences()
        {
            var request = new RestRequest("/{apiVersion}/audiences", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddQueryParameter("project_id", _fullStackSettingsOptions.ProjectId);
            var audiences = _restClient.Get<List<Audience>>(request);
            return audiences.Data;
        }

        public Audience GetAudience(string id)
        {
            var request = new RestRequest("/{apiVersion}/audiences/{audience_id}", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddUrlSegment("audience_id", id);
            var audience = _restClient.Get<Audience>(request);
            return audience.Data;
        }

        public Audience SaveAudience(SaveAudienceRequest saveAudienceRequest)
        {
            saveAudienceRequest.ProjectId = long.Parse(_fullStackSettingsOptions.ProjectId);
            var request = new RestRequest("/{apiVersion}/audiences", Method.POST)
                   .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                   .AddJsonBody(saveAudienceRequest);

            var audience = _restClient.Post<Audience>(request);
            if (audience.IsSuccessful)
                return audience.Data;

            return null;
        }

        #endregion Audiences

        #region Attributes

        public Attribute GetAttribute(string id)
        {
            var request = new RestRequest("/{apiVersion}/attributes/{attribute_id}", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddUrlSegment("attribute_id", id);

            var attributes = _restClient.Get<Attribute>(request);
            if (attributes.IsSuccessful)
                return attributes.Data;

            return null;
        }

        public List<Attribute> GetAttributes(int perPage = 20, int page = 1)
        {
            var request = new RestRequest("/{apiVersion}/attributes", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddQueryParameter("project_id", _fullStackSettingsOptions.ProjectId)
                .AddQueryParameter("per_page", perPage.ToString())
                .AddQueryParameter("project_id", page.ToString());

            var attributes = _restClient.Get<List<Attribute>>(request);
            if (attributes.IsSuccessful)
                return attributes.Data;

            return new List<Attribute>();
        }

        public Attribute SaveAttribute(SaveAttributeRequest attributeRequest)
        {
            attributeRequest.ProjectId = long.Parse(_fullStackSettingsOptions.ProjectId);
            var request = new RestRequest("/{apiVersion}/attributes", Method.POST)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddJsonBody(attributeRequest);

            var attribute = _restClient.Post<Attribute>(request);
            if (attribute.IsSuccessful)
                return attribute.Data;

            return null;
        }

        #endregion Attributes

        #region Features

        public List<Feature> GetFeatures()
        {
            var request = new RestRequest("/{apiVersion}/features", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddQueryParameter("project_id", _fullStackSettingsOptions.ProjectId);
            var features = _restClient.Get<List<Feature>>(request);
            return features.Data;
        }

        public Feature GetFeature(string id)
        {
            var request = new RestRequest("/{apiVersion}/features/{feature_id}", Method.GET)
                .AddUrlSegment("apiVersion", _fullStackSettingsOptions.APIVersion)
                .AddUrlSegment("feature_id", id);
            var feature = _restClient.Get<Feature>(request);
            return feature.Data;
        }

        #endregion Features

        #region Flags

        public FlagList GetFlags(int perPage = 20)
        {
            var request = new RestRequest("/flags/v1/projects/{project_id}/flags", Method.GET)
                .AddUrlSegment("project_id", _fullStackSettingsOptions.ProjectId)
                .AddQueryParameter("per_page", perPage.ToString());
            var features = _restClient.Get<FlagList>(request);
            return features.Data;
        }

        public Flag GetFlag(string flagKey)
        {
            var request = new RestRequest("/flags/v1/projects/{project_id}/flags/{flag_key}", Method.GET)
                .AddUrlSegment("project_id", _fullStackSettingsOptions.ProjectId)
                .AddUrlSegment("flag_key", flagKey);
            var feature = _restClient.Get<Flag>(request);
            return feature.Data;
        }

        #endregion Flags

        #region Variations

        public VariationList GetVariations(string flagKey, int perPage = 20)
        {
            var request = new RestRequest("/flags/v1/projects/{project_id}/flags/{flag_key}/variations", Method.GET)
                .AddUrlSegment("project_id", _fullStackSettingsOptions.ProjectId)
                .AddUrlSegment("flag_key", flagKey)
                .AddQueryParameter("per_page", perPage.ToString());
            var features = _restClient.Get<VariationList>(request);
            return features.Data;
        }

        public VariationItem GetVariation(string flagKey, string variationKey)
        {
            var request = new RestRequest("/flags/v1/projects/{project_id}/flags/{flag_key}/variations/{variation_key}", Method.GET)
                .AddUrlSegment("project_id", _fullStackSettingsOptions.ProjectId)
                .AddUrlSegment("flag_key", flagKey)
                .AddUrlSegment("variation_key", variationKey);
            var feature = _restClient.Get<VariationItem>(request);
            return feature.Data;
        }

        public VariationItem GetVariation(string uri)
        {
            var request = new RestRequest("/flags/v1{url}", Method.GET)
                .AddUrlSegment("url", uri);
            var feature = _restClient.Get<VariationItem>(request);
            return feature.Data;
        }

        #endregion Variations
    }
}