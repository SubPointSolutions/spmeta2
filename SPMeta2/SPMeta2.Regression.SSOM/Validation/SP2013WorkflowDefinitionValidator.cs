using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class SP2013WorkflowDefinitionValidator : SP2013WorkflowDefinitionHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SP2013WorkflowDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentWorkflowDefinition(webModelHost.HostWeb, definition);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                                 .SkipProperty(m => m.Override, "Override is not supported yet.")
                                 .ShouldBeEqual(m => m.Xaml, o => o.Xaml)
                                 .ShouldBeEqual(m => m.DisplayName, o => o.DisplayName);


            // TODO, check Override later
        }
    }
}
