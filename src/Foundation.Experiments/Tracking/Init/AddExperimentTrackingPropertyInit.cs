using System;
using System.Linq;
using EPiServer.DataAbstraction;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Framework.Localization;
using EPiServer.Security;

namespace Foundation.Experiments.Tracking.Init
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class AddExperimentTrackingPropertyInit : IInitializableModule
    {
        private const string ExperimentEventsTabName = "Experiment tracking";
        public const string ExperimentEventsPropertyName = "ExperimentTrackingEvents";

        private static readonly object _lock = new object();
        private IContentTypeRepository _contentTypeRepository;
        private ITabDefinitionRepository _tabDefinitionRepository;
        private IPropertyDefinitionRepository _propertyDefinitionRepository;
        private IPropertyDefinitionTypeRepository _propertyDefinitionTypeRepository;
        private LocalizationService _localizationService;

        public void Initialize(InitializationEngine context)
        {
            _contentTypeRepository = context.Locate.Advanced.GetInstance<IContentTypeRepository>();
            _tabDefinitionRepository = context.Locate.Advanced.GetInstance<ITabDefinitionRepository>();
            _propertyDefinitionRepository = context.Locate.Advanced.GetInstance<IPropertyDefinitionRepository>();
            _propertyDefinitionTypeRepository = context.Locate.Advanced.GetInstance<IPropertyDefinitionTypeRepository>();
            _localizationService = context.Locate.Advanced.GetInstance<LocalizationService>();

            SetupExperimentEventTrackingProperty();
        }

        private void SetupExperimentEventTrackingProperty()
        {
            CreateOrDeleteTab(ExperimentEventsTabName, true);
            var allowMappingType = typeof(IExperimentTracking);
            foreach (var modelType in _contentTypeRepository.List())
            {
                var contentType = _contentTypeRepository.Load(modelType.ID);
                if (contentType != null)
                {
                    if (allowMappingType.IsAssignableFrom(contentType.ModelType))
                    {
                        CreateUpdatePropertyDefinition(contentType,
                            ExperimentEventsPropertyName,
                            _localizationService.GetString("/episerver/experimentation/eventracking", "Events to track"),
                            typeof(EPiServer.SpecializedProperties.PropertyContentArea),
                            ExperimentEventsTabName,
                            10);
                    }
                }
            }
        }

        private void CreateOrDeleteTab(string tabName, bool createNew)
        {
            var obj2 = _lock;
            lock (obj2)
            {
                TabDefinition tabDefinition = this._tabDefinitionRepository.Load(tabName);
                if (createNew)
                {
                    if (tabDefinition != null) return;

                    tabDefinition = new TabDefinition
                    {
                        Name = tabName,
                        SortIndex = int.MaxValue,
                        RequiredAccess = AccessLevel.Publish
                    };
                    _tabDefinitionRepository.Save(tabDefinition);
                }
                else if (tabDefinition != null)
                {
                    _tabDefinitionRepository.Delete(tabDefinition);
                }
            }
        }

        private void CreateUpdatePropertyDefinition(ContentType contentType, string propertyDefinitionName, string editCaption = null, Type propertyDefinitionType = null, string tabName = null, int? propertyOrder = new int?())
        {
            PropertyDefinition propertyDefinition =
                GetPropertyDefinition(contentType, propertyDefinitionName);
            if (propertyDefinition == null)
            {
                if (propertyDefinitionType == null)
                {
                    return;
                }
                propertyDefinition = new PropertyDefinition();
            }
            else
            {
                propertyDefinition = propertyDefinition.CreateWritableClone();
            }
            propertyDefinition.ContentTypeID = contentType.ID;
            propertyDefinition.DisplayEditUI = true;
            propertyDefinition.DefaultValueType = DefaultValueType.None;
            if (propertyDefinitionName != null)
            {
                propertyDefinition.Name = propertyDefinitionName;
            }
            if (editCaption != null)
            {
                propertyDefinition.EditCaption = editCaption;
            }
            if (propertyDefinitionType != null)
            {
                propertyDefinition.Type = this._propertyDefinitionTypeRepository.Load(propertyDefinitionType);
            }
            if (tabName != null)
            {
                propertyDefinition.Tab = _tabDefinitionRepository.Load(tabName);
            }
            if (propertyOrder.HasValue)
            {
                propertyDefinition.FieldOrder = propertyOrder.Value;
            }
            _propertyDefinitionRepository.Save(propertyDefinition);
        }

        private PropertyDefinition GetPropertyDefinition(ContentType contentType, string propertyName, Type propertyDefinitionType = null)
        {
            var source = from pd in contentType.PropertyDefinitions
                         where pd.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)
                         select pd;
            if (propertyDefinitionType != null)
            {
                source = from pd in source
                         where pd.Type.DefinitionType == propertyDefinitionType
                         select pd;
            }
            return source.FirstOrDefault();
        }

        private bool IsInsightInstalled()
        {

            return true;
        }

        public void Uninitialize(InitializationEngine context) { }
    }
}
