using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace SPMeta2.SSOM.ModelHandlers.Webparts
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

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            var typedModel = webpartModel.WithAssertAndCast<PageViewerWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(PageViewerWebPart).AssemblyQualifiedName;
        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition webpartModel)
        {
            base.ProcessWebpartProperties(webpartInstance, webpartModel);

            var typedWebpart = webpartInstance.WithAssertAndCast<PageViewerWebPart>("webpartInstance", value => value.RequireNotNull());
            var definition = webpartModel.WithAssertAndCast<PageViewerWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            if (!string.IsNullOrEmpty(definition.ContentLink))
            {
                var contentLinkValue = definition.ContentLink ?? string.Empty;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original contentLinkValue: [{0}]", contentLinkValue);

                contentLinkValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = contentLinkValue,
                    Context = CurrentHost.PageListItem.Web
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced contentLinkValue: [{0}]", contentLinkValue);

                typedWebpart.ContentLink = contentLinkValue;
            }

            if (!string.IsNullOrEmpty(definition.SourceType))
            {
                typedWebpart.SourceType = (PathPattern)Enum.Parse(typeof(PathPattern), definition.SourceType);
            }
        }

        #endregion
    }
}
