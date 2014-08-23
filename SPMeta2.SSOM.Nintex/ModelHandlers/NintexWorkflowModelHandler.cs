using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Nintex.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.Nintex.Consts;

namespace SPMeta2.SSOM.Nintex.ModelHandlers
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
            var list = modelHost.WithAssertAndCast<SPList>("modelHost", value => value.RequireNotNull());
            var workflowDefinition = model.WithAssertAndCast<NintexWorkflowDefinition>("model", value => value.RequireNotNull());

            DeployNintexWorkflow(list, workflowDefinition);
        }

        private void DeployNintexWorkflow(SPList list, NintexWorkflowDefinition workflowDefinition)
        {
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
                nintexService.Url = SPUrlUtility.CombineUrl(list.ParentWeb.Url, NintexUrls.WorkflowServiceUrl);
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
