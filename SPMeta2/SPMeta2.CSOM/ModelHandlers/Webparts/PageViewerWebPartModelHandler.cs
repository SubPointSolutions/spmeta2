using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Services;
using SPMeta2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.CSOM.ModelHandlers.Webparts
{
    public class PageViewerWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(PageViewerWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var definition = webPartModel.WithAssertAndCast<PageViewerWebPartDefinition>("model", value => value.RequireNotNull());
            var wpXml = WebpartXmlExtensions
                .LoadWebpartXmlDocument(BuiltInWebPartTemplates.PageViewerWebPart);

            if (!string.IsNullOrEmpty(definition.ContentLink))
            {
                var contentLinkValue = definition.ContentLink ?? string.Empty;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original contentLinkValue: [{0}]", contentLinkValue);

                contentLinkValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = contentLinkValue,
                    Context = listItemModelHost.HostClientContext
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced contentLinkValue: [{0}]", contentLinkValue);

                wpXml.SetOrUpdatePageViewerWebPartProperty("ContentLink", contentLinkValue);
            }

            if (!string.IsNullOrEmpty(definition.SourceType))
            {
                wpXml.SetOrUpdatePageViewerWebPartProperty("SourceType", definition.SourceType);
            }

            return wpXml.ToString();
        }

        #endregion
    }
}
