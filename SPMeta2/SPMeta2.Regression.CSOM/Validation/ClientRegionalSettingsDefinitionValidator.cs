using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientRegionalSettingsDefinitionValidator : RegionalSettingsModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<RegionalSettingsDefinition>("model", value => value.RequireNotNull());
            
            var spObject = GetCurrentRegionalSettings(webModelHost.HostWeb);

            var assert = ServiceFactory.AssertService
                                      .NewAssert(definition, spObject)
                                            .ShouldNotBeNull(spObject);

        }

        #endregion
    }
}
