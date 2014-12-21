using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class SearchConfigurationDefinitionValidator : SearchConfigurationModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SearchConfigurationDefinition>("model", value => value.RequireNotNull());

            // TODO

            //var renditions = 
            //var spObject = 

            //var assert = ServiceFactory.AssertService
            //               .NewAssert(definition, spObject)
            //                     .ShouldNotBeNull(spObject);
        }

        #endregion
    }
}
