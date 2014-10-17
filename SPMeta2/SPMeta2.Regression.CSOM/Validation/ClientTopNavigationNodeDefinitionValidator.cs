using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Regression.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientTopNavigationNodeDefinitionValidator : TopNavigationNodeModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<TopNavigationNodeDefinition>("model", value => value.RequireNotNull());

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
