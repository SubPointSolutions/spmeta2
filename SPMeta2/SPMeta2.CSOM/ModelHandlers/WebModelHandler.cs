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
using SPMeta2.CSOM.Utils;

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

        protected string GetCurrentWebUrl(ClientRuntimeContext context, Web parentWeb, WebDefinition webModel)
        {
            var result = UrlUtility.CombineUrl(parentWeb.ServerRelativeUrl, webModel.Url);
            return result.ToLower();
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

            context.Load(parentWeb, w => w.Url);
            //context.Load(parentWeb, w => w.RootFolder);
            context.Load(parentWeb, w => w.ServerRelativeUrl);
            context.ExecuteQuery();

            var currentWebUrl = GetCurrentWebUrl(context, parentWeb, webModel);
            var currentWeb = GetExistingWeb(hostSite, parentWeb, currentWebUrl);

            var tmpWebModelHost = new WebModelHost
            {
                HostClientContext = hostClientContext,
                HostSite = hostSite,
                HostWeb = currentWeb
            };

            action(tmpWebModelHost);

            currentWeb.Update();
            context.ExecuteQuery();
        }

        private Site ExtractHostSite(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostSite;

            throw new SPMeta2NotSupportedException(string.Format("Cannot get host site from model host of type:[{0}]", modelHost.GetType()));

        }

        protected ClientContext ExtractHostClientContext(object modelHost)
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

        //protected Web GetWeb(Web parentWeb, WebDefinition definition)
        //{
        //    var currentWebUrl = GetCurrentWebUrl(parentWeb.Context, parentWeb, definition);

        //    var tmp = new ClientContext(currentWebUrl);

        //    tmp.Credentials = parentWeb.Context.Credentials;

        //    tmp.Load(tmp.Web);
        //    tmp.ExecuteQuery();

        //    return tmp.Web;
        //}

        protected Web GetExistingWeb(Site site, Web parentWeb, string currentWebUrl)
        {
            var result = false;
            var srcUrl = currentWebUrl.ToLower().Trim('/').Trim('\\');

            // for self-hosting and '/'
            if (parentWeb.Url.ToLower().Trim('/').Trim('\\').EndsWith(srcUrl))
                return parentWeb;

            var context = parentWeb.Context;

            Web web = null;

            var scope = new ExceptionHandlingScope(context);

            using (scope.StartScope())
            {
                using (scope.StartTry())
                {
                    web = site.OpenWeb(currentWebUrl);
                }

                using (scope.StartCatch())
                {

                }
            }

            context.ExecuteQuery();

            if (!scope.HasException && web != null && web.ServerObjectIsNull == false)
            {
                context.Load(web);
                context.ExecuteQuery();

                return web;
            }

            return null;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var parentWeb = ExtractWeb(modelHost);
            var hostClientContext = ExtractHostClientContext(modelHost);
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

            var context = parentWeb.Context;

            context.Load(parentWeb, w => w.Url);
            //context.Load(parentWeb, w => w.RootFolder);
            context.Load(parentWeb, w => w.ServerRelativeUrl);
            context.ExecuteQuery();

            var currentWebUrl = GetCurrentWebUrl(context, parentWeb, webModel);

            Web currentWeb = GetExistingWeb(hostClientContext.Site, parentWeb, currentWebUrl);

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

            if (currentWeb == null)
            {
                var newWebInfo = new WebCreationInformation
                {
                    Title = webModel.Title,
                    Url = webModel.Url,
                    Description = webModel.Description ?? string.Empty,
                    WebTemplate = webModel.WebTemplate,
                    UseSamePermissionsAsParentSite = !webModel.UseUniquePermission,
                    Language = (int)webModel.LCID
                };

                var newWeb = parentWeb.Webs.Add(newWebInfo);
                context.ExecuteQuery();

                context.Load(newWeb);
                context.ExecuteQuery();

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newWeb,
                    ObjectType = typeof(Web),
                    ObjectDefinition = model,
                    ModelHost = modelHost
                });
                InvokeOnModelEvent<WebDefinition, Web>(newWeb, ModelEventType.OnUpdated);
            }
            else
            {

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentWeb,
                    ObjectType = typeof(Web),
                    ObjectDefinition = model,
                    ModelHost = modelHost
                });
                InvokeOnModelEvent<WebDefinition, Web>(currentWeb, ModelEventType.OnUpdated);
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
