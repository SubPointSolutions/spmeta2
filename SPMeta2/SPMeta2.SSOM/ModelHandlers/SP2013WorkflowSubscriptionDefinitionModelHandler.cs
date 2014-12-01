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
using SPMeta2.Exceptions;
using SPMeta2.Services;
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
            var workflowSubscriptionModel = model.WithAssertAndCast<SP2013WorkflowSubscriptionDefinition>("model", value => value.RequireNotNull());

            if (modelHost is ListModelHost)
            {
                var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
                var list = listModelHost.HostList;

                DeployListWorkflowSubscriptionDefinition(modelHost, list, workflowSubscriptionModel);
            }

            if (modelHost is WebModelHost)
            {
                var listModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
                var web = listModelHost.HostWeb;

                DeployWebWorkflowSubscriptionDefinition(modelHost, web, workflowSubscriptionModel);
            }

            throw new SPMeta2NotSupportedException("model host should be of type ListModelHost or WebModelHost");
        }

        private void DeployWebWorkflowSubscriptionDefinition(
            object host,
            SPWeb list,
            SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            throw new SPMeta2NotImplementedException("Web workflow accosiation is not implemented yet.");
        }

        private void DeployListWorkflowSubscriptionDefinition(
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
                var taskList = web.GetList(SPUrlUtility.CombineUrl(web.Url, workflowSubscriptionModel.TaskListUrl));
                var historyList = web.GetList(SPUrlUtility.CombineUrl(web.Url, workflowSubscriptionModel.HistoryListUrl));

                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new SP2013 workflow subscription");

                var newSubscription = new WorkflowSubscription();

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Setting subscription properties");

                newSubscription.Name = workflowSubscriptionModel.Name;
                newSubscription.DefinitionId = currentWorkflowDefinition.Id;

                newSubscription.EventTypes = workflowSubscriptionModel.EventTypes.ToList();
                newSubscription.EventSourceId = list.ID;

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

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling PublishSubscription()");
                var currentSubscriptionId = workflowSubscriptionService.PublishSubscription(newSubscription);
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing SP2013 workflow subscription");

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

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling PublishSubscription()");
                workflowSubscriptionService.PublishSubscription(currentSubscription);
            }
        }

        #endregion
    }
}
