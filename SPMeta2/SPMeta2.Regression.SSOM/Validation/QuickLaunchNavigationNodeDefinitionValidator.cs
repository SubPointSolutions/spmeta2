using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;

using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class QuickLaunchNavigationNodeDefinitionValidator : QuickLaunchNavigationNodeModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<QuickLaunchNavigationNodeDefinition>("model", value => value.RequireNotNull());

            var spObject = LookupNodeForHost(modelHost, definition);
            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                 .ShouldNotBeNull(spObject)
                 .ShouldBeEndOf(m => m.Url, o => o.Url)
                 .ShouldBeEqual(m => m.IsExternal, o => o.IsExternal)
                 .ShouldBeEqual(m => m.IsVisible, o => o.IsVisible)
                 .ShouldBeEqual(m => m.Title, o => o.Title);

        }
    }
}
