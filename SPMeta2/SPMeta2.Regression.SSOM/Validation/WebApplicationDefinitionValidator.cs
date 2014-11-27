using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;

using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WebApplicationDefinitionValidator : WebApplicationModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var webApplicationDefinition = model.WithAssertAndCast<WebApplicationDefinition>("model", value => value.RequireNotNull());

            ValidateWebApplication(farmModelHost, farmModelHost.HostFarm, webApplicationDefinition);
        }

        private void ValidateWebApplication(FarmModelHost farmModelHost, Microsoft.SharePoint.Administration.SPFarm sPFarm, WebApplicationDefinition webApplicationDefinition)
        {
           
        }
    }
}
