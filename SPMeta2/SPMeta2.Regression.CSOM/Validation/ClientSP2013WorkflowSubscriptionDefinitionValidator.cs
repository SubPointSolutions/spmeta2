using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client.WorkflowServices;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSP2013WorkflowSubscriptionDefinitionValidator : SP2013WorkflowSubscriptionDefinitionModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            if (modelHost is WebModelHost)
            {
                var workflowWebSubscriptionModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
                var definition = model.WithAssertAndCast<SP2013WorkflowSubscriptionDefinition>("model", value => value.RequireNotNull());

                var web = workflowWebSubscriptionModelHost.HostWeb;

                var spObject = GetCurrentWebWorkflowSubscriptioBySourceId(workflowWebSubscriptionModelHost,
                       workflowWebSubscriptionModelHost.HostClientContext,
                       web,
                       web.Id,
                       definition);

                ValidateWorkflowSubscription(modelHost, workflowWebSubscriptionModelHost.HostClientContext, workflowWebSubscriptionModelHost.HostWeb, spObject, definition);
            }

            if (modelHost is ListModelHost)
            {
                var workflowSubscriptionModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
                var definition = model.WithAssertAndCast<SP2013WorkflowSubscriptionDefinition>("model", value => value.RequireNotNull());

                var web = workflowSubscriptionModelHost.HostWeb;
                var list = workflowSubscriptionModelHost.HostList;

                var spObject = GetCurrentWebWorkflowSubscriptioBySourceId(workflowSubscriptionModelHost,
                      workflowSubscriptionModelHost.HostClientContext,
                      list.ParentWeb,
                      list.Id,
                      definition);

                ValidateWorkflowSubscription(modelHost, workflowSubscriptionModelHost.HostClientContext, web, spObject, definition);
            }
        }

        private void ValidateWorkflowSubscription(object modelHost,
            ClientContext clientContext,
            Web web,
            WorkflowSubscription spObject,
            SP2013WorkflowSubscriptionDefinition definition)
        {

            var spObjectContext = spObject.Context;

            //spObjectContext.Load(spObject);
            //spObjectContext.Load(spObject, o => o.PropertyDefinitions);
            //spObjectContext.Load(spObject, o => o.EventSourceId);
            //spObjectContext.Load(spObject, o => o.EventTypes);

            //spObjectContext.ExecuteQuery();

            #region list accos

            var webContext = web.Context;

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Name, o => o.Name);

            // [FALSE] - [WorkflowDisplayName] <!- check DefinitionId, load workflow
            //        [FALSE] - [HistoryListUrl] 
            //        [FALSE] - [TaskListUrl]
            //        [FALSE] - [EventSourceId]
            //        [FALSE] - [EventTypes]

            #region event types

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.EventTypes);

                var hasAllEventTypes = true;

                foreach (var srcEventType in s.EventTypes)
                    if (!d.EventTypes.Contains(srcEventType))
                        hasAllEventTypes = false;

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    IsValid = hasAllEventTypes
                };
            });

            #endregion

            #region validate DefinitionId

            var workflowDefinition = GetWorkflowDefinition(modelHost,
                clientContext,
                web,
                definition);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.WorkflowDisplayName);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    IsValid = d.DefinitionId == workflowDefinition.Id
                };
            });

            #endregion

            #region  validate task and history list

            var taskListId = new Guid(spObject.PropertyDefinitions["TaskListId"]);
            var historyListId = new Guid(spObject.PropertyDefinitions["HistoryListId"]);

            var lists = webContext.LoadQuery<List>(web.Lists.Include(l => l.DefaultViewUrl, l => l.Id));
            webContext.ExecuteQuery();

            var srcTaskList = lists.FirstOrDefault(l => l.Id == taskListId);
            var srcHistoryList = lists.FirstOrDefault(l => l.Id == historyListId);

            var dstTaskList = GetTaskList(web, definition);
            var dstHistoryList = GetHistoryList(web, definition);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.TaskListUrl);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    IsValid = srcTaskList.Id == dstTaskList.Id
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.HistoryListUrl);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    IsValid = srcHistoryList.Id == dstHistoryList.Id
                };
            });

            #endregion

            #endregion
        }
    }
}
