using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Utils;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

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
            sContext.ExecuteQuery();

            var siteRelativeUrl = site.ServerRelativeUrl;

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
                web.MasterUrl = UrlUtility.CombineUrl(siteRelativeUrl, masterPageSettings.SiteMasterPageUrl);

            if (!string.IsNullOrEmpty(masterPageSettings.SystemMasterPageUrl))
                web.CustomMasterUrl = UrlUtility.CombineUrl(siteRelativeUrl, masterPageSettings.SystemMasterPageUrl);

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

                web.Update();
                web.Context.ExecuteQuery();
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
