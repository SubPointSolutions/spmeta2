using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class InformationRightsManagementSettingsModelHandler : CSOMModelHandlerBase
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
                ObjectType = typeof(InformationRightsManagementSettings),
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
                ObjectType = typeof(InformationRightsManagementSettings),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            currentSettings.Update();
        }

        private void MapInformationRightsManagementSettings(InformationRightsManagementSettings currentSettings, InformationRightsManagementSettingsDefinition definition)
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

        protected InformationRightsManagementSettings GetCurrentInformationRightsManagementSettings(List list)
        {
            return list.InformationRightsManagementSettings;
        }

        #endregion
    }
}
