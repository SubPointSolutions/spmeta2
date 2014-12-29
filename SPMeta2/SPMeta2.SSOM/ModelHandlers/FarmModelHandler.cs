using System;
using Microsoft.SharePoint.Administration;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Common;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class FarmModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(FarmDefinition); }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var farmModelHost = modelHost as FarmModelHost;

            action(farmModelHost);

            try
            {
                farmModelHost.HostFarm.Update();
            }
            catch (SPUpdatedConcurrencyException )
            {
                farmModelHost.HostFarm.Update();
            }
            
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var farmModel = model.WithAssertAndCast<FarmDefinition>("model", value => value.RequireNotNull());

            var farm = farmModelHost.HostFarm;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = farm,
                ObjectType = typeof(SPFarm),
                ObjectDefinition = farmModel,
                ModelHost = modelHost
            });


            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = farm,
                ObjectType = typeof(SPFarm),
                ObjectDefinition = farmModel,
                ModelHost = modelHost
            });
        }


        #endregion
    }
}
