using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSP2013WorkflowSubscriptionDefinitionValidator : SP2013WorkflowSubscriptionDefinitionModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var workflowSubscriptionModelHost = modelHost.WithAssertAndCast<SP2013WorkflowSubscriptionModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SP2013WorkflowSubscriptionDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentWorkflowSubscription(workflowSubscriptionModelHost,
                    workflowSubscriptionModelHost.HostClientContext,
                    workflowSubscriptionModelHost.HostList, definition);

            // [FALSE] - [WorkflowDisplayName] <!- check DefinitionId, load workflow
            //        [FALSE] - [HistoryListUrl] 
            //        [FALSE] - [TaskListUrl]
            //        [FALSE] - [EventSourceId]
            //        [FALSE] - [EventTypes]

            if (!spObject.IsPropertyAvailable("PropertyDefinitions"))
            {
                var c = spObject.Context;
                c.Load(spObject, o => o.PropertyDefinitions);
                c.ExecuteQuery();

                // PropertyDefinitions has the following props to check later
                // HistoryListId
                // TaskListId
            }

            var assert = ServiceFactory.AssertService
                          .NewAssert(definition, spObject)
                                .ShouldNotBeNull(spObject)
                                .ShouldBeEqual(m => m.Name, o => o.Name);
        }
    }
}
