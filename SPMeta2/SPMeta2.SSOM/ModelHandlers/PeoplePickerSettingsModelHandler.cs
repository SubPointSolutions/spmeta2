using System;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class PeoplePickerSettingsModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(PeoplePickerSettingsDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PeoplePickerSettingsDefinition>("model", value => value.RequireNotNull());

            DeployPeoplePickerSettings(modelHost, webAppModelHost.HostWebApplication, definition);
        }

        private void DeployPeoplePickerSettings(object modelHost, SPWebApplication webApplication, PeoplePickerSettingsDefinition definition)
        {
            var settings = GetCurrentPeoplePickerSettings(webApplication);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = settings,
                ObjectType = typeof(SPPeoplePickerSettings),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            MapPeoplePickerSettings(settings, definition);

            // reSP doesn't like updating SPWebApplication here, don't see an other way though
            webApplication.Update();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = settings,
                ObjectType = typeof(SPPeoplePickerSettings),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        protected SPPeoplePickerSettings GetCurrentPeoplePickerSettings(SPWebApplication webApplication)
        {
            return webApplication.PeoplePickerSettings;
        }

        private static void MapPeoplePickerSettings(SPPeoplePickerSettings settings, PeoplePickerSettingsDefinition definition)
        {
            if (!string.IsNullOrEmpty(definition.ActiveDirectoryCustomFilter))
                settings.ActiveDirectoryCustomFilter = definition.ActiveDirectoryCustomFilter;

            if (!string.IsNullOrEmpty(definition.ActiveDirectoryCustomQuery))
                settings.ActiveDirectoryCustomQuery = definition.ActiveDirectoryCustomQuery;

            if (definition.ActiveDirectoryRestrictIsolatedNameLevel.HasValue)
                settings.ActiveDirectoryRestrictIsolatedNameLevel = definition.ActiveDirectoryRestrictIsolatedNameLevel.Value;

            if (definition.ActiveDirectorySearchTimeout.HasValue)
                settings.ActiveDirectorySearchTimeout = definition.ActiveDirectorySearchTimeout.Value;

            if (definition.AllowLocalAccount.HasValue)
                settings.AllowLocalAccount = definition.AllowLocalAccount.Value;

            if (definition.NoWindowsAccountsForNonWindowsAuthenticationMode.HasValue)
                settings.NoWindowsAccountsForNonWindowsAuthenticationMode = definition.NoWindowsAccountsForNonWindowsAuthenticationMode.Value;

            if (definition.OnlySearchWithinSiteCollection.HasValue)
                settings.OnlySearchWithinSiteCollection = definition.OnlySearchWithinSiteCollection.Value;

            if (definition.PeopleEditorOnlyResolveWithinSiteCollection.HasValue)
                settings.PeopleEditorOnlyResolveWithinSiteCollection = definition.PeopleEditorOnlyResolveWithinSiteCollection.Value;
        }

        #endregion
    }
}
