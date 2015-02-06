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
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                               .NewAssert(definition, spObject)
                               .ShouldNotBeNull(spObject);

            if (definition.QuickLaunchEnabled.HasValue)
                assert.ShouldBeEqual(m => m.QuickLaunchEnabled, o => o.QuickLaunchEnabled);
            else
                assert.SkipProperty(m => m.QuickLaunchEnabled, "QuickLaunchEnabled is NULL. Skipping.");

            if (definition.TreeViewEnabled.HasValue)
                assert.ShouldBeEqual(m => m.TreeViewEnabled, o => o.TreeViewEnabled);
            else
                assert.SkipProperty(m => m.TreeViewEnabled, "TreeViewEnabled is NULL. Skipping.");
        }
    }
}
