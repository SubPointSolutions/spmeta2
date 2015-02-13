using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint.WorkflowServices;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class SP2013WorkflowSubscriptionDefinitionValidator : SP2013WorkflowSubscriptionDefinitionModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            if (modelHost is WebModelHost)
            {
                var workflowWebSubscriptionModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
                var definition = model.WithAssertAndCast<SP2013WorkflowSubscriptionDefinition>("model", value => value.RequireNotNull());

                var web = workflowWebSubscriptionModelHost.HostWeb;

                var spObject = GetCurrentWebWorkflowSubscriptioBySourceId(workflowWebSubscriptionModelHost,
                       web,
                       web.ID,
                       definition);

                ValidateWorkflowSubscription(modelHost, workflowWebSubscriptionModelHost.HostWeb, spObject, definition);
            }

            if (modelHost is ListModelHost)
            {
                var workflowSubscriptionModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
                var definition = model.WithAssertAndCast<SP2013WorkflowSubscriptionDefinition>("model", value => value.RequireNotNull());

                var list = workflowSubscriptionModelHost.HostList;
                var web = list.ParentWeb;

                var spObject = GetCurrentWebWorkflowSubscriptioBySourceId(workflowSubscriptionModelHost,
                      web,
                      list.ID,
                      definition);

                ValidateWorkflowSubscription(modelHost, web, spObject, definition);
            }
        }

        private void ValidateWorkflowSubscription(object modelHost,
            SPWeb web,
            WorkflowSubscription spObject,
            SP2013WorkflowSubscriptionDefinition definition)
        {

            #region list accos

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

            var workflowDefinition = GetWorkflowDefinition(modelHost, web, definition);

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

            var lists = web.Lists;

            var srcTaskList = lists.OfType<SPList>().FirstOrDefault(l => l.ID == taskListId);
            var srcHistoryList = lists.OfType<SPList>().FirstOrDefault(l => l.ID == historyListId);

            var dstTaskList = GetTaskList(web, definition);
            var dstHistoryList = GetHistoryList(web, definition);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.TaskListUrl);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    IsValid = srcTaskList.ID == dstTaskList.ID
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.HistoryListUrl);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    IsValid = srcHistoryList.ID == dstHistoryList.ID
                };
            });

            #endregion

            #endregion
        }
    }
}
