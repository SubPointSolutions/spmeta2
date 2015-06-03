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
        #region properties

        public override Type TargetType
        {
            get { return typeof(FarmDefinition); }
        }

        public static int ConcurrencyUpdateAttempts = 10;

        #endregion

        #region methods

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var farmModelHost = modelHost as FarmModelHost;

            action(farmModelHost);

            if (farmModelHost.ShouldUpdateHost)
            {
                var count = 0;

                while (count < ConcurrencyUpdateAttempts)
                {
                    try
                    {
                        farmModelHost.HostFarm.Update();
                        farmModelHost.HostFarm = SPFarm.Local;

                        break;
                    }
                    catch (SPUpdatedConcurrencyException)
                    {
                        count++;
                        if (count > ConcurrencyUpdateAttempts)
                        {
                            throw;
                        }
                    }
                }
            }

            farmModelHost.HostFarm = SPFarm.Local;
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
