using EPiServer;
using EPiServer.Construction;
using EPiServer.Construction.Internal;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Optimizely.DeveloperFullStack.Core;
using Optimizely.DeveloperFullStack.Models;
using Optimizely.DeveloperFullStack.REST.Models;
using Optimizely.DeveloperFullStack.REST.Models.Audiences;
using Optimizely.DeveloperFullStack.REST.Models.Events;
using Optimizely.DeveloperFullStack.REST.Models.Flags;
using Optimizely.DeveloperFullStack.REST.Models.Variations;
using OptimizelyFullStackContentProvider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimizely.DeveloperFullStack
{
    public class OptimizelyFullStackContentProvider : ContentProvider
    {
        private readonly IOptimizelyFullStackRepository _optimizelyFullStackRepository;

        private readonly IdentityMappingService _identityMappingService;

        private readonly IContentLoader _contentLoader;

        private readonly IContentTypeRepository _contentTypeRepository;

        private ContentFactory _contentFactory;

        private readonly FullStackSettingsOptions _fullStackSettingsOptions;

        public OptimizelyFullStackContentProvider(
            IOptimizelyFullStackRepository optimizelyFullStackAPI,
            IdentityMappingService identityMappingService,
            ContentFactory contentFactory,
            IContentTypeRepository contentTypeRepository,
            IContentLoader contentLoader,
            FullStackSettingsOptions fullStackSettingsOptions)
        {
            _identityMappingService = identityMappingService;
            _contentLoader = contentLoader;
            _contentFactory = contentFactory;
            _contentTypeRepository = contentTypeRepository;
            _optimizelyFullStackRepository = optimizelyFullStackAPI;
            _fullStackSettingsOptions = fullStackSettingsOptions;
        }

        protected override IContent LoadContent(ContentReference contentLink, ILanguageSelector languageSelector)
        {
            IContent currentContent = null;

            MappedIdentity mappedIdentity = _identityMappingService.Get(contentLink);
            if (mappedIdentity == null)
                return null;

            var externalId = mappedIdentity.ExternalIdentifier;
            if (externalId == null)
                return null;

            var fullStackResourceId = (FullStackResourceId)externalId;
            switch (fullStackResourceId.FullStackType)
            {
                case FullStackType.AudienceFolder:
                    currentContent = CreateFolder<AudienceFolderData>(mappedIdentity, "Audiences");
                    break;

                case FullStackType.Audience:
                    var audience = _optimizelyFullStackRepository.GetAudience(fullStackResourceId.Id);
                    currentContent = CreateAudience(mappedIdentity, audience);
                    break;

                case FullStackType.EventFolder:
                    currentContent = CreateFolder<EventFolderData>(mappedIdentity, "Events");
                    break;

                case FullStackType.Event:
                    var eventItem = _optimizelyFullStackRepository.GetEvent(fullStackResourceId.Id);
                    currentContent = CreateEvent(mappedIdentity, eventItem);
                    break;

                case FullStackType.FlagFolder:
                    currentContent = CreateFolder<FlagFolderData>(mappedIdentity, "Feature Flags");
                    break;

                case FullStackType.Flag:
                    var flag = _optimizelyFullStackRepository.GetFlag(fullStackResourceId.Id);
                    currentContent = CreateFlag(mappedIdentity, flag);
                    break;

                case FullStackType.Variation:

                    var variation = _optimizelyFullStackRepository.GetVariation(fullStackResourceId.Id, fullStackResourceId.Key);
                    if (variation != null)
                        currentContent = CreateVariation(mappedIdentity, variation);
                    break;

                case FullStackType.VariableDefinition:
                    var flagVariable = _optimizelyFullStackRepository.GetFlag(fullStackResourceId.Id);
                    if (flagVariable?.VariableDefinitions?.Definitions?.Keys?.Contains(fullStackResourceId.Key) == true)
                        currentContent = CreateVariableDefinition(mappedIdentity, flagVariable.VariableDefinitions.Definitions[fullStackResourceId.Key]);
                    break;
            }
            return currentContent;
        }

        protected override IList<GetChildrenReferenceResult> LoadChildrenReferencesAndTypes(ContentReference contentLink, string languageID, out bool languageSpecific)
        {
            languageSpecific = false;
            var childrenList = new List<GetChildrenReferenceResult>();
            if (contentLink.CompareToIgnoreWorkID(EntryPoint))
            {
                childrenList.Add(new GetChildrenReferenceResult() { ContentLink = _identityMappingService.Get(MappedIdentity.ConstructExternalIdentifier(ProviderKey, new FullStackResourceId(_fullStackSettingsOptions.ProjectId.ToString(), FullStackType.AudienceFolder, ((int)FullStackType.AudienceFolder).ToString()).ToString()), true).ContentLink, IsLeafNode = false, ModelType = typeof(AudienceFolderData) });
                childrenList.Add(new GetChildrenReferenceResult() { ContentLink = _identityMappingService.Get(MappedIdentity.ConstructExternalIdentifier(ProviderKey, new FullStackResourceId(_fullStackSettingsOptions.ProjectId.ToString(), FullStackType.EventFolder, ((int)FullStackType.EventFolder).ToString()).ToString()), true).ContentLink, IsLeafNode = false, ModelType = typeof(EventFolderData) });
                childrenList.Add(new GetChildrenReferenceResult() { ContentLink = _identityMappingService.Get(MappedIdentity.ConstructExternalIdentifier(ProviderKey, new FullStackResourceId(_fullStackSettingsOptions.ProjectId.ToString(), FullStackType.FlagFolder, ((int)FullStackType.FlagFolder).ToString()).ToString()), true).ContentLink, IsLeafNode = false, ModelType = typeof(FlagFolderData) });

                return childrenList;
            }

            var mappedItem = _identityMappingService.Get(contentLink);
            var fullStackResourceId = (FullStackResourceId)mappedItem.ExternalIdentifier;
            FullStackType fullStackType = fullStackResourceId.FullStackType;

            switch (fullStackType)
            {
                case FullStackType.AudienceFolder:
                    var audiences = _optimizelyFullStackRepository.GetAudiences();
                    if (audiences.Any())
                    {
                        childrenList.AddRange(this.GetChildrenReferenceResults<AudienceData>(FullStackType.Audience, audiences));
                    }
                    break;

                case FullStackType.EventFolder:
                    var events = _optimizelyFullStackRepository.GetEvents();
                    if (events.Any())
                    {
                        childrenList.AddRange(this.GetChildrenReferenceResults<EventData>(FullStackType.Event, events));
                    }
                    break;

                case FullStackType.FlagFolder:
                    var flags = _optimizelyFullStackRepository.GetFlags();
                    if (flags?.Flags.Any() == true)
                    {
                        foreach (var flagItem in flags.Flags)
                        {
                            var flagResourceId = new FullStackResourceId(flagItem.ProjectId.ToString(), FullStackType.Flag, flagItem.Key);
                            var mappedIdentity = _identityMappingService.Get(MappedIdentity.ConstructExternalIdentifier(ProviderKey, flagResourceId.ToString()), true);
                            childrenList.Add(new GetChildrenReferenceResult()
                            {
                                ContentLink = mappedIdentity.ContentLink,
                                IsLeafNode = false,
                                ModelType = typeof(FlagData)
                            });
                        }
                    }
                    break;

                case FullStackType.Flag:
                    var flag = _optimizelyFullStackRepository.GetFlag(fullStackResourceId.Id);

                    if (flag != null)
                    {
                        // Load Variables
                        if (flag.VariableDefinitions.Definitions.Any())
                        {
                            foreach (var definition in flag.VariableDefinitions.Definitions)
                            {
                                var definitionResourceId = new FullStackResourceId(_fullStackSettingsOptions.ProjectId, FullStackType.VariableDefinition, fullStackResourceId.Id, definition.Key);
                                var definitionMappedIdentity = _identityMappingService.Get(MappedIdentity.ConstructExternalIdentifier(ProviderKey, definitionResourceId.ToString()), true);

                                childrenList.Add(new GetChildrenReferenceResult()
                                {
                                    ContentLink = definitionMappedIdentity.ContentLink,
                                    IsLeafNode = true,
                                    ModelType = typeof(VariableDefinitionData)
                                });
                                AddContentToCache(CreateVariableDefinition(definitionMappedIdentity, definition.Value));
                            }
                        }

                        // Load in Experiments
                        if (flag.Enviroments.Environments.ContainsKey(_fullStackSettingsOptions.EnviromentKey))
                        {
                            var environment = flag.Enviroments.Environments[_fullStackSettingsOptions.EnviromentKey];
                            if (environment.Rules.Any())
                            {
                                var experimentResourceId = new FullStackResourceId(_fullStackSettingsOptions.ProjectId, FullStackType.Experiment, fullStackResourceId.Id, environment.Key);
                                var enviromentMappedIdentity = _identityMappingService.Get(MappedIdentity.ConstructExternalIdentifier(ProviderKey, experimentResourceId.ToString()), true);
                                childrenList.Add(new GetChildrenReferenceResult()
                                {
                                    ContentLink = enviromentMappedIdentity.ContentLink,
                                    IsLeafNode = true,
                                    ModelType = typeof(ExperimentData)
                                });
                            }
                        }

                        // Load Variants
                        var variants = _optimizelyFullStackRepository.GetVariations(flag.Key, 100);
                        if (variants.Count > 0)
                        {
                            foreach (var variant in variants.Items)
                            {
                                var variantResourceId = new FullStackResourceId(_fullStackSettingsOptions.ProjectId, FullStackType.Variation, fullStackResourceId.Id, variant.Key);
                                var variantMappedIdentity = _identityMappingService.Get(MappedIdentity.ConstructExternalIdentifier(ProviderKey, variantResourceId.ToString()), true);
                                childrenList.Add(new GetChildrenReferenceResult()
                                {
                                    ContentLink = variantMappedIdentity.ContentLink,
                                    IsLeafNode = true,
                                    ModelType = typeof(VariationData)
                                });
                                var variationItem = _optimizelyFullStackRepository.GetVariation(flag.Key, variant.Key);
                                if (variationItem != null)
                                    AddContentToCache(CreateVariation(variantMappedIdentity, variationItem));
                            }
                        }
                    }
                    break;
            }
            return childrenList;
        }

        protected override IEnumerable<IContent> LoadContents(IList<ContentReference> contentReferences, ILanguageSelector selector)
        {
            var list = new List<IContent>();

            if (contentReferences.Any())
                list.AddRange(contentReferences.Select(reference => Load(reference, selector)));

            return list;
        }

        protected override ContentResolveResult ResolveContent(Guid contentGuid)
        {
            if (contentGuid == null || contentGuid.Equals(Guid.Empty))
                return null;

            var mappedIdentity = _identityMappingService.Get(contentGuid);
            if (mappedIdentity != null)
            {
                return ResolveContent(mappedIdentity.ContentLink);
            }
            return null;
        }

        protected override ContentResolveResult ResolveContent(ContentReference contentLink)
        {
            if (ContentReference.IsNullOrEmpty(contentLink))
                return null;

            if (!contentLink.ProviderName.Equals(FullStackConstants.RepositoryKey, StringComparison.OrdinalIgnoreCase))
                return null;

            var mappedIdentity = _identityMappingService.Get(contentLink);
            if (mappedIdentity != null)
            {
                var contentItem = LoadContent(mappedIdentity.ContentLink, null);
                ContentResolveResult resolveResult = new ContentResolveResult
                {
                    ContentLink = mappedIdentity.ContentLink,
                    UniqueID = mappedIdentity.ContentGuid,
                    ContentUri = ConstructContentUri(contentItem.ContentTypeID, mappedIdentity.ContentLink, mappedIdentity.ContentGuid)
                };

                return resolveResult;
            }
            return null;
        }

        #region Fill DataTypes

        private List<GetChildrenReferenceResult> GetChildrenReferenceResults<T>(FullStackType fullStackType, IEnumerable<FullStackBase> baseDatas, bool isLeafNode = true)
        {
            var childrenList = new List<GetChildrenReferenceResult>();
            foreach (var data in baseDatas)
            {
                // Since flags use the key, we need to as well
                var id = data.Id.ToString();
                if (data is Flag)
                    id = data.Key;

                var fullStackResourceId = new FullStackResourceId(data.ProjectId.ToString(), fullStackType, id);
                var mappedIdentity = _identityMappingService.Get(MappedIdentity.ConstructExternalIdentifier(ProviderKey, fullStackResourceId.ToString()), true);

                var result = new GetChildrenReferenceResult()
                {
                    ContentLink = mappedIdentity.ContentLink,
                    IsLeafNode = isLeafNode,
                    ModelType = typeof(T)
                };

                childrenList.Add(result);

                // Add the content to cache
                if (data is Flag flag)
                    AddContentToCache(CreateFlag(mappedIdentity, flag));
                else if (data is Event eventItem)
                    AddContentToCache(CreateEvent(mappedIdentity, eventItem));
                else if (data is Audience audience)
                    AddContentToCache(CreateAudience(mappedIdentity, audience));
            }
            return childrenList;
        }

        private T CreateFolder<T>(MappedIdentity mappedIdentity, string folderName) where T : ContentFolder =>
            CreateBasicContentData<T>(mappedIdentity, folderName) as T;

        private AudienceData CreateAudience(MappedIdentity mappedIdentity, Audience item)
        {
            var content = CreateBasicContentData<AudienceData>(mappedIdentity, item.Name) as AudienceData;
            content = FillData(content, item);

            return content;
        }

        private EventData CreateEvent(MappedIdentity mappedIdentity, Event item)
        {
            var content = CreateBasicContentData<EventData>(mappedIdentity, item.Name) as EventData;
            content = FillData(content, item);
            content.EventType = item.EventType;
            content.CategoryName = item.Category;
            return content;
        }

        private FlagData CreateFlag(MappedIdentity mappedIdentity, Flag item)
        {
            var parentIdentity = MappedIdentity.ConstructExternalIdentifier(ProviderKey, new FullStackResourceId(_fullStackSettingsOptions.ProjectId, FullStackType.FlagFolder, ((int)FullStackType.FlagFolder).ToString()).ToString());
            var content = CreateBasicContentData<FlagData>(mappedIdentity, _identityMappingService.Get(parentIdentity, true).ContentLink, item.Name) as FlagData;
            FillData(content, item);
            content.Urn = item.Urn;
            content.Url = item.Url;

            return content;
        }

        private VariationData CreateVariation(MappedIdentity mappedIdentity, VariationItem item)
        {
            var resourceId = (FullStackResourceId)mappedIdentity.ExternalIdentifier;

            ContentReference parentContentLink = _identityMappingService.Get(MappedIdentity.ConstructExternalIdentifier(ProviderKey, new FullStackResourceId(resourceId.ProjectId, FullStackType.Flag, resourceId.Id).ToString()), true).ContentLink;
            var contentType = _contentTypeRepository.Load(typeof(VariationData));

            var content = _contentFactory.CreateContent(contentType, new BuildingContext(contentType)
            {
                Parent = _contentLoader.Get<IContent>(parentContentLink)
            }) as VariationData;

            content.Name = item.Key;
            content.ContentTypeID = contentType.ID;
            content.ParentLink = parentContentLink;
            content.ContentGuid = mappedIdentity.ContentGuid;
            content.ContentLink = mappedIdentity.ContentLink;
            content.Description = item.Description;
            content.Key = item.Key;
            content.Flag = item.FlagKey;
            content.Enabled = item.Enabled;
            content.InUse = item.InUse;
            content.ProjectId = _fullStackSettingsOptions.ProjectId;
            content.FullStackId = item.Id.ToString();

            if (content is IContentSecurable securable)
                securable.GetContentSecurityDescriptor().AddEntry(new AccessControlEntry(EveryoneRole.RoleName, AccessLevel.Read));

            if (content is IVersionable versionable)
                versionable.Status = VersionStatus.Published;

            if (content is IChangeTrackable changeTrackable)
            {
                changeTrackable.Created = item.CreatedTime.UtcDateTime;
                changeTrackable.Changed = item.UpdatedTime.UtcDateTime;
            }

            return content;
        }

        private VariableDefinitionData CreateVariableDefinition(MappedIdentity mappedIdentity, Advanced item)
        {
            var resourceId = (FullStackResourceId)mappedIdentity.ExternalIdentifier;

            ContentReference parentContentLink = _identityMappingService.Get(MappedIdentity.ConstructExternalIdentifier(ProviderKey, new FullStackResourceId(resourceId.ProjectId.ToString(), FullStackType.Flag, resourceId.Id).ToString()), true).ContentLink;
            var contentType = _contentTypeRepository.Load(typeof(VariableDefinitionData));

            var content = _contentFactory.CreateContent(contentType, new BuildingContext(contentType)
            {
                Parent = _contentLoader.Get<IContent>(parentContentLink)
            }) as VariableDefinitionData;

            content.ContentTypeID = contentType.ID;
            content.ParentLink = parentContentLink;
            content.ContentGuid = mappedIdentity.ContentGuid;
            content.ContentLink = mappedIdentity.ContentLink;
            content.Description = item.Description;
            content.DefaultValue = item.DefaultValue;
            content.Type = item.Type;
            content.Key = item.Key;
            if (content is IContentSecurable securable)
            {
                securable.GetContentSecurityDescriptor().AddEntry(new AccessControlEntry(EveryoneRole.RoleName, AccessLevel.Read));
            }

            if (content is IVersionable versionable)
            {
                versionable.Status = VersionStatus.Published;
            }

            content.Name = item.Key;
            if (content is IChangeTrackable changeTrackable)
            {
                changeTrackable.Created = item.CreatedTime.UtcDateTime;
                changeTrackable.Changed = item.UpdatedTime.UtcDateTime;
            }

            return content;
        }

        private T FillData<T>(T content, FullStackBase fullStackBase) where T : FullStackBaseData
        {
            content.Key = fullStackBase.Key;
            content.Name = fullStackBase.Name;
            content.ProjectId = fullStackBase.ProjectId.ToString();
            content.FullStackId = fullStackBase.Id.ToString();
            content.IsClassic = fullStackBase.IsClassic;

            return content;
        }

        #endregion Fill DataTypes

        #region Create IContent

        private IContent CreateBasicContentData<T>(MappedIdentity mappedIdentity, ContentReference parentContentLink, string name) where T : class
        {
            var contentType = _contentTypeRepository.Load(typeof(T));

            var content = _contentFactory.CreateContent(contentType, new BuildingContext(contentType)
            {
                Parent = _contentLoader.Get<IContent>(parentContentLink)
            });

            content.Name = name;
            content.ContentTypeID = contentType.ID;
            content.ParentLink = parentContentLink;
            content.ContentGuid = mappedIdentity.ContentGuid;
            content.ContentLink = mappedIdentity.ContentLink;

            if (content is IContentSecurable securable)
            {
                securable.GetContentSecurityDescriptor().AddEntry(new AccessControlEntry(EveryoneRole.RoleName, AccessLevel.Read));
            }

            if (content is IVersionable versionable)
            {
                versionable.Status = VersionStatus.Published;
            }

            return content;
        }

        private IContent CreateBasicContentData<T>(MappedIdentity mappedIdentity, string name) where T : class =>
            CreateBasicContentData<T>(mappedIdentity, EntryPoint, name);

        #endregion Create IContent

        #region Caching

        protected override void SetCacheSettings(IContent content, CacheSettings cacheSettings)
        {
            cacheSettings.SlidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;
            cacheSettings.AbsoluteExpiration = DateTime.Now.AddMinutes(_fullStackSettingsOptions.CacheInMinutes);

            base.SetCacheSettings(content, cacheSettings);
        }

        protected override void SetCacheSettings(ContentReference contentReference, IEnumerable<GetChildrenReferenceResult> children, CacheSettings cacheSettings)
        {
            cacheSettings.SlidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;
            cacheSettings.AbsoluteExpiration = DateTime.Now.AddMinutes(_fullStackSettingsOptions.CacheInMinutes);

            base.SetCacheSettings(contentReference, children, cacheSettings);
        }

        public virtual void ClearCache()
        {
            ClearProviderPagesFromCache();
        }

        #endregion Caching

        #region Helper

        public static IContent GetEntryPoint()
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var optimizelyRoot = contentRepository.GetBySegment(SiteDefinition.Current.RootPage, FullStackConstants.RepositoryKey, LanguageSelector.AutoDetect(true));
            if (optimizelyRoot == null)
            {
                optimizelyRoot = contentRepository.GetDefault<ContentFolder>(SiteDefinition.Current.RootPage);
                optimizelyRoot.Name = FullStackConstants.RepositoryName;
                contentRepository.Save(optimizelyRoot, SaveAction.Publish, AccessLevel.NoAccess);
            }
            return optimizelyRoot;
        }

        #endregion Helper
    }
}