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

        private string GetCurrentWebUrl(ClientRuntimeContext context, Web parentWeb, WebDefinition webModel)
        {
            // TOSDO, need to have "safe" url concats here, CSOM is doomed
            // WOOOOOOOOOHOAAAAAAAAA how bad is this?! :)

            var fullUrl = context.Url.ToLower();
            var serverUrl = fullUrl.Replace(parentWeb.ServerRelativeUrl.ToLower(), string.Empty);
            var currentWebUrl = serverUrl + "/" + parentWeb.ServerRelativeUrl + "/" + webModel.Url;

            return currentWebUrl.ToLower();
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

            var parentWeb = GetParentWeb(webModelHost);

            var context = parentWeb.Context;

            context.Load(parentWeb, w => w.RootFolder);
            context.Load(parentWeb, w => w.ServerRelativeUrl);
            context.ExecuteQuery();

            var currentWebUrl = GetCurrentWebUrl(context, parentWeb, webModel);

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

            var currentWebUrl = GetCurrentWebUrl(context, parentWeb, webModel);

            Web currentWeb = null;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = null,
                ObjectType = typeof(Web),
                ObjectDefinition = model,
                ModelHost = modelHost
            });
            InvokeOnModelEvent<WebDefinition, Web>(currentWeb, ModelEventType.OnUpdating);

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

            using (var tmpContext = new ClientContext(currentWebUrl))
            {
                tmpContext.Credentials = context.Credentials;

                var tmpWeb = tmpContext.Web;

                tmpContext.Load(tmpWeb);
                tmpContext.ExecuteQuery();

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = tmpWeb,
                    ObjectType = typeof(Web),
                    ObjectDefinition = model,
                    ModelHost = modelHost
                });
                InvokeOnModelEvent<WebDefinition, Web>(tmpContext.Web, ModelEventType.OnUpdated);

                tmpWeb.Update();
                tmpContext.ExecuteQuery();
            }
        }

        public override void RetractModel(object modelHost, DefinitionBase model)
        {
            // TODO, should be better behavior with try/catch or "Ensure" methods

            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

            var parentWeb = GetParentWeb(webModelHost);

            var context = parentWeb.Context;

            context.Load(parentWeb, w => w.RootFolder);
            context.Load(parentWeb, w => w.ServerRelativeUrl);
            context.ExecuteQuery();

            var currentWebUrl = GetCurrentWebUrl(context, parentWeb, webModel);

            try
            {
                using (var webContext = new ClientContext(currentWebUrl))
                {
                    webContext.Credentials = context.Credentials;

                    var tmpWeb = webContext.Web;

                    webContext.Load(tmpWeb);
                    tmpWeb.DeleteObject();

                    webContext.ExecuteQuery();
                }
            }
            catch (ClientRequestException)
            {
                // TODO, chekc is web exists
            }
        }

        #endregion
    }
}
