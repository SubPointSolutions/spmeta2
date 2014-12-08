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

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var workflowSubscriptionModel = model.WithAssertAndCast<SP2013WorkflowSubscriptionDefinition>("model", value => value.RequireNotNull());

            if (modelHost is ListModelHost)
            {
                var listModelHost = (modelHost as ListModelHost);
                var list = listModelHost.HostList;

                DeployListWorkflowSubscriptionDefinition(listModelHost, list, workflowSubscriptionModel);
            }

            else if (modelHost is WebModelHost)
            {
                var webModelHost = (modelHost as WebModelHost);
                var web = webModelHost.HostWeb;

                DeployWebWorkflowSubscriptionDefinition(webModelHost, web, workflowSubscriptionModel);
            }
            else
            {
                throw new SPMeta2NotSupportedException("model host should be of type ListModelHost or WebModelHost");
            }
        }

        protected WorkflowSubscription GetCurrentWebWorkflowSubscriptioBySourceId(
             object host,
             SPWeb web,
             Guid eventSourceId,
             SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            var workflowServiceManager = new WorkflowServicesManager(web);
            var workflowSubscriptionService = workflowServiceManager.GetWorkflowSubscriptionService();
            var subscriptions = workflowSubscriptionService.EnumerateSubscriptionsByEventSource(eventSourceId);

            return subscriptions.FirstOrDefault(s => s.Name == workflowSubscriptionModel.Name);
        }


        protected WorkflowDefinition GetWorkflowDefinition(object host,
            SPWeb web,
            SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving workflow definition by DisplayName: [{0}]", workflowSubscriptionModel.WorkflowDisplayName);
            var workflowServiceManager = new WorkflowServicesManager(web);

            var workflowSubscriptionService = workflowServiceManager.GetWorkflowSubscriptionService();
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();
            var tgtwis = workflowServiceManager.GetWorkflowInstanceService();

            var publishedWorkflows = workflowDeploymentService.EnumerateDefinitions(true);

            var result = publishedWorkflows.FirstOrDefault(w => w.DisplayName == workflowSubscriptionModel.WorkflowDisplayName);

            if (result == null)
            {
                TraceService.ErrorFormat((int)LogEventId.ModelProvisionCoreCall,
                    "Cannot find workflow definition with DisplayName: [{0}]. Provision might break.",
                    workflowSubscriptionModel.WorkflowDisplayName);
            }

            return result;
        }

        private void DeployWebWorkflowSubscriptionDefinition(
            object host,
            SPWeb web,
            SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            var workflowServiceManager = new WorkflowServicesManager(web);

            var workflowSubscriptionService = workflowServiceManager.GetWorkflowSubscriptionService();
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();
            var tgtwis = workflowServiceManager.GetWorkflowInstanceService();

            var publishedWorkflows = workflowDeploymentService.EnumerateDefinitions(true);

            var currentWorkflowDefinition = publishedWorkflows.FirstOrDefault(w => w.DisplayName == workflowSubscriptionModel.WorkflowDisplayName);

            if (currentWorkflowDefinition == null)
                throw new Exception(string.Format("Cannot lookup workflow definition with display name: [{0}] on web:[{1}]", workflowSubscriptionModel.WorkflowDisplayName, web.Url));

            // EnumerateSubscriptionsByEventSource() somehow throws an exception
            //var subscriptions = workflowSubscriptionService.EnumerateSubscriptionsByEventSource(web.ID);
            var subscriptions = workflowSubscriptionService.EnumerateSubscriptions().Where(s => s.EventSourceId == web.ID);

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
                var taskList = GetTaskList(web, workflowSubscriptionModel);
                var historyList = GetHistoryList(web, workflowSubscriptionModel);

                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new SP2013 workflow subscription");

                var newSubscription = new WorkflowSubscription();

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Setting subscription properties");

                newSubscription.Name = workflowSubscriptionModel.Name;
                newSubscription.DefinitionId = currentWorkflowDefinition.Id;

                newSubscription.EventTypes = new List<string>(workflowSubscriptionModel.EventTypes);
                newSubscription.EventSourceId = web.ID;

                newSubscription.SetProperty("HistoryListId", historyList.ID.ToString());
                newSubscription.SetProperty("TaskListId", taskList.ID.ToString());

                newSubscription.SetProperty("WebId", web.ID.ToString());
                newSubscription.SetProperty("Microsoft.SharePoint.ActivationProperties.WebId", web.ID.ToString());

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

                currentSubscription.EventTypes = new List<string>(workflowSubscriptionModel.EventTypes);

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

        private void DeployListWorkflowSubscriptionDefinition(
            object host,
            SPList list,
            SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            var web = list.ParentWeb;
            var workflowServiceManager = new WorkflowServicesManager(web);

            var workflowSubscriptionService = workflowServiceManager.GetWorkflowSubscriptionService();
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();
            var tgtwis = workflowServiceManager.GetWorkflowInstanceService();

            var publishedWorkflows = workflowDeploymentService.EnumerateDefinitions(true);

            var currentWorkflowDefinition = publishedWorkflows.FirstOrDefault(w => w.DisplayName == workflowSubscriptionModel.WorkflowDisplayName);

            if (currentWorkflowDefinition == null)
                throw new Exception(string.Format("Cannot lookup workflow definition with display name: [{0}] on web:[{1}]", workflowSubscriptionModel.WorkflowDisplayName, web.Url));

            var subscriptions = workflowSubscriptionService.EnumerateSubscriptionsByEventSource(list.ID);

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
                var taskList = GetTaskList(web, workflowSubscriptionModel);
                var historyList = GetHistoryList(web, workflowSubscriptionModel);

                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new SP2013 workflow subscription");

                var newSubscription = new WorkflowSubscription();

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Setting subscription properties");

                newSubscription.Name = workflowSubscriptionModel.Name;
                newSubscription.DefinitionId = currentWorkflowDefinition.Id;

                newSubscription.EventTypes = new List<string>(workflowSubscriptionModel.EventTypes);
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

                currentSubscription.EventTypes = new List<string>(workflowSubscriptionModel.EventTypes);

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

        protected SPList GetTaskList(SPWeb web, SP2013WorkflowSubscriptionDefinition definition)
        {
            return web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, definition.TaskListUrl));
        }

        protected SPList GetHistoryList(SPWeb web, SP2013WorkflowSubscriptionDefinition definition)
        {
            return web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, definition.HistoryListUrl));
        }

        #endregion

        #endregion
    }
}
