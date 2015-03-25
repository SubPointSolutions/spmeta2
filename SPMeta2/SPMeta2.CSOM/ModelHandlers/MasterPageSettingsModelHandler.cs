using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.Utils;
using UrlUtility = SPMeta2.Utils.UrlUtility;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class MasterPageSettingsModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(MasterPageSettingsDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var masterPageSettings = model.WithAssertAndCast<MasterPageSettingsDefinition>("model", value => value.RequireNotNull());

            DeployMasterPageSettings(modelHost, webModelHost, masterPageSettings);
        }

        private void DeployMasterPageSettings(object modelHost, WebModelHost webModelHost, MasterPageSettingsDefinition masterPageSettings)
        {
            var site = webModelHost.HostSite;
            var web = webModelHost.HostWeb;

            var sContext = site.Context;
            sContext.Load(site, s => s.ServerRelativeUrl);
            sContext.ExecuteQueryWithTrace();

            var siteRelativeUrl = site.ServerRelativeUrl;

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving server relative URL: [{0}]", siteRelativeUrl);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = masterPageSettings,
                ModelHost = modelHost
            });

            if (!string.IsNullOrEmpty(masterPageSettings.SiteMasterPageUrl))
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Setting web.MasterUrlL: [{0}]", masterPageSettings.SiteMasterPageUrl);
                web.CustomMasterUrl = UrlUtility.CombineUrl(siteRelativeUrl, masterPageSettings.SiteMasterPageUrl);
            }

            if (!string.IsNullOrEmpty(masterPageSettings.SystemMasterPageUrl))
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Setting web.CustomMasterUrl: [{0}]", masterPageSettings.SystemMasterPageUrl);
                web.MasterUrl = UrlUtility.CombineUrl(siteRelativeUrl, masterPageSettings.SystemMasterPageUrl);
            }

            if (!string.IsNullOrEmpty(masterPageSettings.SiteMasterPageUrl) ||
                !string.IsNullOrEmpty(masterPageSettings.SystemMasterPageUrl))
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = web,
                    ObjectType = typeof(Web),
                    ObjectDefinition = masterPageSettings,
                    ModelHost = modelHost
                });

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling web.Update()");
                web.Update();

                web.Context.ExecuteQueryWithTrace();
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = web,
                    ObjectType = typeof(Web),
                    ObjectDefinition = masterPageSettings,
                    ModelHost = modelHost
                });
            }
        }

        #endregion
    }
}
