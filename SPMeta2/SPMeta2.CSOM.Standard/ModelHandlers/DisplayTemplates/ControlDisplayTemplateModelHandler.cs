using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers.Base;
using SPMeta2.CSOM.Standard.ModelHandlers.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.DisplayTemplates
{
    public class ControlDisplayTemplateModelHandler : ItemControlTemplateModelHandlerBase
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

            var typedTemplateModel = definition.WithAssertAndCast<ControlDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            item[BuiltInInternalFieldNames.ContentTypeId] = "0x0101002039C03B61C64EC4A04F5361F385106601";

            if (!string.IsNullOrEmpty(typedTemplateModel.CrawlerXSLFileURL))
            {
                var crawlerXSLFileValue = new FieldUrlValue { Url = typedTemplateModel.CrawlerXSLFileURL };

                if (!string.IsNullOrEmpty(typedTemplateModel.CrawlerXSLFileDescription))
                    crawlerXSLFileValue.Description = typedTemplateModel.CrawlerXSLFileDescription;

                item["CrawlerXSLFile"] = crawlerXSLFileValue;
            }
        }

        public override Type TargetType
        {
            get { return typeof(ControlDisplayTemplateDefinition); }
        }
    }
}
