using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Services;
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
            var workflowSubscriptionModel = model.WithAssertAndCast<SP2013WorkflowSubscriptionDefinition>("model", value => value.RequireNotNull());

            if (modelHost is ListModelHost)
            {
                var listModelHost = (modelHost as ListModelHost);

                var list = listModelHost.HostList;
                var hostclientContext = listModelHost.HostClientContext;

                DeployListWorkflowSubscriptionDefinition(listModelHost, hostclientContext, list, workflowSubscriptionModel);
            }

            else if (modelHost is WebModelHost)
            {
                var webModelHost = (modelHost as WebModelHost);

                var web = webModelHost.HostWeb;
                var hostclientContext = webModelHost.HostClientContext;

                DeployWebWorkflowSubscriptionDefinition(webModelHost, hostclientContext, web, workflowSubscriptionModel);
            }
            else
            {
                throw new SPMeta2NotSupportedException("model host should be of type ListModelHost or WebModelHost");
            }
        }

        protected WorkflowSubscription GetCurrentWebWorkflowSubscriptioBySourceId(
             object host,
             ClientContext hostclientContext,
            Web web,
            Guid eventSourceId,
             SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            var context = web.Context;

            var workflowServiceManager = new WorkflowServicesManager(hostclientContext, web);

            context.Load(web);
            context.Load(web);

            context.ExecuteQueryWithTrace();

            hostclientContext.Load(workflowServiceManager);
            hostclientContext.ExecuteQueryWithTrace();

            var workflowSubscriptionService = workflowServiceManager.GetWorkflowSubscriptionService();

            var subscriptions = workflowSubscriptionService.EnumerateSubscriptionsByEventSource(eventSourceId);

            hostclientContext.Load(subscriptions);
            hostclientContext.ExecuteQueryWithTrace();

            return subscriptions.FirstOrDefault(s => s.Name == workflowSubscriptionModel.Name);
        }


        protected WorkflowDefinition GetWorkflowDefinition(object host,
            ClientContext hostclientContext,
            Web web,
            SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving workflow definition by DisplayName: [{0}]", workflowSubscriptionModel.WorkflowDisplayName);

            var context = hostclientContext;
            //var web = list.ParentWeb;

            var workflowServiceManager = new WorkflowServicesManager(hostclientContext, web);

            //context.Load(web);
            //context.Load(list);

            context.ExecuteQueryWithTrace();

            hostclientContext.Load(workflowServiceManager);
            hostclientContext.ExecuteQueryWithTrace();

            var workflowSubscriptionService = workflowServiceManager.GetWorkflowSubscriptionService();
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();
            var tgtwis = workflowServiceManager.GetWorkflowInstanceService();

            hostclientContext.Load(workflowSubscriptionService);
            hostclientContext.Load(workflowDeploymentService);
            hostclientContext.Load(tgtwis);

            hostclientContext.ExecuteQueryWithTrace();

            var publishedWorkflows = workflowDeploymentService.EnumerateDefinitions(true);

            hostclientContext.Load(publishedWorkflows);
            hostclientContext.ExecuteQueryWithTrace();

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
            ClientContext hostclientContext,
            Web web, SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            // hostclientContext - it must be clientContext, not ClientRuntimeContext - won't work and would give some weirs error with wg publishing
            // use only clientContext instance for the workflow publishing, not ClientRuntimeContext

            var context = web.Context;

            var workflowServiceManager = new WorkflowServicesManager(hostclientContext, web);

            context.Load(web);

            context.ExecuteQueryWithTrace();

            hostclientContext.Load(workflowServiceManager);
            hostclientContext.ExecuteQueryWithTrace();

            var workflowSubscriptionService = workflowServiceManager.GetWorkflowSubscriptionService();
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();
            var tgtwis = workflowServiceManager.GetWorkflowInstanceService();

            hostclientContext.Load(workflowSubscriptionService);
            hostclientContext.Load(workflowDeploymentService);
            hostclientContext.Load(tgtwis);

            hostclientContext.ExecuteQueryWithTrace();

            var publishedWorkflows = workflowDeploymentService.EnumerateDefinitions(true);

            hostclientContext.Load(publishedWorkflows);
            hostclientContext.ExecuteQueryWithTrace();

            var currentWorkflowDefinition = publishedWorkflows.FirstOrDefault(w => w.DisplayName == workflowSubscriptionModel.WorkflowDisplayName);

            if (currentWorkflowDefinition == null)
                throw new Exception(string.Format("Cannot lookup workflow definition with display name: [{0}] on web:[{1}]", workflowSubscriptionModel.WorkflowDisplayName, web.Url));

            var subscriptions = workflowSubscriptionService.EnumerateSubscriptionsByEventSource(web.Id);
            hostclientContext.Load(subscriptions);
            hostclientContext.ExecuteQueryWithTrace();

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

                var newSubscription = new WorkflowSubscription(hostclientContext);

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Setting subscription properties");

                newSubscription.Name = workflowSubscriptionModel.Name;
                newSubscription.DefinitionId = currentWorkflowDefinition.Id;

                newSubscription.EventTypes = workflowSubscriptionModel.EventTypes;
                newSubscription.EventSourceId = web.Id;

                newSubscription.SetProperty("HistoryListId", historyList.Id.ToString());
                newSubscription.SetProperty("TaskListId", taskList.Id.ToString());

                newSubscription.SetProperty("WebId", web.Id.ToString());
                newSubscription.SetProperty("Microsoft.SharePoint.ActivationProperties.WebId", web.Id.ToString());

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
                hostclientContext.ExecuteQueryWithTrace();
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing SP2013 workflow subscription");

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

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling PublishSubscription()");
                workflowSubscriptionService.PublishSubscription(currentSubscription);

                hostclientContext.ExecuteQueryWithTrace();
            }
        }

        private void DeployListWorkflowSubscriptionDefinition(
            object host,
            ClientContext hostclientContext, List list, SP2013WorkflowSubscriptionDefinition workflowSubscriptionModel)
        {
            // hostclientContext - it must be clientContext, not ClientRuntimeContext - won't work and would give some weirs error with wg publishing
            // use only clientContext instance for the workflow publishing, not ClientRuntimeContext

            var context = list.Context;
            var web = list.ParentWeb;

            //This WorkflowServiceManager object is created for current web from client context, 
            //but actually it has to be created for parent web of current web.
            //Otherwise it uses wrong web for provisions with multiple webs
            //var workflowServiceManager = new WorkflowServicesManager(hostclientContext, hostclientContext.Web);

            context.Load(web);
            context.Load(list);

            context.ExecuteQueryWithTrace();

            //This is creation of WorkflowServiceManager with right web
            var workflowServiceManager = new WorkflowServicesManager(hostclientContext, web);

            hostclientContext.Load(workflowServiceManager);
            hostclientContext.ExecuteQueryWithTrace();

            var workflowSubscriptionService = workflowServiceManager.GetWorkflowSubscriptionService();
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();
            var tgtwis = workflowServiceManager.GetWorkflowInstanceService();

            hostclientContext.Load(workflowSubscriptionService);
            hostclientContext.Load(workflowDeploymentService);
            hostclientContext.Load(tgtwis);

            hostclientContext.ExecuteQueryWithTrace();

            var publishedWorkflows = workflowDeploymentService.EnumerateDefinitions(true);

            hostclientContext.Load(publishedWorkflows);
            hostclientContext.ExecuteQueryWithTrace();

            var currentWorkflowDefinition = publishedWorkflows.FirstOrDefault(w => w.DisplayName == workflowSubscriptionModel.WorkflowDisplayName);

            if (currentWorkflowDefinition == null)
                throw new Exception(string.Format("Cannot lookup workflow definition with display name: [{0}] on web:[{1}]", workflowSubscriptionModel.WorkflowDisplayName, web.Url));

            var subscriptions = workflowSubscriptionService.EnumerateSubscriptionsByEventSource(list.Id);
            hostclientContext.Load(subscriptions);
            hostclientContext.ExecuteQueryWithTrace();

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

                var newSubscription = new WorkflowSubscription(hostclientContext);

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Setting subscription properties");

                newSubscription.Name = workflowSubscriptionModel.Name;
                newSubscription.DefinitionId = currentWorkflowDefinition.Id;

                newSubscription.EventTypes = workflowSubscriptionModel.EventTypes;
                newSubscription.EventSourceId = list.Id;

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

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling PublishSubscription()");
                var currentSubscriptionId = workflowSubscriptionService.PublishSubscription(newSubscription);
                hostclientContext.ExecuteQueryWithTrace();
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing SP2013 workflow subscription");

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

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling PublishSubscription()");
                workflowSubscriptionService.PublishSubscription(currentSubscription);

                hostclientContext.ExecuteQueryWithTrace();
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
