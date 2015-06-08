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
    public class FilterDisplayTemplateModelHandler : ItemControlTemplateModelHandlerBase
    {
        public override string FileExtension
        {
            get { return "html"; }
            set
            {

            }
        }

        protected override void MapProperties(object modelHost, Hashtable fileProperties, ContentPageDefinitionBase definition)
        {
            base.MapProperties(modelHost, fileProperties, definition);

            var typedDefinition = definition.WithAssertAndCast<FilterDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            fileProperties[BuiltInInternalFieldNames.ContentTypeId] = "0x0101002039C03B61C64EC4A04F5361F38510660400F643FF79F6BD764F8A469B6F153396EE";

            if (!string.IsNullOrEmpty(typedDefinition.CrawlerXSLFileURL))
            {
                var crawlerXSLFileValue = new SPFieldUrlValue { Url = typedDefinition.CrawlerXSLFileURL };

                if (!string.IsNullOrEmpty(typedDefinition.CrawlerXSLFileDescription))
                    crawlerXSLFileValue.Description = typedDefinition.CrawlerXSLFileDescription;

                fileProperties["CrawlerXSLFile"] = crawlerXSLFileValue.ToString();
            }

            if (!string.IsNullOrEmpty(typedDefinition.CompatibleManagedProperties))
                fileProperties["CompatibleManagedProperties"] = typedDefinition.CompatibleManagedProperties;

            if (typedDefinition.CompatibleSearchDataTypes.Count > 0)
            {
                var multiChoiceValue = new SPFieldMultiChoiceValue();

                foreach (var value in typedDefinition.CompatibleSearchDataTypes)
                    multiChoiceValue.Add(value);

                fileProperties["CompatibleSearchDataTypes"] = multiChoiceValue.ToString();
            }
        }

        public override Type TargetType
        {
            get { return typeof(FilterDisplayTemplateDefinition); }
        }
    }
}
