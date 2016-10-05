using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;

using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class DeveloperDashboardSettingsDefinitionValidator : WebApplicationModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var ddsDefinition = model.WithAssertAndCast<DeveloperDashboardSettingsDefinition>("model", value => value.RequireNotNull());

            ValidateDefinition(farmModelHost, farmModelHost.HostFarm, ddsDefinition);
        }

        private void ValidateDefinition(FarmModelHost farmModelHost, Microsoft.SharePoint.Administration.SPFarm sPFarm, DeveloperDashboardSettingsDefinition ddsDefinition)
        {
            // TODO
        }
    }
}
