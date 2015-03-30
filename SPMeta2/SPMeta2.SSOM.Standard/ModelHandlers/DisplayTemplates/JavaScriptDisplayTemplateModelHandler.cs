using System;
using System.Collections;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.Standard.ModelHandlers.Base;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.DisplayTemplates
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

        protected override void MapProperties(object modelHost, Hashtable fileProperties, ContentPageDefinitionBase definition)
        {
            base.MapProperties(modelHost, fileProperties, definition);

            var typedDefinition = definition.WithAssertAndCast<JavaScriptDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            fileProperties[BuiltInInternalFieldNames.ContentTypeId] = "0x0101002039C03B61C64EC4A04F5361F3851068";

            if (!string.IsNullOrEmpty(typedDefinition.Standalone))
                fileProperties["DisplayTemplateJSTemplateType"] = typedDefinition.Standalone;

            if (!string.IsNullOrEmpty(typedDefinition.TargetControlType))
                fileProperties["DisplayTemplateJSTargetControlType"] = typedDefinition.TargetControlType;

            if (!string.IsNullOrEmpty(typedDefinition.TargetListTemplateId))
                fileProperties["DisplayTemplateJSTargetListTemplate"] = typedDefinition.TargetListTemplateId;

            if (!string.IsNullOrEmpty(typedDefinition.TargetScope))
                fileProperties["DisplayTemplateJSTargetScope"] = typedDefinition.TargetScope;

            if (!string.IsNullOrEmpty(typedDefinition.IconUrl))
            {
                var iconValue = new SPFieldUrlValue { Url = typedDefinition.IconUrl };

                if (!string.IsNullOrEmpty(typedDefinition.IconDescription))
                    iconValue.Description = typedDefinition.IconDescription;

                fileProperties["DisplayTemplateJSIconUrl"] = iconValue.ToString();
            }
        }

        public override Type TargetType
        {
            get { return typeof(JavaScriptDisplayTemplateDefinition); }
        }
    }
}
