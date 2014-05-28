using System;
using System.Runtime.Remoting.Contexts;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class WebModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WebDefinition); }
        }

        #endregion

        #region methods

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

            var parentWeb = GetParentWeb(webModelHost);

            var context = parentWeb.Context;

            context.Load(parentWeb, w => w.RootFolder);
            context.Load(parentWeb, w => w.ServerRelativeUrl);
            context.ExecuteQuery();

            // TOSDO, need to have "safe" url concats here, CSOM is doomed
            // WOOOOOOOOOHOAAAAAAAAA how bad is this?! :)
            var fullUrl = context.Url;
            var serverUrl = fullUrl.Replace(parentWeb.ServerRelativeUrl, string.Empty);
            var currentWebUrl = serverUrl + "/" + parentWeb.ServerRelativeUrl + "/" + webModel.Url;

            using (var webContext = new ClientContext(currentWebUrl))
            {
                var tmpWebContext = webContext;

                webContext.Credentials = context.Credentials;
                var tmpWebModelHost = ModelHostBase.Inherit<WebModelHost>(webModelHost,
                    webHost =>
                    {
                        webHost.HostWeb = tmpWebContext.Web;
                    });

                action(tmpWebModelHost);
            }
        }

        private static Web GetParentWeb(WebModelHost csomModelHost)
        {
            Web parentWeb = null;

            if (csomModelHost.HostWeb != null)
                parentWeb = csomModelHost.HostWeb;
            else if (csomModelHost.HostSite != null)
                parentWeb = csomModelHost.HostSite.RootWeb;
            else
                throw new ArgumentException("modelHost");

            return parentWeb;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var csomModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

            var parentWeb = GetParentWeb(csomModelHost); ;
            var context = parentWeb.Context;

            context.Load(parentWeb, w => w.RootFolder);
            context.Load(parentWeb, w => w.ServerRelativeUrl);
            context.ExecuteQuery();

            // TOSDO, need to have "safe" url concats here, CSOM is doomed
            // WOOOOOOOOOHOAAAAAAAAA how bad is this?! :)
            var fullUrl = context.Url;
            var serverUrl = fullUrl.Replace(parentWeb.ServerRelativeUrl, string.Empty);
            var currentWebUrl = serverUrl + "/" + parentWeb.ServerRelativeUrl + "/" + webModel.Url;

            Web currentWeb = null;

            InvokeOnModelEvents<WebDefinition, Web>(currentWeb, ModelEventType.OnUpdating);

            try
            {
                // TODO
                // how else???
                using (var tmp = new ClientContext(currentWebUrl))
                {
                    tmp.Credentials = context.Credentials;

                    tmp.Load(tmp.Web);
                    tmp.ExecuteQuery();
                }
            }
            catch (Exception)
            {
                var newWebInfo = new WebCreationInformation
                {
                    Title = webModel.Title,
                    Url = webModel.Url,
                    Description = webModel.Description ?? string.Empty,
                    WebTemplate = webModel.WebTemplate,
                    UseSamePermissionsAsParentSite = !webModel.UseUniquePermission
                };

                parentWeb.Webs.Add(newWebInfo);
                context.ExecuteQuery();
            }

            using (var tmp = new ClientContext(currentWebUrl))
            {
                tmp.Credentials = context.Credentials;

                tmp.Load(tmp.Web);
                tmp.ExecuteQuery();

                InvokeOnModelEvents<WebDefinition, Web>(tmp.Web, ModelEventType.OnUpdated);
            }
        }

        #endregion
    }
}
