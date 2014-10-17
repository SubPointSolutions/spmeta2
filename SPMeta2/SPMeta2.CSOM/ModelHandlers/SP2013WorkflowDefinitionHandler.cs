using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class SP2013WorkflowDefinitionHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SP2013WorkflowDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());

            var web = webModelHost.HostWeb;
            var workflowDefinitionModel = model.WithAssertAndCast<SP2013WorkflowDefinition>("model", value => value.RequireNotNull());

            DeployWorkflowDefinition(webModelHost, web, workflowDefinitionModel);
        }

        protected WorkflowDefinition GetCurrentWorkflowDefinition(Web web, SP2013WorkflowDefinition workflowDefinitionModel)
        {
            var clientContext = web.Context;

            var workflowServiceManager = new WorkflowServicesManager(clientContext, web);
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();

            var publishedWorkflows = workflowDeploymentService.EnumerateDefinitions(false);
            clientContext.Load(publishedWorkflows, c => c.Include(
                        w => w.DisplayName,
                        w => w.Id,
                        w => w.Published
                        ));
            clientContext.ExecuteQuery();

            return publishedWorkflows.FirstOrDefault(w => w.DisplayName == workflowDefinitionModel.DisplayName);

        }

        private void DeployWorkflowDefinition(WebModelHost host,
            Web web,
            SP2013WorkflowDefinition workflowDefinitionModel)
        {
            var clientContext = web.Context;

            var workflowServiceManager = new WorkflowServicesManager(clientContext, web);
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();

            var currentWorkflowDefinition = GetCurrentWorkflowDefinition(web, workflowDefinitionModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentWorkflowDefinition,
                ObjectType = typeof(WorkflowDefinition),
                ObjectDefinition = workflowDefinitionModel,
                ModelHost = host
            });

            if (currentWorkflowDefinition == null)
            {
                var workflowDefinition = new WorkflowDefinition(clientContext)
                {
                    Xaml = workflowDefinitionModel.Xaml,
                    DisplayName = workflowDefinitionModel.DisplayName
                };

                clientContext.Load(workflowDefinition);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = workflowDefinition,
                    ObjectType = typeof(WorkflowDefinition),
                    ObjectDefinition = workflowDefinitionModel,
                    ModelHost = host
                });

                workflowDeploymentService.SaveDefinition(workflowDefinition);
                clientContext.ExecuteQuery();

                workflowDeploymentService.PublishDefinition(workflowDefinition.Id);
                clientContext.ExecuteQuery();
            }
            else
            {
                if (workflowDefinitionModel.Override)
                {
                    currentWorkflowDefinition.Xaml = workflowDefinitionModel.Xaml;

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = currentWorkflowDefinition,
                        ObjectType = typeof(WorkflowDefinition),
                        ObjectDefinition = workflowDefinitionModel,
                        ModelHost = host
                    });

                    workflowDeploymentService.SaveDefinition(currentWorkflowDefinition);
                    workflowDeploymentService.PublishDefinition(currentWorkflowDefinition.Id);

                    clientContext.ExecuteQuery();
                }
                else
                {
                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = currentWorkflowDefinition,
                        ObjectType = typeof(WorkflowDefinition),
                        ObjectDefinition = workflowDefinitionModel,
                        ModelHost = host
                    });

                    workflowDeploymentService.PublishDefinition(currentWorkflowDefinition.Id);
                    clientContext.ExecuteQuery();
                }
            }
        }

        #endregion
    }
}
