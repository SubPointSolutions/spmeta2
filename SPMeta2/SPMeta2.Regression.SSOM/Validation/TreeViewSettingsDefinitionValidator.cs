using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class TreeViewSettingsDefinitionValidator : TreeViewSettingsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TreeViewSettingsDefinition>("model", value => value.RequireNotNull());

            var spObject = webModelHost.HostWeb;

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
