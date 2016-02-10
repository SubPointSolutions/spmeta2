using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Workflow;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WorkflowAssociationModelHandler : SSOMModelHandlerBase
    {
        #region propeties
        public override Type TargetType
        {
            get { return typeof(WorkflowAssociationDefinition); }
        }
        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var workflowAssociationModel = model.WithAssertAndCast<WorkflowAssociationDefinition>("model", value => value.RequireNotNull());

            if (modelHost is ListModelHost)
            {
                var listModelHost = (modelHost as ListModelHost);
                var list = listModelHost.HostList;

                DeployListWorkflowAssociationDefinition(listModelHost, list, workflowAssociationModel);
            }

            else if (modelHost is WebModelHost)
            {
                var webModelHost = (modelHost as WebModelHost);
                var web = webModelHost.HostWeb;

                DeployWebWorkflowAssociationDefinition(webModelHost, web, workflowAssociationModel);
            }
            else
            {
                throw new SPMeta2NotSupportedException("model host should be of type ListModelHost or WebModelHost");
            }
        }

        protected SPWorkflowAssociation GetWorfklowAssotiation(object modelHost)
        {
            // TODO
            return null;
        }

        private void DeployWebWorkflowAssociationDefinition(WebModelHost webModelHost, Microsoft.SharePoint.SPWeb web, WorkflowAssociationDefinition workflowAssociationModel)
        {
            // TODO
        }

        private void DeployListWorkflowAssociationDefinition(ListModelHost listModelHost, Microsoft.SharePoint.SPList list, WorkflowAssociationDefinition workflowAssociationModel)
        {
            // TODO
        }

        #endregion
    }
}
