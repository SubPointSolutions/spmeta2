using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientTreeViewSettingsDefinitionValidator : TreeViewSettingsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TreeViewSettingsDefinition>("model", value => value.RequireNotNull());

            var spObject = webModelHost.HostWeb;
            var context = spObject.Context;

            context.Load(spObject);
            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                .ShouldNotBeNull(spObject)

                .ShouldBeEqualIfHasValue(m => m.QuickLaunchEnabled, o => o.QuickLaunchEnabled)
                .ShouldBeEqualIfHasValue(m => m.TreeViewEnabled, o => o.TreeViewEnabled);
        }
    }
}
