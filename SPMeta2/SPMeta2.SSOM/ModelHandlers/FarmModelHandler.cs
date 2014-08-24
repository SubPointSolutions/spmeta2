using System;
using Microsoft.SharePoint.Administration;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;

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

            farmModelHost.HostFarm.Update();
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var farm = modelHost.WithAssertAndCast<SPFarm>("modelHost", value => value.RequireNotNull());
            var farmModel = model.WithAssertAndCast<FarmDefinition>("model", value => value.RequireNotNull());
        }


        #endregion
    }
}
