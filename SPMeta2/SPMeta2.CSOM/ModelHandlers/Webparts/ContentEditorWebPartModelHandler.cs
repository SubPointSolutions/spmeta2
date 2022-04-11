using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Webparts
{
    public class ContentEditorWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContentEditorWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var typedModel = webPartModel.WithAssertAndCast<ContentEditorWebPartDefinition>("model", value => value.RequireNotNull());

            var wpXml = WebpartXmlExtensions.LoadWebpartXmlDocument(BuiltInWebPartTemplates.ContentEditorWebPart);

            if (!string.IsNullOrEmpty(typedModel.Content))
                wpXml.SetOrUpdateContentEditorWebPartProperty("Content", typedModel.Content, true);

            if (!string.IsNullOrEmpty(typedModel.ContentLink))
            {
                var urlValue = typedModel.ContentLink ?? string.Empty;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original value: [{0}]", urlValue);

                urlValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = urlValue,
                    Context = listItemModelHost
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced value: [{0}]", urlValue);

                wpXml.SetOrUpdateContentEditorWebPartProperty("ContentLink", urlValue);
            }

            return wpXml.ToString();
        }

        #endregion
    }
}
