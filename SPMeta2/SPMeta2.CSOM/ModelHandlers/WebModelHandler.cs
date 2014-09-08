using System;
using System.Runtime.Remoting.Contexts;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Exceptions;

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

            // @tarurar fix
            // "GetCurrentWebUrl fix (didn't work for root url sites)"
            // https://github.com/tarurar/spmeta2/commit/580172bb008742131ec5fb771f61af617b0e5f46

            var fullUrl = context.Url.ToLower();
            var serverUrl = fullUrl.EndsWith(parentWeb.ServerRelativeUrl.ToLower())
                ? fullUrl.Substring(0, fullUrl.LastIndexOf(parentWeb.ServerRelativeUrl.ToLower()))
                : fullUrl;
            var isParentWebRootUrl = parentWeb.ServerRelativeUrl.Trim('/').Length == 0;
            var currentWebUrl = serverUrl +
                                (isParentWebRootUrl
                                    ? String.Empty
                                    : parentWeb.ServerRelativeUrl +
                                      (webModel.Url.StartsWith("/") ? webModel.Url : "/" + webModel.Url));

            return currentWebUrl.ToLower();
        }

        protected Web ExtractWeb(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite.RootWeb;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb;

            throw new SPMeta2NotSupportedException(string.Format("New web canonot be created under model host of type:[{0}]", modelHost.GetType()));
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var parentWeb = ExtractWeb(modelHost);
            var hostClientContext = ExtractHostClientContext(modelHost);
            var hostSite = ExtractHostSite(modelHost);

            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

            var context = parentWeb.Context;

            context.Load(parentWeb, w => w.RootFolder);
            context.Load(parentWeb, w => w.ServerRelativeUrl);
            context.ExecuteQuery();

            var currentWebUrl = GetCurrentWebUrl(context, parentWeb, webModel);

            using (var webContext = new ClientContext(currentWebUrl))
            {
                var tmpWebContext = webContext;

                webContext.Credentials = context.Credentials;

                var tmpWebModelHost = new WebModelHost
                {
                    HostClientContext = hostClientContext,
                    HostSite = hostSite,
                    HostWeb = tmpWebContext.Web
                };

                action(tmpWebModelHost);
            }
        }

        private Site ExtractHostSite(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostSite;

            throw new SPMeta2NotSupportedException(string.Format("Cannot get host site from model host of type:[{0}]", modelHost.GetType()));

        }

        private ClientContext ExtractHostClientContext(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostClientContext;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostClientContext;

            throw new SPMeta2NotSupportedException(string.Format("Cannot get host client context from model host of type:[{0}]", modelHost.GetType()));

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

        protected Web GetWeb(Web parentWeb, WebDefinition definition)
        {
            var currentWebUrl = GetCurrentWebUrl(parentWeb.Context, parentWeb, definition);

            var tmp = new ClientContext(currentWebUrl);

            tmp.Credentials = parentWeb.Context.Credentials;

            tmp.Load(tmp.Web);
            tmp.ExecuteQuery();

            return tmp.Web;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var parentWeb = ExtractWeb(modelHost);
            var hostClientContext = ExtractHostClientContext(modelHost);
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

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
