using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WorkflowServices;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SP2013WorkflowDefinitionHandler : SSOMModelHandlerBase
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

            DeployWorkflowDefinition(modelHost, web, workflowDefinitionModel);
        }

        private void DeployWorkflowDefinition(object host, SPWeb web, SP2013WorkflowDefinition workflowDefinitionModel)
        {
            var workflowServiceManager = new WorkflowServicesManager(web);
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();

            var publishedWorkflows = workflowDeploymentService.EnumerateDefinitions(false);
            var currentWorkflowDefinition = publishedWorkflows.FirstOrDefault(w => w.DisplayName == workflowDefinitionModel.DisplayName);

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
                var workflowDefinition = new WorkflowDefinition()
                {
                    Xaml = workflowDefinitionModel.Xaml,
                    DisplayName = workflowDefinitionModel.DisplayName
                };

                workflowDeploymentService.SaveDefinition(workflowDefinition);

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

                workflowDeploymentService.PublishDefinition(workflowDefinition.Id);
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
                }
            }
        }

        #endregion
    }
}
