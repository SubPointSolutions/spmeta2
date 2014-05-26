using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class SP2013WorkflowDefinitionHandler : ModelHandlerBase
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

            DeployWorkflowDefinition(web, workflowDefinitionModel);
        }

        private void DeployWorkflowDefinition(Web web, SP2013WorkflowDefinition workflowDefinitionModel)
        {
            var clientContext = web.Context;

            var workflowServiceManager = new WorkflowServicesManager(clientContext, web);
            var workflowDeploymentService = workflowServiceManager.GetWorkflowDeploymentService();

            var publishedWorkflows = workflowDeploymentService.EnumerateDefinitions(false);
            clientContext.Load(publishedWorkflows, c => c.Include(
                        w => w.DisplayName,
                        w => w.Id
                        ));
            clientContext.ExecuteQuery();

            var currentWorkflowDefinition = publishedWorkflows.FirstOrDefault(w => w.DisplayName == workflowDefinitionModel.DisplayName);

            if (currentWorkflowDefinition == null)
            {
                var workflowDefinition = new WorkflowDefinition(clientContext)
                {
                    Xaml = workflowDefinitionModel.Xaml,
                    DisplayName = workflowDefinitionModel.DisplayName
                };

                clientContext.Load(workflowDefinition);
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
                    clientContext.ExecuteQuery();

                    workflowDeploymentService.PublishDefinition(currentWorkflowDefinition.Id);
                    clientContext.ExecuteQuery();
                }
                else
                {
                    workflowDeploymentService.PublishDefinition(currentWorkflowDefinition.Id);
                    clientContext.ExecuteQuery();
                }
            }
        }

        #endregion
    }
}
