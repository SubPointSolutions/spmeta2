using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Services;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class DeleteTopNavigationNodesModelHandler : TopNavigationNodeModelHandler
    {
        #region constructors

        public DeleteTopNavigationNodesModelHandler()
        {
            DeleteNavigationNodesService = new SSOMDeleteNavigationNodesService();
        }

        #endregion

        #region properties
        protected SSOMDeleteNavigationNodesService DeleteNavigationNodesService { get; set; }

        public override Type TargetType
        {
            get { return typeof(DeleteTopNavigationNodesDefinition); }
        }
        #endregion

        #region methods
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<DeleteTopNavigationNodesDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, typedModelHost, typedDefinition);
        }

        private void DeployDefinition(object modelHost, WebModelHost typedModelHost, DeleteTopNavigationNodesDefinition typedDefinition)
        {
            var web = typedModelHost.HostWeb;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            if (typedDefinition.NavigationNodes != null && typedDefinition.NavigationNodes.Any())
            {
                var nodesCollection = GetNavigationNodeCollection(web);

                DeleteNavigationNodesService.DeleteNodesByMatch(typedDefinition, nodesCollection, url =>
                {
                    return ResolveTokenizedUrl(typedModelHost, url);
                });
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });
        }
        #endregion
    }
}
