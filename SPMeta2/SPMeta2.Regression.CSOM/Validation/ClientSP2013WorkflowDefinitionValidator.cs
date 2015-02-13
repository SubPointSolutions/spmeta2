using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSP2013WorkflowDefinitionValidator : SP2013WorkflowDefinitionHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SP2013WorkflowDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentWorkflowDefinition(webModelHost.HostWeb, definition);

            if (!spObject.IsPropertyAvailable("Xaml"))
            {
                var spObjectContext = spObject.Context;

                spObjectContext.Load(spObject, o => o.Xaml);
                spObjectContext.ExecuteQuery();
            }

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
