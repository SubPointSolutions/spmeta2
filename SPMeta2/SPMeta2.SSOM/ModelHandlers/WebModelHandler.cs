using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
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

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());
            var parentHost = modelHost;

            if (modelHost is SiteModelHost)
            {
                CreateWeb(modelHost, (modelHost as SiteModelHost).HostSite.RootWeb, webModel);
            }
            else if (parentHost is WebModelHost)
            {
                CreateWeb(modelHost, (parentHost as WebModelHost).HostWeb, webModel);
            }
            else
            {
                throw new Exception("modelHost needs to be either SPSite or SPWeb");
            }
        }

        private void CreateWeb(object modelHost, SPWeb parentWeb, WebDefinition webModel)
        {
            if (string.IsNullOrEmpty(webModel.CustomWebTemplate))
            {
                // TODO
                using (var web = GetOrCreateWeb(parentWeb, webModel))
                {
                    web.Title = webModel.Title;
                    web.Description = webModel.Description;

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
            else
            {
                throw new NotImplementedException("CUstom web templates is not supported yet");
            }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var webDefinition = model as WebDefinition;
            SPWeb parentWeb = null;

            if (modelHost is SiteModelHost)
                parentWeb = (modelHost as SiteModelHost).HostSite.RootWeb;

            if (modelHost is WebModelHost)
                parentWeb = (modelHost as WebModelHost).HostWeb;

            using (var currentWeb = GetOrCreateWeb(parentWeb, webDefinition))
            {
                action(new WebModelHost
                {
                    HostWeb = currentWeb
                });

                currentWeb.Update();
            }
        }

        protected SPWeb GetWeb(SPWeb parentWeb, WebDefinition webModel)
        {
            var newWebSiteRelativeUrl = SPUrlUtility.CombineUrl(parentWeb.ServerRelativeUrl, webModel.Url);
            var currentWeb = parentWeb.Site.OpenWeb(newWebSiteRelativeUrl);

            return currentWeb;
        }

        protected SPWeb GetOrCreateWeb(SPWeb parentWeb, WebDefinition webModel)
        {
            var webUrl = webModel.Url;
            var webDescription = string.IsNullOrEmpty(webModel.Description) ? String.Empty : webModel.Description;

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
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Current web is not null. Updating Title/Description.");

                currentWeb.Title = webModel.Title;
                currentWeb.Description = webModel.Description ?? string.Empty;

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

            return currentWeb;
        }

        #endregion
    }
}
