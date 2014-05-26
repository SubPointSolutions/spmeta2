using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class SP2013WorkflowSubscriptionDefinitionModelHandler : ModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SP2013WorkflowSubscriptionDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var workflowSubscriptionModelHost = modelHost.WithAssertAndCast<SP2013WorkflowSubscriptionModelHost>("modelHost", value => value.RequireNotNull());

            var list = workflowSubscriptionModelHost.HostList;
            var hostClientContext = workflowSubscriptionModelHost.HostClientContext;

            var workflowSubscriptionModel = model.WithAssertAndCast<SP2013WorkflowSubscriptionDefinition>("model", value => value.RequireNotNull());

            DeployWorkflowSubscriptionDefinition(hostClientContext, list, workflowSubscriptionModel);
        }

        private void DeployWorkflowSubscriptionDefinition(ClientContext hostClientContext, List list, SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            // hostClientContext - it must be ClientContext, not ClientRuntimeContext - won't work and would give some weirs error with wg publishing
            // use only ClientContext instance for the workflow pubnlishing, not ClientRuntimeContext

            var context = list.Context;
            var web = list.ParentWeb;

            var workflowServiceManager = new WorkflowServicesManager(hostClientContext, hostClientContext.Web);

            context.Load(web);
            context.Load(list);

            context.ExecuteQuery();

            hostClientContext.Load(workflowServiceManager);
            hostClientContext.ExecuteQuery();

            var workflowSubscriptionService = workflowServiceManager.GetWorkflowSubscriptionService();
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();
            var tgtwis = workflowServiceManager.GetWorkflowInstanceService();

            hostClientContext.Load(workflowSubscriptionService);
            hostClientContext.Load(workflowDeploymentService);
            hostClientContext.Load(tgtwis);

            hostClientContext.ExecuteQuery();

            var publishedWorkflows = workflowDeploymentService.EnumerateDefinitions(true);

            hostClientContext.Load(publishedWorkflows);
            hostClientContext.ExecuteQuery();

            var currentWorkflowDefinition = publishedWorkflows.FirstOrDefault(w => w.DisplayName == workflowSubscriptionModel.WorkflowDisplayName);

            if (currentWorkflowDefinition == null)
                throw new Exception(string.Format("Cannot lookup workflow definition with display name: [{0}] on web:[{1}]", workflowSubscriptionModel.WorkflowDisplayName, web.Url));

            var subscriptions = workflowSubscriptionService.EnumerateSubscriptionsByList(list.Id);
            hostClientContext.Load(subscriptions);
            hostClientContext.ExecuteQuery();

            InvokeOnModelEvents<SP2013WorkflowSubscriptionDefinition, WorkflowSubscription>(null, ModelEventType.OnUpdating);

            var currentSubscription = subscriptions.FirstOrDefault(s => s.Name == workflowSubscriptionModel.Name);

            if (currentSubscription == null)
            {
                var newSubscription = new WorkflowSubscription(hostClientContext);

                newSubscription.Name = workflowSubscriptionModel.Name;
                newSubscription.DefinitionId = currentWorkflowDefinition.Id;

                newSubscription.EventTypes = workflowSubscriptionModel.EventTypes;
                newSubscription.EventSourceId = list.Id;

                // lookup task and history lists, probaly need to think ab otehr strategy
                var taskList = WebExtensions.QueryAndGetListByUrl(web, workflowSubscriptionModel.TaskListUrl);
                var historyList = WebExtensions.QueryAndGetListByUrl(web, workflowSubscriptionModel.HistoryListUrl);

                newSubscription.SetProperty("HistoryListId", historyList.Id.ToString());
                newSubscription.SetProperty("TaskListId", taskList.Id.ToString());

                newSubscription.SetProperty("ListId", list.Id.ToString());
                newSubscription.SetProperty("Microsoft.SharePoint.ActivationProperties.ListId", list.Id.ToString());

                // to be able to change HistoryListId, TaskListId, ListId
                InvokeOnModelEvents<SP2013WorkflowSubscriptionDefinition, WorkflowSubscription>(newSubscription, ModelEventType.OnUpdated);

                var currentSubscriptionId = workflowSubscriptionService.PublishSubscription(newSubscription);
                hostClientContext.ExecuteQuery();
            }
            else
            {
                currentSubscription.EventTypes = workflowSubscriptionModel.EventTypes;

                InvokeOnModelEvents<SP2013WorkflowSubscriptionDefinition, WorkflowSubscription>(currentSubscription, ModelEventType.OnUpdated);

                workflowSubscriptionService.PublishSubscription(currentSubscription);
                hostClientContext.ExecuteQuery();
            }
        }

        #endregion
    }
}
