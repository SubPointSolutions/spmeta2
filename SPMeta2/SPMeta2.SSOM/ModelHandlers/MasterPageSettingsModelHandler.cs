using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class MasterPageSettingsModelHandler : SSOMModelHandlerBase
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
            var web = webModelHost.HostWeb;
            var site = web.Site;

            var siteRelativeUrl = site.ServerRelativeUrl;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = masterPageSettings,
                ModelHost = modelHost
            });

            if (!string.IsNullOrEmpty(masterPageSettings.SiteMasterPageUrl))
                web.MasterUrl = SPUrlUtility.CombineUrl(siteRelativeUrl, masterPageSettings.SiteMasterPageUrl);

            if (!string.IsNullOrEmpty(masterPageSettings.SystemMasterPageUrl))
                web.CustomMasterUrl = SPUrlUtility.CombineUrl(siteRelativeUrl, masterPageSettings.SystemMasterPageUrl);

            if (!string.IsNullOrEmpty(masterPageSettings.SiteMasterPageUrl) ||
                !string.IsNullOrEmpty(masterPageSettings.SystemMasterPageUrl))
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = web,
                    ObjectType = typeof(SPWeb),
                    ObjectDefinition = masterPageSettings,
                    ModelHost = modelHost
                });

                web.Update();
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = web,
                    ObjectType = typeof(SPWeb),
                    ObjectDefinition = masterPageSettings,
                    ModelHost = modelHost
                });
            }
        }

        #endregion
    }
}
