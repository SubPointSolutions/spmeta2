using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Regression.Assertion;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSP2013WorkflowSubscriptionDefinitionValidator : SP2013WorkflowSubscriptionDefinitionModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var workflowSubscriptionModelHost = modelHost.WithAssertAndCast<SP2013WorkflowSubscriptionModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SP2013WorkflowSubscriptionDefinition>("model", value => value.RequireNotNull());

            var list = workflowSubscriptionModelHost.HostList;

            var web = workflowSubscriptionModelHost.HostWeb;
            var webContext = web.Context;

            var spObject = GetCurrentWorkflowSubscription(workflowSubscriptionModelHost,
                    workflowSubscriptionModelHost.HostClientContext,
                    workflowSubscriptionModelHost.HostList, definition);

            var assert = ServiceFactory.AssertService
                          .NewAssert(definition, spObject)
                                .ShouldNotBeNull(spObject)
                                .ShouldBeEqual(m => m.Name, o => o.Name);

            // [FALSE] - [WorkflowDisplayName] <!- check DefinitionId, load workflow
            //        [FALSE] - [HistoryListUrl] 
            //        [FALSE] - [TaskListUrl]
            //        [FALSE] - [EventSourceId]
            //        [FALSE] - [EventTypes]

            var spObjectContext = spObject.Context;

            spObjectContext.Load(spObject, o => o.PropertyDefinitions);
            spObjectContext.Load(spObject, o => o.EventSourceId);

            spObjectContext.ExecuteQuery();

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

            var workflowDefinition = GetWorkflowDefinition(workflowSubscriptionModelHost, 
                workflowSubscriptionModelHost.HostClientContext,
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

        }
    }
}
