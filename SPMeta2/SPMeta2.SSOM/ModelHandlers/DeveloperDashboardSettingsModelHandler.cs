using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Utils;
using Microsoft.SharePoint.Administration;
using System.Security;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class DeveloperDashboardSettingsModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DeveloperDashboardSettingsDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var ddsDefinition = model.WithAssertAndCast<DeveloperDashboardSettingsDefinition>("model", value => value.RequireNotNull());

            DeployArtifact(farmModelHost, farmModelHost.HostFarm, ddsDefinition);
        }

        protected virtual SPDeveloperDashboardSettings GetCurrentSettings()
        {
            var contentService = SPWebService.ContentService;
            var currentSettings = contentService.DeveloperDashboardSettings;

            return currentSettings;
        }

        private void DeployArtifact(FarmModelHost modelHost, SPFarm farm, DeveloperDashboardSettingsDefinition ddsDefinition)
        {
            var currentSettings = GetCurrentSettings();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentSettings,
                ObjectType = typeof(SPDeveloperDashboardSettings),
                ObjectDefinition = ddsDefinition,
                ModelHost = modelHost
            });

            if (!string.IsNullOrEmpty(ddsDefinition.DisplayLevel))
            {
                var displayLevel = (SPDeveloperDashboardLevel)Enum.Parse(typeof(SPDeveloperDashboardLevel), ddsDefinition.DisplayLevel);
                currentSettings.DisplayLevel = displayLevel;
                currentSettings.Update();
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentSettings,
                ObjectType = typeof(SPDeveloperDashboardSettings),
                ObjectDefinition = ddsDefinition,
                ModelHost = modelHost
            });
        }

        #endregion

    }
}
