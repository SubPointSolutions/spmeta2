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
    public class FilterDisplayTemplateModelHandler : ItemControlTemplateModelHandlerBase
    {
        public override string FileExtension
        {
            get { return "html"; }
            set
            {

            }
        }

        protected override void MapProperties(object modelHost, ListItem item, ContentPageDefinitionBase definition)
        {
            base.MapProperties(modelHost, item, definition);

            var typedDefinition = definition.WithAssertAndCast<FilterDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            item[BuiltInInternalFieldNames.ContentTypeId] = "0x0101002039C03B61C64EC4A04F5361F38510660400F643FF79F6BD764F8A469B6F153396EE";


            if (!string.IsNullOrEmpty(typedDefinition.CrawlerXSLFileURL))
            {
                var crawlerXSLFileValue = new FieldUrlValue { Url = typedDefinition.CrawlerXSLFileURL };

                if (!string.IsNullOrEmpty(typedDefinition.CrawlerXSLFileDescription))
                    crawlerXSLFileValue.Description = typedDefinition.CrawlerXSLFileDescription;

                item["CrawlerXSLFile"] = crawlerXSLFileValue;
            }

            if (!string.IsNullOrEmpty(typedDefinition.CompatibleManagedProperties))
                item["CompatibleManagedProperties"] = typedDefinition.CompatibleManagedProperties;

            if (typedDefinition.CompatibleSearchDataTypes.Count > 0)
            {
                item["CompatibleSearchDataTypes"] = typedDefinition.CompatibleSearchDataTypes.ToArray();
            }
        }

        public override Type TargetType
        {
            get { return typeof(FilterDisplayTemplateDefinition); }
        }
    }
}
