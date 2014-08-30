using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ManagedAccountDefinitionValidator : ManagedAccountModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var managedAccountDefinition = model.WithAssertAndCast<ManagedAccountDefinition>("model", value => value.RequireNotNull());

            DeployManagedAccount(modelHost, farmModelHost.HostFarm, managedAccountDefinition);
        }

        private void DeployManagedAccount(object modelHost, Microsoft.SharePoint.Administration.SPFarm sPFarm, ManagedAccountDefinition managedAccountDefinition)
        {

        }

    }
}
