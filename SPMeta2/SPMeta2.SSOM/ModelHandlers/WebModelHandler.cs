using System;
using System.Globalization;
using System.Linq;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Exceptions;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(WebDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());
            var parentHost = modelHost;

            if (modelHost is SiteModelHost)
            {
                CreateWeb(modelHost, ((SiteModelHost)modelHost).HostSite.RootWeb, webModel);
            }
            else if (parentHost is WebModelHost)
            {
                CreateWeb(modelHost, ((WebModelHost)parentHost).HostWeb, webModel);
            }
            else
            {
                throw new Exception("modelHost needs to be either SPSite or SPWeb");
            }
        }

        private void CreateWeb(object modelHost, SPWeb parentWeb, WebDefinition webModel)
        {
            using (var web = GetOrCreateWeb(parentWeb, webModel, true))
            {
                MapProperties(web, webModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = web,
                    ObjectType = typeof(SPWeb),
                    ObjectDefinition = webModel,
                    ModelHost = modelHost
                });

                web.Update();
            }
        }

        protected virtual void MapProperties(SPWeb web, WebDefinition webModel)
        {
            // temporarily switch culture to allow setting of the properties Title and Description for multi-language scenarios
            CultureUtils.WithCulture(web.UICulture, () =>
            {
                if (!string.IsNullOrEmpty(webModel.Title))
                    web.Title = webModel.Title;

                if (!string.IsNullOrEmpty(webModel.Description))
                    web.Description = webModel.Description;
            });

            if (!string.IsNullOrEmpty(webModel.RequestAccessEmail))
                web.RequestAccessEmail = webModel.RequestAccessEmail;

            if (webModel.LCID > 0)
                web.Locale = new CultureInfo((int)webModel.LCID);

            if (!string.IsNullOrEmpty(webModel.AlternateCssUrl))
                web.AlternateCssUrl = webModel.AlternateCssUrl;

            if (!string.IsNullOrEmpty(webModel.SiteLogoUrl))
                web.SiteLogoUrl = webModel.SiteLogoUrl;

#if !NET35
            if (webModel.IndexedPropertyKeys.Any())
            {
                foreach (var indexedProperty in webModel.IndexedPropertyKeys)
                {
                    // indexed prop should exist in the prop bag
                    // otherwise it won't be saved by SharePoint (ILSpy / Refletor to see the logic)
                    // http://rwcchen.blogspot.com.au/2014/06/sharepoint-2013-indexed-property-keys.html

                    var propName = indexedProperty.Name;
                    var propValue = string.IsNullOrEmpty(indexedProperty.Value)
                                            ? string.Empty
                                            : indexedProperty.Value;

                    if (web.AllProperties.ContainsKey(propName))
                        web.AllProperties[propName] = propValue;
                    else
                        web.AllProperties.Add(propName, propValue);

                    if (!web.IndexedPropertyKeys.Contains(propName))
                        web.IndexedPropertyKeys.Add(propName);
                }
            }
#endif

            if (webModel.UseUniquePermission && web.HasUniqueRoleAssignments)
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

        protected virtual SPGroup ResolveSecurityGroup(SPWeb web, string groupName)
        {
            return web.SiteGroups[groupName];
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var webDefinition = model as WebDefinition;
            SPWeb parentWeb = null;

            if (modelHost is SiteModelHost)
                parentWeb = (modelHost as SiteModelHost).HostSite.RootWeb;

            if (modelHost is WebModelHost)
                parentWeb = (modelHost as WebModelHost).HostWeb;

            using (var currentWeb = GetOrCreateWeb(parentWeb, webDefinition, false))
            {
                if (childModelType == typeof(ModuleFileDefinition))
                {
                    var folderModelHost = new FolderModelHost
                    {
                        CurrentWeb = currentWeb,
                        CurrentWebFolder = currentWeb.RootFolder,
                    };

                    action(folderModelHost);
                }
                else
                {
                    action(new WebModelHost
                    {
                        HostWeb = currentWeb
                    });

                }

                currentWeb.Update();
            }
        }

        protected SPWeb GetWeb(SPWeb parentWeb, WebDefinition webModel)
        {
            var newWebSiteRelativeUrl = SPUrlUtility.CombineUrl(parentWeb.ServerRelativeUrl, webModel.Url);
            var currentWeb = parentWeb.Site.OpenWeb(newWebSiteRelativeUrl);

            return currentWeb;
        }

        protected virtual SPWebTemplate LookupCustomWebTemplateFromWeb(SPWeb web, WebDefinition definition)
        {
            // smain lookup by the internal name
            var result = web.GetAvailableWebTemplates(definition.LCID)
                            .OfType<SPWebTemplate>()
                            .FirstOrDefault(tmpl => tmpl.IsCustomTemplate
                                                    && !string.IsNullOrEmpty(tmpl.Name)
                                                    && tmpl.Name.ToUpper() == definition.CustomWebTemplate.ToUpper());

            if (result != null)
                return result;

            // one more try by title
            return web.GetAvailableWebTemplates(definition.LCID)
                      .OfType<SPWebTemplate>()
                      .FirstOrDefault(tmpl => tmpl.IsCustomTemplate
                                            && !string.IsNullOrEmpty(tmpl.Title)
                                            && tmpl.Title.ToUpper() == definition.CustomWebTemplate.ToUpper());
        }

        protected virtual SPWebTemplate LookupCustomWebTemplate(SPWeb web, WebDefinition definition)
        {
            // lookup on the current web?
            SPWebTemplate result = LookupCustomWebTemplateFromWeb(web, definition);

            // lookup on the site?
            if (result == null)
                result = LookupCustomWebTemplateFromWeb(web.Site.RootWeb, definition);

            return result;
        }

        protected SPWeb GetOrCreateWeb(SPWeb parentWeb, WebDefinition webModel, bool updateProperties)
        {
            var webUrl = webModel.Url;
            var webDescription = string.IsNullOrEmpty(webModel.Description) ? String.Empty : webModel.Description;

            // Enhance web provision - handle '/' slash #620
            // https://github.com/SubPointSolutions/spmeta2/issues/620
            webUrl = UrlUtility.RemoveStartingSlash(webUrl);

            var currentWeb = GetWeb(parentWeb, webModel);

            if (!currentWeb.Exists)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new web");

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = null,
                    ObjectType = typeof(SPWeb),
                    ObjectDefinition = webModel,
                    ModelHost = webModel
                });

                // custom web template handling
                // based on pull request Implematation for CustomWebTemplate included #612 by @andreasblueher 
                if (string.IsNullOrEmpty(webModel.CustomWebTemplate))
                {
                    currentWeb = parentWeb.Webs.Add(webUrl,
                        webModel.Title,
                        webDescription,
                        webModel.LCID,
                        webModel.WebTemplate,
                        webModel.UseUniquePermission,
                        webModel.ConvertIfThere);
                }
                else
                {
                    var customWebTemplate = LookupCustomWebTemplate(parentWeb, webModel);

                    if (customWebTemplate == null)
                        throw new SPMeta2ModelDeploymentException("Couldn't find custom web template: " + webModel.CustomWebTemplate);

                    currentWeb = parentWeb.Webs.Add(webUrl,
                        webModel.Title,
                        webModel.Description,
                        webModel.LCID,
                        customWebTemplate,
                        webModel.UseUniquePermission,
                        webModel.ConvertIfThere);
                }

                MapProperties(currentWeb, webModel);
                ProcessLocalization(currentWeb, webModel);
            }
            else
            {
                if (updateProperties)
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject,
                        "Current web is not null. Updating Title/Description.");

                    MapProperties(currentWeb, webModel);

                    ProcessLocalization(currentWeb, webModel);

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioning,
                        Object = currentWeb,
                        ObjectType = typeof(SPWeb),
                        ObjectDefinition = webModel,
                        ModelHost = webModel
                    });

                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "currentWeb.Update()");
                    currentWeb.Update();
                }
            }

            return currentWeb;
        }

        protected virtual void ProcessLocalization(SPWeb obj, WebDefinition definition)
        {
            if (definition.TitleResource.Any())
            {
                foreach (var locValue in definition.TitleResource)
                    LocalizationService.ProcessUserResource(obj, obj.TitleResource, locValue);
            }

            if (definition.DescriptionResource.Any())
            {
                foreach (var locValue in definition.DescriptionResource)
                    LocalizationService.ProcessUserResource(obj, obj.DescriptionResource, locValue);
            }
        }

        #endregion
    }
}
