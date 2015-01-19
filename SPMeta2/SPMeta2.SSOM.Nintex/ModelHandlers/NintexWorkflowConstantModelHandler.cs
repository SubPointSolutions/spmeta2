using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.Web.Hosting.Administration;
using Nintex.Workflow;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
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
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.Nintex.ModelHandlers
{
    public class NintexWorkflowConstantModelHandler : ModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(NintexWorkflowConstantDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var workflowConstantDefinition = model.WithAssertAndCast<NintexWorkflowConstantDefinition>("model", value => value.RequireNotNull());

            if (modelHost is FarmModelHost)
            {
                DeployNintexWorkflowConstant(modelHost, workflowConstantDefinition, Guid.Empty, Guid.Empty);
            }
            else if (modelHost is SiteModelHost)
            {
                var site = (modelHost as SiteModelHost).HostSite;
                DeployNintexWorkflowConstant(modelHost, workflowConstantDefinition, site.ID, Guid.Empty);
            }
            else if (modelHost is WebModelHost)
            {
                var web = (modelHost as WebModelHost).HostWeb;
                var site = web.Site;

                DeployNintexWorkflowConstant(modelHost, workflowConstantDefinition, site.ID, web.ID);
            }
            else
            {
                throw new SPMeta2NotSupportedException("Unsupported model host. Should be FarmModelHost/SiteModelHost/WebModelHost");
            }

        }

        private void DeployNintexWorkflowConstant(object modelHost,
            NintexWorkflowConstantDefinition definition,
            Guid siteId, Guid webId)
        {
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = null,
                ObjectType = typeof(NintexWorkflowConstantDefinition),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            var constType = (WorkflowConstant.WorkflowConstantType)Enum.Parse(typeof(WorkflowConstant.WorkflowConstantType), definition.Type);
            var constant = new WorkflowConstant(definition.Title, definition.Description, definition.Value,
                definition.Sensitive,
                siteId, webId, constType, definition.IsAdminOnly);

            constant.Update();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = definition,
                ObjectType = typeof(NintexWorkflowConstantDefinition),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

        }

        #endregion
    }
}
