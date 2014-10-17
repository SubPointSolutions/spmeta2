using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class SP2013WorkflowSubscriptionDefinitionModelHandler : CSOMModelHandlerBase
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

            DeployWorkflowSubscriptionDefinition(workflowSubscriptionModelHost, hostClientContext, list, workflowSubscriptionModel);
        }


        protected WorkflowSubscription GetCurrentWorkflowSubscription(
             SP2013WorkflowSubscriptionModelHost host,
             ClientContext hostClientContext, List list, SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            var context = list.Context;
            var web = list.ParentWeb;

            var workflowServiceManager = new WorkflowServicesManager(hostClientContext, hostClientContext.Web);

            context.Load(web);
            context.Load(list);

            context.ExecuteQuery();

            hostClientContext.Load(workflowServiceManager);
            hostClientContext.ExecuteQuery();

            var workflowSubscriptionService = workflowServiceManager.GetWorkflowSubscriptionService();

            var subscriptions = workflowSubscriptionService.EnumerateSubscriptionsByList(list.Id);

            hostClientContext.Load(subscriptions);
            hostClientContext.ExecuteQuery();

            InvokeOnModelEvent<SP2013WorkflowSubscriptionDefinition, WorkflowSubscription>(null, ModelEventType.OnUpdating);

            return subscriptions.FirstOrDefault(s => s.Name == workflowSubscriptionModel.Name);
        }

        protected WorkflowDefinition GetWorkflowDefinition(SP2013WorkflowSubscriptionModelHost host,
             ClientContext hostClientContext,
            SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            var context = host.HostList.Context; ;
            //var web = list.ParentWeb;

            var workflowServiceManager = new WorkflowServicesManager(hostClientContext, hostClientContext.Web);

            //context.Load(web);
            //context.Load(list);

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

            return publishedWorkflows.FirstOrDefault(w => w.DisplayName == workflowSubscriptionModel.WorkflowDisplayName);
        }

        private void DeployWorkflowSubscriptionDefinition(
            SP2013WorkflowSubscriptionModelHost host,
            ClientContext hostClientContext, List list, SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
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

            InvokeOnModelEvent<SP2013WorkflowSubscriptionDefinition, WorkflowSubscription>(null, ModelEventType.OnUpdating);

            var currentSubscription = subscriptions.FirstOrDefault(s => s.Name == workflowSubscriptionModel.Name);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentSubscription,
                ObjectType = typeof(WorkflowSubscription),
                ObjectDefinition = workflowSubscriptionModel,
                ModelHost = host
            });

            if (currentSubscription == null)
            {
                var newSubscription = new WorkflowSubscription(hostClientContext);

                newSubscription.Name = workflowSubscriptionModel.Name;
                newSubscription.DefinitionId = currentWorkflowDefinition.Id;

                newSubscription.EventTypes = workflowSubscriptionModel.EventTypes;
                newSubscription.EventSourceId = list.Id;

                // lookup task and history lists, probaly need to think ab otehr strategy
                var taskList = GetTaskList(web, workflowSubscriptionModel);
                var historyList = GetHistoryList(web, workflowSubscriptionModel);

                newSubscription.SetProperty("HistoryListId", historyList.Id.ToString());
                newSubscription.SetProperty("TaskListId", taskList.Id.ToString());

                newSubscription.SetProperty("ListId", list.Id.ToString());
                newSubscription.SetProperty("Microsoft.SharePoint.ActivationProperties.ListId", list.Id.ToString());

                // to be able to change HistoryListId, TaskListId, ListId
                InvokeOnModelEvent<SP2013WorkflowSubscriptionDefinition, WorkflowSubscription>(newSubscription, ModelEventType.OnUpdated);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newSubscription,
                    ObjectType = typeof(WorkflowSubscription),
                    ObjectDefinition = workflowSubscriptionModel,
                    ModelHost = host
                });

                var currentSubscriptionId = workflowSubscriptionService.PublishSubscription(newSubscription);
                hostClientContext.ExecuteQuery();
            }
            else
            {
                currentSubscription.EventTypes = workflowSubscriptionModel.EventTypes;

                InvokeOnModelEvent<SP2013WorkflowSubscriptionDefinition, WorkflowSubscription>(currentSubscription, ModelEventType.OnUpdated);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentSubscription,
                    ObjectType = typeof(WorkflowSubscription),
                    ObjectDefinition = workflowSubscriptionModel,
                    ModelHost = host
                });

                workflowSubscriptionService.PublishSubscription(currentSubscription);
                hostClientContext.ExecuteQuery();
            }
        }

        protected List GetTaskList(Web web, SP2013WorkflowSubscriptionDefinition definition)
        {
            return WebExtensions.QueryAndGetListByUrl(web, definition.TaskListUrl);
        }

        protected List GetHistoryList(Web web, SP2013WorkflowSubscriptionDefinition definition)
        {
            return WebExtensions.QueryAndGetListByUrl(web, definition.HistoryListUrl);
        }

        #endregion
    }
}
