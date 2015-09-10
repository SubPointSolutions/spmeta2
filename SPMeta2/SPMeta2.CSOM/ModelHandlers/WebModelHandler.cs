using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.Remoting.Contexts;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Exceptions;
using SPMeta2.Services;

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

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost as ModelHostBase;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var parentWeb = ExtractWeb(modelHost);
            var hostclientContext = ExtractHostClientContext(modelHost);
            var hostSite = ExtractHostSite(modelHost);

            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

            var context = parentWeb.Context;

            context.Load(parentWeb, w => w.Url);
            //context.Load(parentWeb, w => w.RootFolder);
            context.Load(parentWeb, w => w.ServerRelativeUrl);
            context.ExecuteQueryWithTrace();

            var currentWebUrl = GetCurrentWebUrl(context, parentWeb, webModel);
            var currentWeb = GetExistingWeb(hostSite, parentWeb, currentWebUrl);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = modelHostContext.ModelNode,
                Model = null,
                EventType = ModelEventType.OnModelHostResolving,
                Object = parentWeb,
                ObjectType = typeof(Web),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            var tmpWebModelHost = new WebModelHost
            {
                HostClientContext = hostclientContext,
                HostSite = hostSite,
                HostWeb = currentWeb
            };

            if (childModelType == typeof(ModuleFileDefinition))
            {
                var folderModelHost = ModelHostBase.Inherit<FolderModelHost>(modelHost, m =>
                {
                    m.CurrentWeb = currentWeb;
                    m.CurrentWebFolder = currentWeb.RootFolder; ;
                });

                action(folderModelHost);
            }
            else
            {
                action(tmpWebModelHost);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = modelHostContext.ModelNode,
                Model = null,
                EventType = ModelEventType.OnModelHostResolved,
                Object = parentWeb,
                ObjectType = typeof(Web),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            currentWeb.Update();
            context.ExecuteQueryWithTrace();
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

        protected Web GetExistingWeb(Site site, Web parentWeb, string currentWebUrl)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Entering GetExistingWeb()");

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

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "ExecuteQuery()");
            context.ExecuteQueryWithTrace();

            if (!scope.HasException && web != null && web.ServerObjectIsNull == false)
            {
                TraceService.InformationFormat((int)LogEventId.ModelProvisionCoreCall, "Found web with URL: [{0}]", currentWebUrl);

                context.Load(web);

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "ExecuteQuery()");
                context.ExecuteQueryWithTrace();

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Exciting GetExistingWeb()");

                return web;
            }

            TraceService.InformationFormat((int)LogEventId.ModelProvisionCoreCall, "Can't find web with URL: [{0}]. Returning NULL.", currentWebUrl);
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Exciting GetExistingWeb()");

            return null;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Extracting web from modelhost");
            var parentWeb = ExtractWeb(modelHost);

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Extracting host client context from model host");
            var hostclientContext = ExtractHostClientContext(modelHost);

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Casting web model definition");
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Loading Url/ServerRelativeUrl");
            var context = parentWeb.Context;

            context.Load(parentWeb, w => w.Url);
            //context.Load(parentWeb, w => w.RootFolder);
            context.Load(parentWeb, w => w.ServerRelativeUrl);

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "ExecuteQuery()");
            context.ExecuteQueryWithTrace();

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Extracting current web URL");
            var currentWebUrl = GetCurrentWebUrl(context, parentWeb, webModel);

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Current web URL: [{0}].", currentWebUrl);

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Loading existing web.");
            var currentWeb = GetExistingWeb(hostclientContext.Site, parentWeb, currentWebUrl);

            TraceService.Verbose((int)LogEventId.ModelProvisionUpdatingEventCall, "Calling OnUpdating event.");
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
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new web");

                var webUrl = webModel.Url;
                // Enhance web provision - handle '/' slash #620
                // https://github.com/SubPointSolutions/spmeta2/issues/620
                webUrl = UrlUtility.RemoveStartingSlash(webUrl);

                WebCreationInformation newWebInfo;

                if (string.IsNullOrEmpty(webModel.CustomWebTemplate))
                {
                    newWebInfo = new WebCreationInformation
                    {
                        Title = webModel.Title,
                        Url = webUrl,
                        Description = webModel.Description ?? string.Empty,
                        WebTemplate = webModel.WebTemplate,
                        UseSamePermissionsAsParentSite = !webModel.UseUniquePermission,
                        Language = (int)webModel.LCID
                    };
                }
                else
                {
                    var customWebTemplateName = webModel.CustomWebTemplate;

                    // by internal name
                    var templateCollection = parentWeb.GetAvailableWebTemplates(webModel.LCID, true);
                    var templateResult = context.LoadQuery(templateCollection
                                                                    .Include(tmp => tmp.Name, tmp => tmp.Title)
                                                                    .Where(tmp => tmp.Name == customWebTemplateName));


                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Trying to find template based on the given CustomWebTemplate and calling ExecuteQuery.");
                    context.ExecuteQueryWithTrace();

                    if (templateResult.FirstOrDefault() == null)
                    {
                        // one more try by title
                        templateResult = context.LoadQuery(templateCollection
                                                                   .Include(tmp => tmp.Name, tmp => tmp.Title)
                                                                   .Where(tmp => tmp.Title == customWebTemplateName));



                        TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Trying to find template based on the given CustomWebTemplate and calling ExecuteQuery.");
                        context.ExecuteQueryWithTrace();
                    }

                    var template = templateResult.FirstOrDefault();

                    if (template == null)
                        throw new SPMeta2ModelDeploymentException("Couldn't find custom web template: " + webModel.CustomWebTemplate);

                    newWebInfo = new WebCreationInformation
                    {
                        Title = webModel.Title,
                        Url = webModel.Url,
                        Description = webModel.Description ?? string.Empty,
                        WebTemplate = template.Name,
                        UseSamePermissionsAsParentSite = !webModel.UseUniquePermission,
                        Language = (int)webModel.LCID
                    };
                }

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Adding new web to the web collection and calling ExecuteQuery.");
                var newWeb = parentWeb.Webs.Add(newWebInfo);
                context.ExecuteQueryWithTrace();

                ProcessLocalization(newWeb, webModel);

                context.Load(newWeb);

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "ExecuteQuery()");
                context.ExecuteQueryWithTrace();

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
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Current web is not null. Updating Title/Description.");

                currentWeb.Title = webModel.Title;
                currentWeb.Description = webModel.Description ?? string.Empty;

                //  locale is not available with CSOM yet

                ProcessLocalization(currentWeb, webModel);

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

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "currentWeb.Update()");
                currentWeb.Update();

                context.ExecuteQueryWithTrace();
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
            context.ExecuteQueryWithTrace();

            var currentWebUrl = GetCurrentWebUrl(context, parentWeb, webModel);

            try
            {
                using (var webContext = new ClientContext(currentWebUrl))
                {
                    webContext.Credentials = context.Credentials;

                    var tmpWeb = webContext.Web;

                    webContext.Load(tmpWeb);
                    tmpWeb.DeleteObject();

                    webContext.ExecuteQueryWithTrace();
                }
            }
            catch (ClientRequestException)
            {
                // TODO, chekc is web exists
            }
        }

        protected virtual void ProcessLocalization(Web obj, WebDefinition definition)
        {
            ProcessGenericLocalization(obj, new Dictionary<string, List<ValueForUICulture>>
            {
                { "TitleResource", definition.TitleResource },
                { "DescriptionResource", definition.DescriptionResource },
            });
        }

        #endregion
    }
}
