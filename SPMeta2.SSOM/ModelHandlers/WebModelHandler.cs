using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(WebDefinition); }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());
            var parentHost = modelHost;

            if (parentHost is SPSite)
                CreateWeb((parentHost as SPSite).RootWeb, webModel);
            else if (parentHost is SPWeb)
                CreateWeb((parentHost as SPWeb), webModel);
            else
            {
                throw new Exception("modelHost needs to be either SPSite or SPWeb");
            }
        }

        private void CreateWeb(SPWeb parentWeb, WebDefinition webModel)
        {
            if (string.IsNullOrEmpty(webModel.CustomWebTemplate))
            {
                // TODO
                using (var web = GetOrCreateWeb(parentWeb, webModel))
                {
                    web.Title = webModel.Title;
                    web.Description = webModel.Description;

                    web.Update();
                }
            }
            else
            {
                throw new NotImplementedException("CUstom web templates is not supported yet");
            }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var webDefinition = model as WebDefinition;
            SPWeb parentWeb = null;

            if (modelHost is SPSite)
                parentWeb = (modelHost as SPSite).RootWeb;

            if (modelHost is SPWeb)
                parentWeb = (modelHost as SPWeb);

            using (var currentWeb = GetOrCreateWeb(parentWeb, webDefinition))
            {
                action(currentWeb);
            }
        }

        private SPWeb GetOrCreateWeb(SPWeb parentWeb, WebDefinition webModel)
        {
            var webUrl = webModel.Url;
            var webDescription = string.IsNullOrEmpty(webModel.Description) ? String.Empty : webModel.Description;

            var newWebSiteRelativeUrl = SPUrlUtility.CombineUrl(parentWeb.ServerRelativeUrl, webModel.Url);
            var currentWeb = parentWeb.Site.OpenWeb(newWebSiteRelativeUrl);

            if (!currentWeb.Exists)
            {
                currentWeb = parentWeb.Webs.Add(webUrl,
                                        webModel.Title,
                                        webDescription,
                                        webModel.LCID,
                                        webModel.WebTemplate,
                                        webModel.UseUniquePermission,
                                        webModel.ConvertIfThere);
            }

            return currentWeb;
        }

        #endregion
    }
}
