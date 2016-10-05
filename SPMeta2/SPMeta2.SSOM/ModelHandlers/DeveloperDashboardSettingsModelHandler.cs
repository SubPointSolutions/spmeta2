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

        private void DeployArtifact(FarmModelHost farmModelHost, SPFarm farm, DeveloperDashboardSettingsDefinition ddsDefinition)
        {
            var contentService = SPWebService.ContentService;
            var currentSettings = contentService.DeveloperDashboardSettings;

            // TODO, map out the DeveloperDashboardSettingsDefinition

            contentService.Update();
        }

        #endregion

    }
}
