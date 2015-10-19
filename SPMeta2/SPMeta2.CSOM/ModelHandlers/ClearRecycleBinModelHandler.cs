using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ClearRecycleBinModelHandler : CSOMModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ClearRecycleBinDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<ClearRecycleBinDefinition>("model", value => value.RequireNotNull());

            DeployClearRecycleUnderWeb(modelHost, typedModelHost, typedDefinition);
        }

        private void DeployClearRecycleUnderWeb(object modelHost, WebModelHost typedModelHost, ClearRecycleBinDefinition typedDefinition)
        {
            var web = typedModelHost.HostWeb;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            if (typedDefinition.RestoreAll)
            {
                web.RecycleBin.RestoreAll();
                web.Context.ExecuteQueryWithTrace();
            }

            if (typedDefinition.DeleteAll)
            {
                web.RecycleBin.DeleteAll();
                web.Context.ExecuteQueryWithTrace();
            }

            // TODO, CSOM limitations?
            if (typedDefinition.MoveAllToSecondStage)
            {

            }

            // TODO, CSOM limitations?
            if (typedDefinition.DeleteAllInSecondStage)
            {

            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });
        }
    }
}
