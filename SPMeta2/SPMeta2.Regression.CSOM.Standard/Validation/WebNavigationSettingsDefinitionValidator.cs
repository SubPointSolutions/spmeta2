using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class WebNavigationSettingsDefinitionValidator : WebNavigationSettingsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WebNavigationSettingsDefinition>("model", value => value.RequireNotNull());


            var spObject = GetWebNavigationSettings(webModelHost, definition);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject);
        }
    }
}
