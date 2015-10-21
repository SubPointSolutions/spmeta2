using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ClearRecycleBinModelHandler : SSOMModelHandlerBase
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
                ObjectType = typeof(SPWeb),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            if (typedDefinition.RestoreAll)
            {
                web.RecycleBin.RestoreAll();
            }

            if (typedDefinition.DeleteAll)
            {
                web.RecycleBin.DeleteAll();
            }

            if (typedDefinition.MoveAllToSecondStage)
            {
                web.RecycleBin.MoveAllToSecondStage();
            }

            if (typedDefinition.DeleteAllInSecondStage)
            {
                web.Site.RecycleBin.DeleteAll();
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
    }
}
