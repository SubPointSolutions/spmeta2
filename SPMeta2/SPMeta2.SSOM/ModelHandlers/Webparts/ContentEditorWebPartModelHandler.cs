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
    public class ContentEditorWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContentEditorWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            var typedModel = webpartModel.WithAssertAndCast<ContentEditorWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(ContentEditorWebPart).AssemblyQualifiedName;
        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition webpartModel)
        {
            base.ProcessWebpartProperties(webpartInstance, webpartModel);

            var typedWebpart = webpartInstance.WithAssertAndCast<ContentEditorWebPart>("webpartInstance", value => value.RequireNotNull());
            var typedModel = webpartModel.WithAssertAndCast<ContentEditorWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            if (!string.IsNullOrEmpty(typedModel.ContentLink))
            {
                var contentLinkValue = typedModel.ContentLink ?? string.Empty;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original contentLinkValue: [{0}]",
                    contentLinkValue);

                contentLinkValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = contentLinkValue,
                    Context = CurrentHost.PageListItem.Web
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced contentLinkValue: [{0}]", contentLinkValue);

                typedWebpart.ContentLink = contentLinkValue;
            }

            if (!string.IsNullOrEmpty(typedModel.Content))
            {

                var xmlDoc = new XmlDocument();
                var xmlElement = xmlDoc.CreateElement("ContentElement");

                var content = typedModel.Content ?? string.Empty;

                xmlElement.InnerText = content;
                typedWebpart.Content = xmlElement;
            }
        }

        #endregion
    }
}
