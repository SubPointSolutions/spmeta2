using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Standard.ModelHandlers.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.DisplayTemplates
{
    public class JavaScriptDisplayTemplateModelHandler : TemplateModelHandlerBase
    {
        public override string FileExtension
        {
            get { return "js"; }
            set
            {

            }
        }

        protected override void MapProperties(object modelHost, ListItem item, ContentPageDefinitionBase definition)
        {
            base.MapProperties(modelHost, item, definition);

            var typedDefinition = definition.WithAssertAndCast<JavaScriptDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            item[BuiltInInternalFieldNames.ContentTypeId] = "0x0101002039C03B61C64EC4A04F5361F3851068";

            if (!string.IsNullOrEmpty(typedDefinition.Standalone))
                item["DisplayTemplateJSTemplateType"] = typedDefinition.Standalone;

            if (!string.IsNullOrEmpty(typedDefinition.TargetControlType))
                item["DisplayTemplateJSTargetControlType"] = typedDefinition.TargetControlType;

            if (!string.IsNullOrEmpty(typedDefinition.TargetListTemplateId))
                item["DisplayTemplateJSTargetListTemplate"] = typedDefinition.TargetListTemplateId;

            if (!string.IsNullOrEmpty(typedDefinition.TargetScope))
                item["DisplayTemplateJSTargetScope"] = typedDefinition.TargetScope;

            if (!string.IsNullOrEmpty(typedDefinition.IconUrl))
            {
                var iconValue = new FieldUrlValue { Url = typedDefinition.IconUrl };

                if (!string.IsNullOrEmpty(typedDefinition.IconDescription))
                    iconValue.Description = typedDefinition.IconDescription;

                item["DisplayTemplateJSIconUrl"] = iconValue;
            }
        }

        public override Type TargetType
        {
            get { return typeof(JavaScriptDisplayTemplateDefinition); }
        }
    }
}
