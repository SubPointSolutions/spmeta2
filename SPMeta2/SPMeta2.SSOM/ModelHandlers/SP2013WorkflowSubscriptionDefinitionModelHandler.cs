using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WorkflowServices;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SP2013WorkflowSubscriptionDefinitionModelHandler : SSOMModelHandlerBase
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
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var workflowSubscriptionModel = model.WithAssertAndCast<SP2013WorkflowSubscriptionDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            DeployWorkflowSubscriptionDefinition(modelHost, list, workflowSubscriptionModel);
        }

        private void DeployWorkflowSubscriptionDefinition(
            object host,
            SPList list,
            SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            var web = list.ParentWeb;
            var workflowServiceManager = new WorkflowServicesManager(list.ParentWeb);

            var workflowSubscriptionService = workflowServiceManager.GetWorkflowSubscriptionService();
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();
            var tgtwis = workflowServiceManager.GetWorkflowInstanceService();

            var publishedWorkflows = workflowDeploymentService.EnumerateDefinitions(true);

            var currentWorkflowDefinition = publishedWorkflows.FirstOrDefault(w => w.DisplayName == workflowSubscriptionModel.WorkflowDisplayName);

            if (currentWorkflowDefinition == null)
                throw new Exception(string.Format("Cannot lookup workflow definition with display name: [{0}] on web:[{1}]", workflowSubscriptionModel.WorkflowDisplayName, web.Url));

            var subscriptions = workflowSubscriptionService.EnumerateSubscriptionsByList(list.ID);

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
                var newSubscription = new WorkflowSubscription();

                newSubscription.Name = workflowSubscriptionModel.Name;
                newSubscription.DefinitionId = currentWorkflowDefinition.Id;

                newSubscription.EventTypes = workflowSubscriptionModel.EventTypes.ToList();
                newSubscription.EventSourceId = list.ID;

                // lookup task and history lists, probaly need to think ab otehr strategy
                var taskList = web.GetList(SPUrlUtility.CombineUrl(web.Url, workflowSubscriptionModel.TaskListUrl));
                var historyList = web.GetList(SPUrlUtility.CombineUrl(web.Url, workflowSubscriptionModel.HistoryListUrl));

                newSubscription.SetProperty("HistoryListId", historyList.ID.ToString());
                newSubscription.SetProperty("TaskListId", taskList.ID.ToString());

                newSubscription.SetProperty("ListId", list.ID.ToString());
                newSubscription.SetProperty("Microsoft.SharePoint.ActivationProperties.ListId", list.ID.ToString());

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
            }
            else
            {
                currentSubscription.EventTypes = workflowSubscriptionModel.EventTypes.ToList();

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
            }
        }

        #endregion
    }
}
