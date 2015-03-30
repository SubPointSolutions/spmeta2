using System;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.Standard.ModelHandlers.Base;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;
using System.Collections;

namespace SPMeta2.SSOM.Standard.ModelHandlers.DisplayTemplates
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

        protected override void MapProperties(object modelHost, Hashtable fileProperties, ContentPageDefinitionBase definition)
        {
            base.MapProperties(modelHost, fileProperties, definition);

            var typedTemplateModel = definition.WithAssertAndCast<ControlDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            fileProperties[BuiltInInternalFieldNames.ContentTypeId] = "0x0101002039C03B61C64EC4A04F5361F385106601";

            if (!string.IsNullOrEmpty(typedTemplateModel.CrawlerXSLFileURL))
            {
                var crawlerXSLFileValue = new SPFieldUrlValue { Url = typedTemplateModel.CrawlerXSLFileURL };

                if (!string.IsNullOrEmpty(typedTemplateModel.CrawlerXSLFileDescription))
                    crawlerXSLFileValue.Description = typedTemplateModel.CrawlerXSLFileDescription;

                fileProperties["CrawlerXSLFile"] = crawlerXSLFileValue.ToString();
            }
        }

        public override Type TargetType
        {
            get { return typeof(ControlDisplayTemplateDefinition); }
        }
    }
}
