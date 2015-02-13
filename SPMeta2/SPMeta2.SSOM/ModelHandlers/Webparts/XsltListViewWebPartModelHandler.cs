using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace SPMeta2.SSOM.ModelHandlers.Webparts
{
    public class XsltListViewWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(XsltListViewWebPartDefinition); }
        }

        #endregion

        #region methods

        private WebpartPageModelHost _host;

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            _host = host;

            var typedModel = webpartModel.WithAssertAndCast<XsltListViewWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(XsltListViewWebPart).AssemblyQualifiedName;
        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition webpartModel)
        {
            base.ProcessWebpartProperties(webpartInstance, webpartModel);

            var typedWebpart = webpartInstance.WithAssertAndCast<XsltListViewWebPart>("webpartInstance", value => value.RequireNotNull());
            var typedModel = webpartModel.WithAssertAndCast<XsltListViewWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            var web = _host.SPLimitedWebPartManager.Web;

            // bind list
            SPList list = null;

            if (typedModel.ListId.HasValue && typedModel.ListId != default(Guid))
                list = web.Lists[typedModel.ListId.Value];
            else if (!string.IsNullOrEmpty(typedModel.ListUrl))
                list = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, typedModel.ListUrl));
            else if (!string.IsNullOrEmpty(typedModel.ListTitle))
                list = web.Lists.TryGetList(typedModel.ListTitle);
            else
                throw new SPMeta2Exception();

            if (list != null)
            {
                typedWebpart.ListName = list.ID.ToString("B").ToUpperInvariant();
                typedWebpart.TitleUrl = list.DefaultViewUrl;
            }

            // view check
            if (list != null)
            {
                SPView view = null;

                if (typedModel.ViewId.HasValue && typedModel.ViewId != default(Guid))
                    view = list.Views[typedModel.ViewId.Value];
                else if (!string.IsNullOrEmpty(typedModel.ViewName))
                    view = list.Views[typedModel.ViewName];

                if (view != null)
                {
                    typedWebpart.ViewGuid = view.ID.ToString("B").ToUpperInvariant();
                    typedWebpart.TitleUrl = view.ServerRelativeUrl;
                }
            }

            // able to 'reset', if NULL or use list-view based URLs
            if (typedModel.TitleUrl != null)
                typedWebpart.TitleUrl = typedModel.TitleUrl;

            // weird, but it must be set to avoid null-ref exceptions
            typedWebpart.GhostedXslLink = "main.xsl";

#if !NET35
            // rest
            typedWebpart.JSLink = typedModel.JSLink;
#endif
        }

        #endregion
    }
}
