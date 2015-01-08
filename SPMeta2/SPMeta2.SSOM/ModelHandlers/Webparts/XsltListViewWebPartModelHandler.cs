using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
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
            SPList list = null;

            if (typedModel.ListId.HasValue)
                list = web.Lists[typedModel.ListId.Value];
            else if (!string.IsNullOrEmpty(typedModel.ListUrl))
                list = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, typedModel.ListUrl));
            else if (!string.IsNullOrEmpty(typedModel.ListTitle))
                list = web.Lists.TryGetList(typedModel.ListTitle);


            typedWebpart.JSLink = typedModel.JSLink;
            typedWebpart.ListName = list.ID.ToString();
            typedWebpart.ListId = list.ID;
            typedWebpart.TitleUrl = list.DefaultViewUrl;
        }

        #endregion
    }
}
