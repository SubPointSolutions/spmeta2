using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;

using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
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


#if !NET35
            context.Load(parentWeb, w => w.Url);
#endif
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

        private static Site ExtractHostSite(object modelHost)
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
            Web parentWeb;

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

            var srcUrl = currentWebUrl.ToLower().Trim('/').Trim('\\');

#if !NET35
            // for self-hosting and '/'
            if (parentWeb.Url.ToLower().Trim('/').Trim('\\').EndsWith(srcUrl))
            {
                return parentWeb;
            }

#endif

#if NET35
            // for self-hosting and '/'
            if (parentWeb.ServerRelativeUrl.ToLower().Trim('/').Trim('\\').EndsWith(srcUrl))
            {
                return parentWeb;
            }
#endif

            var context = parentWeb.Context;

            Web web;

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

#if !NET35
            context.Load(parentWeb, w => w.Url);
#endif
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

#if !NET35
                if (webModel.IndexedPropertyKeys.Any())
                {
                    context.Load(newWeb, w => w.AllProperties);
                    context.ExecuteQueryWithTrace();
                }
#endif

                MapProperties(newWeb, webModel);
                ProcessLocalization(newWeb, webModel);

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

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "newWeb.Update()");

                newWeb.Update();
                context.ExecuteQueryWithTrace();
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Current web is not null. Updating Title/Description.");

#if !NET35
                if (webModel.IndexedPropertyKeys.Any())
                {
                    context.Load(currentWeb, w => w.AllProperties);
                    context.ExecuteQueryWithTrace();
                }
#endif

                MapProperties(currentWeb, webModel);
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

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "currentWeb.Update()");
                currentWeb.Update();

                context.ExecuteQueryWithTrace();
            }
        }

        protected virtual void MapProperties(Web web, WebDefinition webModel)
        {
            if (!string.IsNullOrEmpty(webModel.Title))
                web.Title = webModel.Title;

            if (!string.IsNullOrEmpty(webModel.Description))
                web.Description = webModel.Description;

            var supportedRuntime = ReflectionUtils.HasProperty(web, "AlternateCssUrl") && ReflectionUtils.HasProperty(web, "SiteLogoUrl");

            if (supportedRuntime)
            {
                var context = web.Context;

                if (!string.IsNullOrEmpty(webModel.AlternateCssUrl))
                    ClientRuntimeQueryService.SetProperty(web, "AlternateCssUrl", webModel.AlternateCssUrl);

                if (!string.IsNullOrEmpty(webModel.SiteLogoUrl))
                    ClientRuntimeQueryService.SetProperty(web, "SiteLogoUrl", webModel.SiteLogoUrl);
            }
            else
            {
                TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                    "CSOM runtime doesn't have Web.AlternateCssUrl and Web.SiteLogoUrl methods support. Update CSOM runtime to a new version. Provision is skipped");
            }

            if (!string.IsNullOrEmpty(webModel.RequestAccessEmail))
            {
                if (ReflectionUtils.HasProperty(web, "RequestAccessEmail"))
                    ClientRuntimeQueryService.SetProperty(web, "RequestAccessEmail", webModel.RequestAccessEmail);
                else
                {
                    TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                        "CSOM runtime doesn't have Web.RequestAccessEmail. Update CSOM runtime to a new version. Provision is skipped");
                }
            }
            if (webModel.MembersCanShare.HasValue)
            {
                if (ReflectionUtils.HasProperty(web, "MembersCanShare"))
                    ClientRuntimeQueryService.SetProperty(web, "MembersCanShare", webModel.MembersCanShare.Value);
                else
                {
                    TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                        "CSOM runtime doesn't have Web.MembersCanShare. Update CSOM runtime to a new version. Provision is skipped");
                }
            }

#if !NET35
            if (webModel.IndexedPropertyKeys.Any())
            {
                var props = web.AllProperties;

                // may not be there at all
                var indexedPropertyValue = props.FieldValues.Keys.Contains("vti_indexedpropertykeys")
                                            ? ConvertUtils.ToStringAndTrim(props["vti_indexedpropertykeys"])
                                            : string.Empty;

                var currentIndexedProperties = IndexedPropertyUtils.GetDecodeValueForSearchIndexProperty(indexedPropertyValue);

                // setup property bag
                foreach (var indexedProperty in webModel.IndexedPropertyKeys)
                {
                    // indexed prop should exist in the prop bag
                    // otherwise it won't be saved by SharePoint (ILSpy / Refletor to see the logic)
                    // http://rwcchen.blogspot.com.au/2014/06/sharepoint-2013-indexed-property-keys.html

                    var propName = indexedProperty.Name;
                    var propValue = string.IsNullOrEmpty(indexedProperty.Value)
                                            ? string.Empty
                                            : indexedProperty.Value;

                    props[propName] = propValue;
                }

                // merge and setup indexed prop keys, preserve existing props
                foreach (var indexedProperty in webModel.IndexedPropertyKeys)
                {
                    if (!currentIndexedProperties.Contains(indexedProperty.Name))
                        currentIndexedProperties.Add(indexedProperty.Name);
                }

                props["vti_indexedpropertykeys"] = IndexedPropertyUtils.GetEncodedValueForSearchIndexProperty(currentIndexedProperties);
            }
#endif

            // skipping check on web.HasUniqueRoleAssignments
            // we might have to deal with newrly created web
            if (webModel.UseUniquePermission)
            {
                // safe check - if not then we'll get the following exception
                // ---> System.InvalidOperationException: 
                // You cannot set this property since the web does not have unique permissions.

                if (!string.IsNullOrEmpty(webModel.AssociatedMemberGroupName))
                    web.AssociatedMemberGroup = ResolveSecurityGroup(web, webModel.AssociatedMemberGroupName);

                if (!string.IsNullOrEmpty(webModel.AssociatedOwnerGroupName))
                    web.AssociatedOwnerGroup = ResolveSecurityGroup(web, webModel.AssociatedOwnerGroupName);

                if (!string.IsNullOrEmpty(webModel.AssociatedVisitorGroupName))
                    web.AssociatedVisitorGroup = ResolveSecurityGroup(web, webModel.AssociatedVisitorGroupName);
            }
        }

        protected virtual Group ResolveSecurityGroup(Web web, String groupName)
        {
#if !NET35
            return web.SiteGroups.GetByName(groupName);
#endif

#if NET35
            // TODO, https://github.com/SubPointSolutions/spmeta2/issues/1108
            throw new SPMeta2NotImplementedException("This feature is not implemented yet for SP2010 - https://github.com/SubPointSolutions/spmeta2/issues/1108"); 
#endif
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
