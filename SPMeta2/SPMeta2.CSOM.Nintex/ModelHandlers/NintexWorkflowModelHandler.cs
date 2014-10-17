using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Nintex.Consts;
using SPMeta2.Nintex.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;

namespace SPMeta2.CSOM.Nintex.ModelHandlers
{
    public class NintexWorkflowModelHandler : ModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(NintexWorkflowDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var workflowDefinition = model.WithAssertAndCast<NintexWorkflowDefinition>("model", value => value.RequireNotNull());

            // Nintex workflow web service updates current list with 4 versions
            // We can't really call SPList.Update for the current list as we have 'Save conflict exception'
            // We say 'don't update list' to the parent model host, so we safe and can make other list operations
            listModelHost.ShouldUpdateHost = false;

            DeployNintexWorkflow(listModelHost, workflowDefinition);
        }

        private void DeployNintexWorkflow(ListModelHost listModelHost, NintexWorkflowDefinition workflowDefinition)
        {
            var list = listModelHost.HostList;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = workflowDefinition,
                ObjectType = typeof(NintexWorkflowDefinition),
                ObjectDefinition = workflowDefinition,
                ModelHost = list
            });

            using (var nintexService = new NintexWorkflowService.NintexWorkflowWS())
            {
                nintexService.Url = list.ParentWeb.Url + "/" + NintexUrls.WorkflowServiceUrl;
                nintexService.PreAuthenticate = true;
                nintexService.Credentials = System.Net.CredentialCache.DefaultCredentials;

                var xmlnw = Encoding.UTF8.GetString(workflowDefinition.WorkflowXml);

                var result = nintexService.PublishFromNWFXml(xmlnw, list.Title, workflowDefinition.WorkflowName, true);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = workflowDefinition,
                ObjectType = typeof(NintexWorkflowDefinition),
                ObjectDefinition = workflowDefinition,
                ModelHost = list
            });

        }


        #endregion
    }
}
