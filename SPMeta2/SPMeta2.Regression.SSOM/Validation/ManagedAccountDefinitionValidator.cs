using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ManagedAccountDefinitionValidator : ManagedAccountModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ManagedAccountDefinition>("model", value => value.RequireNotNull());

            var spObject = GetManagedAccount(farmModelHost.HostFarm, definition);

            var assert = ServiceFactory.AssertService
                               .NewAssert(definition, spObject)
                                     .ShouldNotBeNull(spObject)
                                     // domain agnostic check
                                     .ShouldBeEndOf(m => m.LoginName, o => o.Username);
        }

        #endregion
    }
}
