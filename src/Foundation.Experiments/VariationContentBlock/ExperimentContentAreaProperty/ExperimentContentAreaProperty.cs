//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using EPiServer.Core;
//using EPiServer.PlugIn;
//using EPiServer.Shell.ObjectEditing;
//using EPiServer.Shell.ObjectEditing.EditorDescriptors;
//using EPiServer.SpecializedProperties;

//namespace ExperimentContainer.ExperimentContentArea.ExperimentContentAreaProperty
//{
//    [PropertyDefinitionTypePlugIn]
//    public class PropertyExperimentContentArea : PropertyContentArea
//    {
//    }

//    public class ExperimentableContentArea : ContentArea
//    {

//    }

//    [EditorDescriptorRegistration(
//        TargetType = typeof(ExperimentableContentArea),
//        UIHint = "ExperimentContentArea",
//        EditorDescriptorBehavior = EditorDescriptorBehavior.Default)]
//    public class ExperimentContentAreaEditorDescriptor : EditorDescriptor
//    {
//        public const string UIHint = "ExperimentContentArea";

//        public ExperimentContentAreaEditorDescriptor()
//        {
//            //base.ClientEditingClass = "epi-cms/contentediting/editors/ContentAreaEditor";
//            Type[] typeArray1 = new Type[] { typeof(IContentData) };
//            base.AllowedTypes = typeArray1;
//            base.AllowedTypesFormatSuffix = "fragment";
//        }

//        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
//        {
//            base.ModifyMetadata(metadata, attributes);
//            metadata.OverlayConfiguration["customType"] = "epi-cms/widget/overlay/ContentArea";
//            metadata.CustomEditorSettings["converter"] = "epi-cms/propertyexperimentcontentarea";
//            metadata.OverlayConfiguration["dndTargetPropertyAllowMultiple"] = true;
//        }
//    }
//}