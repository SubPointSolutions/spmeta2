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
using SPMeta2.Services;
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
            {
                throw new SPMeta2Exception("ListUrl, ListTitle or ListId should be defined.");
            }

            if (list != null)
            {
                typedWebpart.ListName = list.ID.ToString("B").ToUpperInvariant();
                typedWebpart.TitleUrl = list.DefaultViewUrl;
            }

            // view check
            if (list != null)
            {
                SPView srcView = null;

                if (typedModel.ViewId.HasValue && typedModel.ViewId != default(Guid))
                    srcView = list.Views[typedModel.ViewId.Value];
                else if (!string.IsNullOrEmpty(typedModel.ViewName))
                    srcView = list.Views[typedModel.ViewName];

                if (srcView != null)
                {
                    if (!string.IsNullOrEmpty(typedWebpart.ViewGuid))
                    {
                        // update hidden view, otherwise we can have weird SharePoint exception
                        // https://github.com/SubPointSolutions/spmeta2/issues/487

                        var hiddenView = list.Views[new Guid(typedWebpart.ViewGuid)];

                        hiddenView.ViewFields.DeleteAll();

                        foreach (string f in srcView.ViewFields)
                            hiddenView.ViewFields.Add(f);

                        hiddenView.RowLimit = srcView.RowLimit;
                        hiddenView.Query = srcView.Query;
#if !NET35
                        hiddenView.JSLink = srcView.JSLink;
#endif
                        hiddenView.IncludeRootFolder = srcView.IncludeRootFolder;
                        hiddenView.Scope = srcView.Scope;

                        hiddenView.Update();
                    }
                    else
                    {
                        typedWebpart.ViewGuid = srcView.ID.ToString("B").ToUpperInvariant();
                    }

                    typedWebpart.TitleUrl = srcView.ServerRelativeUrl;
                }
            }

            // able to 'reset', if NULL or use list-view based URLs
            if (!string.IsNullOrEmpty(typedModel.TitleUrl))
                typedWebpart.TitleUrl = typedModel.TitleUrl;

            // weird, but it must be set to avoid null-ref exceptions
            typedWebpart.GhostedXslLink = "main.xsl";

#if !NET35
            // rest
            typedWebpart.JSLink = typedModel.JSLink;
#endif

            if (typedModel.CacheXslStorage.HasValue)
                typedWebpart.CacheXslStorage = typedModel.CacheXslStorage.Value;

            if (typedModel.CacheXslTimeOut.HasValue)
                typedWebpart.CacheXslTimeOut = typedModel.CacheXslTimeOut.Value;

#if !NET35
            if (typedModel.ShowTimelineIfAvailable.HasValue)
                typedWebpart.ShowTimelineIfAvailable = typedModel.ShowTimelineIfAvailable.Value;
#endif

            if (!string.IsNullOrEmpty(typedModel.Xsl))
            {
                typedWebpart.Xsl = typedModel.Xsl;
            }

            if (!string.IsNullOrEmpty(typedModel.XslLink))
            {
                var urlValue = typedModel.XslLink;

                typedWebpart.XslLink = urlValue;
            }

            if (!string.IsNullOrEmpty(typedModel.XmlDefinition))
            {
                typedWebpart.XmlDefinition = typedModel.XmlDefinition;
            }

            if (!string.IsNullOrEmpty(typedModel.XmlDefinitionLink))
            {
                var urlValue = typedModel.XmlDefinitionLink;
                typedWebpart.XmlDefinitionLink = urlValue;
            }

            if (!string.IsNullOrEmpty(typedModel.GhostedXslLink))
            {
                var urlValue = typedModel.GhostedXslLink;
                typedWebpart.GhostedXslLink = urlValue;
            }

            if (!string.IsNullOrEmpty(typedModel.BaseXsltHashKey))
                typedWebpart.BaseXsltHashKey = typedModel.BaseXsltHashKey;

            if (typedModel.DisableColumnFiltering.HasValue)
                typedWebpart.DisableColumnFiltering = typedModel.DisableColumnFiltering.Value;

#if !NET35
            if (typedModel.DisableSaveAsNewViewButton.HasValue)
                typedWebpart.DisableSaveAsNewViewButton = typedModel.DisableSaveAsNewViewButton.Value;

            if (typedModel.DisableViewSelectorMenu.HasValue)
                typedWebpart.DisableViewSelectorMenu = typedModel.DisableViewSelectorMenu.Value;

            if (typedModel.InplaceSearchEnabled.HasValue)
                typedWebpart.InplaceSearchEnabled = typedModel.InplaceSearchEnabled.Value;
#endif
        }

        #endregion
    }
}
