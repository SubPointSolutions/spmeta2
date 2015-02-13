using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class InformationRightsManagementSettingsModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(InformationRightsManagementSettingsDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<InformationRightsManagementSettingsDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, listModelHost, definition);
        }

        private void DeployDefinition(object modelHost, ListModelHost siteModelHost, InformationRightsManagementSettingsDefinition definition)
        {
            var currentSettings = GetCurrentInformationRightsManagementSettings(siteModelHost.HostList);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentSettings,
                ObjectType = typeof(SPInformationRightsManagementSettings),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            MapInformationRightsManagementSettings(currentSettings, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentSettings,
                ObjectType = typeof(SPInformationRightsManagementSettings),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            currentSettings.Update();
        }

        private void MapInformationRightsManagementSettings(SPInformationRightsManagementSettings currentSettings, InformationRightsManagementSettingsDefinition definition)
        {
            currentSettings.AllowPrint = definition.AllowPrint;
            currentSettings.AllowScript = definition.AllowScript;
            currentSettings.AllowWriteCopy = definition.AllowWriteCopy;

            currentSettings.DisableDocumentBrowserView = definition.DisableDocumentBrowserView;
            currentSettings.DocumentAccessExpireDays = definition.DocumentAccessExpireDays;
            currentSettings.DocumentLibraryProtectionExpireDate = definition.DocumentLibraryProtectionExpireDate;

            currentSettings.EnableDocumentAccessExpire = definition.EnableDocumentAccessExpire;
            currentSettings.EnableDocumentBrowserPublishingView = definition.EnableDocumentBrowserPublishingView;
            currentSettings.EnableGroupProtection = definition.EnableGroupProtection;
            currentSettings.EnableLicenseCacheExpire = definition.EnableLicenseCacheExpire;

            currentSettings.GroupName = definition.GroupName;
            currentSettings.LicenseCacheExpireDays = definition.LicenseCacheExpireDays;

            currentSettings.PolicyDescription = definition.PolicyDescription;
            currentSettings.PolicyTitle = definition.PolicyTitle;
        }

        protected SPInformationRightsManagementSettings GetCurrentInformationRightsManagementSettings(SPList list)
        {
            return list.InformationRightsManagementSettings;
        }

        #endregion
    }
}
