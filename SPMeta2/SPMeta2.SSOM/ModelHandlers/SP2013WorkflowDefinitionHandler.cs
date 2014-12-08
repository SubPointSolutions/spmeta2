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
using SPMeta2.Services;
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

        protected WorkflowDefinition GetCurrentWorkflowDefinition(SPWeb web,
            SP2013WorkflowDefinition workflowDefinitionModel)
        {
            var workflowServiceManager = new WorkflowServicesManager(web);
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();

            var publishedWorkflows = workflowDeploymentService.EnumerateDefinitions(false);
            return publishedWorkflows.FirstOrDefault(w => w.DisplayName == workflowDefinitionModel.DisplayName);
        }

        private void DeployWorkflowDefinition(object host, SPWeb web, SP2013WorkflowDefinition workflowDefinitionModel)
        {
            var workflowServiceManager = new WorkflowServicesManager(web);
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
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new SP2013 workflow definition");

                var workflowDefinition = new WorkflowDefinition()
                {
                    Xaml = workflowDefinitionModel.Xaml,
                    DisplayName = workflowDefinitionModel.DisplayName
                };

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling SaveDefinition()");
                var wfId = workflowDeploymentService.SaveDefinition(workflowDefinition);

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

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling PublishDefinition()");
                workflowDeploymentService.PublishDefinition(wfId);
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing SP2013 workflow definition");

                if (workflowDefinitionModel.Override)
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Override = true. Overriding workflow definition");

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

                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling SaveDefinition()");
                    var wfId = workflowDeploymentService.SaveDefinition(currentWorkflowDefinition);

                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling PublishDefinition()");
                    workflowDeploymentService.PublishDefinition(wfId);
                }
                else
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Override = false. Skipping workflow definition");

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

                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling PublishDefinition()");
                    workflowDeploymentService.PublishDefinition(currentWorkflowDefinition.Id);
                }
            }
        }

        #endregion
    }
}
